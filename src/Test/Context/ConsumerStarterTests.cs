using Mttechne.Toolkit.OutBox;
using Microsoft.AspNetCore.Builder;
using Mttechne.Toolkit.TransactionalOutBox;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Test.Context;

public class ConsumerStarterTests : BaseStarterTests
{
    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseSerilog(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseSerilog);
        var strDbType = dbType.ToString();
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType,
            "DATABASE_CONNECTION_" + contextName, strDbType);
        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            builder.BeginConsumer<FakeContext>(
                    dbTypeVarName: "DATABASE_TYPE_" + contextName,
                    dbConnectionVarName: "DATABASE_CONNECTION_" + contextName)
                .UseSerilog();
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.NotNull(Provider);
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseTelemetryWithoutParameter_TelemetryHost(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseTelemetryWithoutParameter_TelemetryHost);
        var strDbType = dbType.ToString();
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType,
            "DATABASE_CONNECTION_" + contextName, strDbType);
        var variableName = "TELEMETRY_HOST_" + contextName;
        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                builder.BeginConsumer<FakeContext>()
                    .UseSerilog()
                    .UseTelemetry(telemetryHostVariableName: variableName);
            });
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.Equal($"Unable to identify Open Telemetry Host on {variableName} variable. Unable to start Transactional OutBox.", exception.Message);
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseDataBase_WithoutDatabaseType(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseDataBase_WithoutDatabaseType);
        var strDbType = dbType.ToString();
        var variableName = "DATABASE_TYPE_" + contextName;

        //act
        var builder = WebApplication.CreateBuilder();
        var exception = Assert.Throws<NullReferenceException>(() =>
        {
            builder.BeginConsumer<FakeContext>(dbTypeVarName: variableName)
                .UseSerilog()
                .DoNotUseTelemetry()
                .UseDatabase();
        });
        Provider = builder.Services.BuildServiceProvider();

        //assert
        Assert.NotNull(Provider);
        Assert.Equal($"Unable to identify DbType on {variableName} variable. Unable to start Transactional OutBox.", exception.Message);
    }

    [Fact]
    public void BeginConsumerUseDataBase_WithInvalidDatabase()
    {
        //arrange
        var contextName = nameof(BeginConsumerUseDataBase_WithInvalidDatabase);
        var variableName = "DATABASE_TYPE_" + contextName;
        var varNames = SetVars("DATABASE_TYPE_" + contextName, "MySql");

        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            var exception = Assert.Throws<NotImplementedException>(() =>
            {
                builder.BeginConsumer<FakeContext>(dbTypeVarName: variableName)
                    .UseSerilog()
                    .DoNotUseTelemetry()
                    .UseDatabase();
            });
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.NotNull(Provider);
            Assert.Equal($"Invalid DbType (MySql) informed on {variableName} variable. Unable to start Transactional OutBox.", exception.Message);
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseDataBase_WithoutDatabaseConnection(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseDataBase_WithoutDatabaseType);
        var strDbType = dbType.ToString();
        var variableName = "DATABASE_CONNECTION_" + contextName;
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType);

        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                builder.BeginConsumer<FakeContext>(
                        dbTypeVarName: "DATABASE_TYPE_" + contextName,
                        dbConnectionVarName: variableName)
                    .UseSerilog()
                    .DoNotUseTelemetry()
                    .UseDatabase();
            });
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.NotNull(Provider);
            Assert.Equal($"Unable to identify {dbType} Connection on {variableName} variable. Unable to start Transactional OutBox.", exception.Message);
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseRabbitM_WithoutRabbitMqServer(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseRabbitM_WithoutRabbitMqServer);
        var strDbType = dbType.ToString();
        var variableName = "RABBIT_MQ_" + contextName;
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType,
            "DATABASE_CONNECTION_" + contextName, strDbType,
            "RETRY_COUNT_" + contextName, "1",
            "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName, "1");
        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            var exception = Assert.Throws<NullReferenceException>(() =>
             {
                 builder.BeginConsumer<FakeContext>(
                         dbTypeVarName: "DATABASE_TYPE_" + contextName,
                         dbConnectionVarName: "DATABASE_CONNECTION_" + contextName,
                         retryCountVarName: "RETRY_COUNT_" + contextName,
                         retryIntevalInMillisecondsVarName: "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName)
                     .UseSerilog()
                     .DoNotUseTelemetry()
                     .UseDatabase()
                     .UseRabbitMq(rabbitMqVariableName: variableName);
             });
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.NotNull(Provider);
            Assert.Equal($"Unable to identify RabbitMq Host on {variableName} variable. Unable to start Transactional OutBox.", exception.Message);
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseUseAllWithRabbitMqSucess(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseUseAllWithRabbitMqSucess);
        var strDbType = dbType.ToString();
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType,
            "DATABASE_CONNECTION_" + contextName, strDbType,
            "RETRY_COUNT_" + contextName, "1",
            "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName, "1",
            "TELEMETRY_HOST_" + contextName, "localhost",
            "JAEGER_AGENT_HOST_" + contextName, "localhost",
            "JAEGER_AGENT_PORT_" + contextName, "6831",
            "JAEGER_SAMPLER_TYPE_" + contextName, "remote",
            "JAEGER_SAMPLING_ENDPOINT_" + contextName, "http://localhost:5778/sampling",
            "RABBIT_MQ_" + contextName, "localhost");
        try
        {
            //act
            var builder = WebApplication.CreateBuilder();

            if (dbType != DatabaseType.InMemory)
            {
                builder.BeginConsumer<FakeContext>(
                        dbTypeVarName: "DATABASE_TYPE_" + contextName,
                        dbConnectionVarName: "DATABASE_CONNECTION_" + contextName,
                        retryCountVarName: "RETRY_COUNT_" + contextName,
                        retryIntevalInMillisecondsVarName: "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName)
                    .UseSerilog()
                    .UseTelemetry(telemetryHostVariableName: "TELEMETRY_HOST_" + contextName)
                    .UseDatabase()
                    .UseRabbitMq(rabbitMqVariableName: "RABBIT_MQ_" + contextName);
                Provider = builder.Services.BuildServiceProvider();

                //assert
                Assert.NotNull(Provider);
            }
            else
            {
                var exception = Assert.Throws<NotImplementedException>(() =>
                {
                    builder.BeginConsumer<FakeContext>(
                            dbTypeVarName: "DATABASE_TYPE_" + contextName,
                            dbConnectionVarName: "DATABASE_CONNECTION_" + contextName,
                            retryCountVarName: "RETRY_COUNT_" + contextName,
                            retryIntevalInMillisecondsVarName: "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName)
                        .UseSerilog()
                        .UseTelemetry(telemetryHostVariableName: "TELEMETRY_HOST_" + contextName)
                        .UseDatabase()
                        .UseRabbitMq(rabbitMqVariableName: "RABBIT_MQ_" + contextName);
                });
                Provider = builder.Services.BuildServiceProvider();

                //assert
                Assert.Equal($"DbType {dbType} not supported yet on UseRabbitMq.", exception.Message);
            }
        }
        finally
        {
            ClearVars(varNames);
        }
    }

    [Theory]
    [InlineData(DatabaseType.InMemory)]
    [InlineData(DatabaseType.SqlServer)]
    [InlineData(DatabaseType.Postgres)]
    public void BeginConsumerUseUseAllWithHarnessSucess(DatabaseType dbType)
    {
        //arrange
        var contextName = nameof(BeginConsumerUseUseAllWithHarnessSucess);
        var strDbType = dbType.ToString();
        var varNames = SetVars("DATABASE_TYPE_" + contextName, strDbType,
            "DATABASE_CONNECTION_" + contextName, strDbType,
            "RETRY_COUNT_" + contextName, "1",
            "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName, "1",
            "TELEMETRY_HOST_" + contextName, "localhost",
            "JAEGER_AGENT_HOST_" + contextName, "localhost",
            "JAEGER_AGENT_PORT_" + contextName, "6831",
            "JAEGER_SAMPLER_TYPE_" + contextName, "remote",
            "JAEGER_SAMPLING_ENDPOINT_" + contextName, "http://localhost:5778/sampling");
        try
        {
            //act
            var builder = WebApplication.CreateBuilder();
            builder.BeginConsumer<FakeContext>(
                    dbTypeVarName: "DATABASE_TYPE_" + contextName,
                    dbConnectionVarName: "DATABASE_CONNECTION_" + contextName,
                    retryCountVarName: "RETRY_COUNT_" + contextName,
                    retryIntevalInMillisecondsVarName: "RETRY_INTERVAL_IN_MILLISECONDS_" + contextName)
                .UseSerilog()
                .UseTelemetry(telemetryHostVariableName: "TELEMETRY_HOST_" + contextName)
                .UseDatabase()
                .UseHarness();
            Provider = builder.Services.BuildServiceProvider();

            //assert
            Assert.NotNull(Provider);
        }
        finally
        {
            ClearVars(varNames);
        }
    }
}