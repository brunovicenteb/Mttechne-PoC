using Serilog;
using MassTransit;
using MassTransit.RetryPolicies;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Toolkit.OutBox.Producer;

public class RecreateDbHostedService<T> : IHostedService where T : DbContext
{
    private readonly ILogger _Logger;
    private readonly IServiceProvider _ScopeFactory;
    private readonly bool _ForceRecreate;
    private T _Context;

    public RecreateDbHostedService(bool forceRecreate, IServiceProvider scopeFactory)
    {
        _Logger = Log.ForContext<RecreateDbHostedService<T>>();
        _ForceRecreate = forceRecreate;
        _ScopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _Logger.Information("Applying migrations for {DbContext}", TypeCache<T>.ShortName);
        await Retry.Interval(20, TimeSpan.FromSeconds(3)).Retry(async () =>
        {
            var scope = _ScopeFactory.CreateScope();
            try
            {
                _Context = scope.ServiceProvider.GetRequiredService<T>();
                if (_ForceRecreate)
                    await _Context.Database.EnsureDeletedAsync(cancellationToken);
                await _Context.Database.EnsureCreatedAsync(cancellationToken);
                //await _Context.Database.MigrateAsync();
                _Logger.Information("Migrations completed for {DbContext}", TypeCache<T>.ShortName);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync();
                else
                    scope.Dispose();
            }
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}