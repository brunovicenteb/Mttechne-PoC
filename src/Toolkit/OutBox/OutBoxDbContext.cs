using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.EntityFrameworkCoreIntegration;
using Mttechne.Toolkit.MessageBroker;
using Mttechne.Toolkit.OutBox.Producer;

namespace Mttechne.Toolkit.TransactionalOutBox;

public abstract class OutBoxDbContext : SagaDbContext
{
    private const string _DefaultDataBaseVarName = "DATABASE_TYPE";
    private const string _DefaultStringConnVarName = "DATABASE_CONNECTION";

    public OutBoxDbContext(DbContextOptions options)
        : base(options)
    {
    }

    private DatabaseType? _DbType;

    protected internal abstract SagaAdaptorService CreateSagaAdapter(IServiceProvider serviceProvider, string host);

    protected DatabaseType DbType
    {
        get
        {
            _DbType ??= GetDatabaseTypeByName();
            return _DbType.Value;
        }
    }

    private DatabaseType? GetDatabaseTypeByName()
    {
        string dbName = Database.ProviderName;
        string providerName = dbName ?? "undefined";
        if (dbName.Contains("InMemory"))
            return DatabaseType.InMemory;
        else if (providerName.Contains("SqlServer"))
            return DatabaseType.SqlServer;
        else if (providerName.Contains("Postgre") || providerName.Contains("Npgsql"))
            return DatabaseType.Postgres;
        throw new InvalidOperationException($"The provider {providerName} is not yet supported on OutBoxDbContext");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var db = EnvironmentReader.Read<DatabaseType>(_DefaultDataBaseVarName);
            var stringConn = EnvironmentReader.Read<string>(_DefaultStringConnVarName);
            if (db == DatabaseType.Postgres)
                options.UseNpgsql(stringConn);
            else if (db == DatabaseType.SqlServer)
                options.UseSqlServer(stringConn);
        }
        base.OnConfiguring(options);
    }

    protected void AddConsumer<T>(IBusRegistrationConfigurator busRegistration) where T : BrokerConsumer//, new()
    {
        //var consumer = new T();
        busRegistration.AddConsumer<T>();
    }

    public virtual void RegisterConsumers(DatabaseType dbType, IServiceCollection services, IBusRegistrationConfigurator busRegistration)
    {
    }

    protected virtual void DoModelCreating(ModelBuilder modelBuilder)
    {
    }

    protected override sealed void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DoModelCreating(modelBuilder);
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}