using MassTransit;
using MassTransit.RetryPolicies;
using Microsoft.Extensions.Logging;

namespace Mttechne.Toolkit;

public static class Resilience
{
    public static async Task<TResult> TryExecute<TResult>(Func<Task<TResult>> action, ILogger logger = null, string failMessage = null, int retryCount = 5, double intevalInMilliseconds = 100)
        where TResult : class
    {
        try
        {
            var policy = Retry.Interval(retryCount, TimeSpan.FromMilliseconds(intevalInMilliseconds));
            var result = await policy.Retry(async () =>
            {
                return await action();
            });
            return result;
        }
        catch (Exception ex)
        {
            if (logger != null && failMessage.IsFilled())
                logger.LogError(ex, failMessage);
        }
        return default;
    }
}