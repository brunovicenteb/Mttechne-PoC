using Mttechne.Toolkit.OutBox.Consumer;
using Mttechne.Toolkit.OutBox.Producer;
using Mttechne.Toolkit.TransactionalOutBox;
using Microsoft.AspNetCore.Builder;

namespace Mttechne.Toolkit.OutBox;

public static class OutBoxFactory
{
    public static ILogable BeginProducer<T>(this WebApplicationBuilder builder,
        bool recreateDb = false, string dbTypeVarName = "DATABASE_TYPE", string dbConnectionVarName = "DATABASE_CONNECTION",
        string retryCountVarName = "RETRY_COUNT", string retryIntevalInMillisecondsVarName = "RETRY_INTERVAL_IN_MILLISECONDS")
        where T : OutBoxDbContext
    {
        return new ProducerOutBoxStarter<T>(builder, dbTypeVarName, recreateDb, dbConnectionVarName, retryCountVarName, retryIntevalInMillisecondsVarName);
    }

    public static ILogable BeginConsumer<T>(this WebApplicationBuilder builder, string dbTypeVarName = "DATABASE_TYPE",
        string dbConnectionVarName = "DATABASE_CONNECTION", string retryCountVarName = "RETRY_COUNT", 
        string retryIntevalInMillisecondsVarName = "RETRY_INTERVAL_IN_MILLISECONDS")
        where T : OutBoxDbContext, new()
    {
        return new ConsumerOutBoxStarter<T>(builder, dbTypeVarName, dbConnectionVarName, retryCountVarName, retryIntevalInMillisecondsVarName);
    }
}