using Serilog;
using Mttechne.Toolkit.Rabbit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Toolkit.OutBox.Producer;

public abstract class SagaAdaptorService : DisposableObject, IHostedService
{
    public SagaAdaptorService(IServiceProvider serviceProvider, string host, string queueVarName)
    {
        Logger = Log.ForContext(GetType());
        _Host = host;
        _ServiceProvider = serviceProvider;
        _QueueName = EnvironmentReader.Read<string>(queueVarName, varEmptyError:
            $"Unable to identify Translator Queue Name on {queueVarName} variable. Unable to start Saga Translator Service.");
    }

    private readonly string _Host;
    private readonly string _QueueName;
    private RabbitConsumer _Consumer;
    protected readonly ILogger Logger;
    protected readonly IServiceProvider _ServiceProvider;

    protected abstract Task<bool> OnMessageReceived(IServiceScope serviceScope, string receivedMessage);

    protected override sealed void DoDispose()
    {
        _Consumer?.Dispose();
        _Consumer = null;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.Information($"Start monitor Queue to receive Saga Start Requests on Queue: {_QueueName}");
        _Consumer = new RabbitConsumer(_Host, _QueueName, DoMessageReceived);
        _Consumer.Start();
        await Task.CompletedTask;
    }

    private async Task<bool> DoMessageReceived(string receivedMessage)
    {
        using var serviceScope = _ServiceProvider.CreateScope();
        return await OnMessageReceived(serviceScope, receivedMessage);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _Consumer?.Stop();
        await Task.CompletedTask;
    }
}