using Serilog;
using OpenTelemetry;
using Serilog.Events;
using System.Diagnostics;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Builder;
using Mttechne.Toolkit.MessageBroker;
using Mttechne.Toolkit.TransactionalOutBox;
using Microsoft.Extensions.DependencyInjection;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.OutBox;

internal abstract class OutBoxStarter : ILogable, ITelemetreable, IDatabaseable, IBrokeable
{
    internal OutBoxStarter(WebApplicationBuilder builder, string dbTypeVarName, string dbConnectionVarName,
        string retryCountVarName, string retryIntevalInMillisecondsVarName)
    {
        Builder = builder;
        _DbTypeVarName = dbTypeVarName;
        _DbConnectionVarName = dbConnectionVarName;
        int retryCount = EnvironmentReader.Read(retryCountVarName, defaultValue: 5);
        int retryIntevalInMilliseconds = EnvironmentReader.Read(retryIntevalInMillisecondsVarName, defaultValue: 100);
        BrokerConsumer.SetRetryParameters(retryCount, retryIntevalInMilliseconds);
    }

    public WebApplicationBuilder Builder { get; private set; }
    private readonly string _DbTypeVarName;
    private readonly string _DbConnectionVarName;
    protected DatabaseType? DbType;
    protected abstract string TelemetryName { get; }
    protected abstract void DoUseDatabase(string stringConnection);
    protected abstract void DoUseRabbitMq(string host);
    protected abstract void DoUseHarness();

    public IBrokeable UseDatabase()
    {
        var strDbType = EnvironmentReader.Read<string>(_DbTypeVarName, varEmptyError:
            $"Unable to identify DbType on {_DbTypeVarName} variable. Unable to start Transactional OutBox.");
        if (!Enum.TryParse(strDbType, out DatabaseType dbType))
            throw new NotImplementedException($"Invalid DbType ({strDbType}) informed on {_DbTypeVarName} variable. Unable to start Transactional OutBox.");
        var db = Enum.GetName(dbType);
        var stringConnection = EnvironmentReader.Read<string>(_DbConnectionVarName, varEmptyError:
            $"Unable to identify {db} Connection on {_DbConnectionVarName} variable. Unable to start Transactional OutBox.");
        DbType = dbType;
        DoUseDatabase(stringConnection);
        return this;
    }

    public IOutboxBuilder UseRabbitMq(string rabbitMqVariableName = "RABBIT_MQ")
    {
        var host = EnvironmentReader.Read<string>(rabbitMqVariableName, varEmptyError:
            $"Unable to identify RabbitMq Host on {rabbitMqVariableName} variable. Unable to start Transactional OutBox.");
        DoUseRabbitMq(host);
        return this;
    }

    public IOutboxBuilder UseHarness()
    {
        DoUseHarness();
        return this;
    }

    public ITelemetreable UseSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        Builder.Host.UseSerilog();
        return this;
    }

    public IDatabaseable UseTelemetry(string telemetryHostVariableName = "TELEMETRY_HOST")
    {
        var host = EnvironmentReader.Read<string>(telemetryHostVariableName, varEmptyError:
            $"Unable to identify Open Telemetry Host on {telemetryHostVariableName} variable. Unable to start Transactional OutBox.");
        Builder.Services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(TelemetryName)
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector())
                .AddSource("MassTransit")
                .AddJaegerExporter(o =>
                {
                    o.AgentHost = host;
                    o.AgentPort = EnvironmentReader.Read("TELEMETRY_AGENT_PORT", 6831);
                    o.MaxPayloadSizeInBytes = EnvironmentReader.Read("TELEMETRY_MAX_PAY_LOAD_SIZE_IN_BYTES", 4096);
                    o.ExportProcessorType = ExportProcessorType.Batch;
                    o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                    {
                        MaxQueueSize = EnvironmentReader.Read("TELEMETRY_BATCH_MAX_QUEUE_SIZE", 2048),
                        ScheduledDelayMilliseconds = EnvironmentReader.Read("TELEMETRY_BATCH_SCHEDULED_DELAY__MILLISECONDS", 5000),
                        ExporterTimeoutMilliseconds = EnvironmentReader.Read("TELEMETRY_BATCH_EXPORTER_TIMEOUT_MILLISECONDS", 30000),
                        MaxExportBatchSize = EnvironmentReader.Read("TELEMETRY_BATCH_MAX_EXPORT_BATCH_SIZE", 512)
                    };
                });
        });
        return this;
    }

    public IDatabaseable DoNotUseTelemetry()
    {
        return this;
    }

    public IBrokeable DoNotUseDatabase()
    {
        return this;
    }
}