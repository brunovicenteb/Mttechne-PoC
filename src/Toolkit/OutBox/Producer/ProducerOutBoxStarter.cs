using MassTransit;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Mttechne.Toolkit.TransactionalOutBox;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Toolkit.OutBox.Producer;

internal class ProducerOutBoxStarter<T> : OutBoxStarter where T : OutBoxDbContext
{
    internal ProducerOutBoxStarter(WebApplicationBuilder builder, string dbTypeVarName, bool recreateDb, string dbConnectionVarName,
        string retryCountVarName, string retryIntevalInMillisecondsVarName)
        : base(builder, dbTypeVarName, dbConnectionVarName, retryCountVarName, retryIntevalInMillisecondsVarName)
    {
        _RecreateDB = recreateDb;
    }

    private readonly bool _RecreateDB;

    protected override string TelemetryName => "producer";

    protected override void DoUseDatabase(string stringConnection)
    {
        Builder.Services.AddDbContext<T>(o =>
        {
            switch (DbType)
            {
                case DatabaseType.InMemory:
                    UseInMemory(stringConnection, o);
                    break;
                case DatabaseType.SqlServer:
                    UseSqlServer(stringConnection, o);
                    break;
                case DatabaseType.Postgres:
                    UsePostgress(stringConnection, o);
                    break;
                default:
                    throw new NotImplementedException($"DbType {DbType} not supported yet on UseDatabase.");
            }
        });
        Builder.Services.AddHostedService(o => new RecreateDbHostedService<T>(_RecreateDB, o));
    }

    protected override void DoUseRabbitMq(string host)
    {
        Builder.Services.AddMassTransit(busRegistration =>
        {
            busRegistration.SetKebabCaseEndpointNameFormatter();
            busRegistration.AddEntityFrameworkOutbox<T>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);
                switch (DbType)
                {
                    case DatabaseType.SqlServer:
                        o.UseSqlServer();
                        break;
                    case DatabaseType.Postgres:
                        o.UsePostgres();
                        break;
                    default:
                        throw new NotImplementedException($"DbType {DbType} not supported yet on UseRabbitMq.");
                }
                o.UseBusOutbox();
            });
            busRegistration.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(host);
                cfg.AutoStart = true;
            });
        });
        Builder.Services.AddHostedService(o =>
        {
            var contextShell = Activator.CreateInstance(typeof(T)) as T;
            return contextShell.CreateSagaAdapter(o, host);
        });
    }

    protected override void DoUseHarness()
    {
        Builder.Services.AddMassTransitTestHarness(busRegistration =>
        {
            if (DbType == DatabaseType.InMemory)
                busRegistration.AddInMemoryInboxOutbox();
            else
            {
                busRegistration.AddEntityFrameworkOutbox<T>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    if (DbType == DatabaseType.SqlServer)
                        o.UseSqlServer();
                    else
                        o.UsePostgres();
                    o.UseBusOutbox();
                });
            }
        });
    }

    private static void UseInMemory(string stringConnection, DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(stringConnection);
    }

    private void UsePostgress(string stringConnection, DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(stringConnection, options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
            options.MigrationsHistoryTable($"__{nameof(T)}");
            options.EnableRetryOnFailure(5);
            options.MinBatchSize(1);
        });
    }

    private void UseSqlServer(string stringConnection, DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(stringConnection, options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
            options.MigrationsHistoryTable($"__{nameof(T)}");
            options.EnableRetryOnFailure(5);
            options.MinBatchSize(1);
        });
    }
}