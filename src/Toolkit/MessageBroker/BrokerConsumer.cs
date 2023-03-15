using MassTransit;
using System.Diagnostics;
using Mttechne.Toolkit.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mttechne.Toolkit.MessageBroker;

public abstract class BrokerConsumer : IBrokerConsumer
{
    private static int _RetryCount = 5;
    private static int _RetryIntevalInMilliseconds = 100;
    private static readonly object _Lock = new();
    private static readonly Dictionary<Guid, List<string>> _Sagas = new();

    public BrokerConsumer()
    {
    }

    protected virtual int RetryCount
        => _RetryCount;

    protected virtual int RetryIntevalInMilliseconds
        => _RetryIntevalInMilliseconds;

    public static void AddSagaState(Guid sagaID, string state)
    {
        lock (_Lock)
        {
            if (!_Sagas.TryGetValue(sagaID, out List<string> lst))
            {
                lst = new List<string>();
                _Sagas.Add(sagaID, lst);
            }
            lst.Add(state);
        }
    }

    public static bool ExistSagaState(Guid sagaID, string state, int timeOutInMilliseconds = 5000)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.ElapsedMilliseconds < timeOutInMilliseconds)
        {
            lock (_Lock)
                if (_Sagas.TryGetValue(sagaID, out List<string> lst) && lst.Contains(state))
                    return true;
            Thread.Sleep(100);
        }
        return false;
    }

    public static void SetRetryParameters(int retryCount, int retryIntevalInMilliseconds)
    {
        _RetryCount = retryCount;
        _RetryIntevalInMilliseconds = retryIntevalInMilliseconds;
    }

    protected internal virtual void Configure<T>(IConsumerConfigurator<T> configuration) where T : BrokerConsumer, new()
    {
        configuration.UseMessageRetry(o => o.Intervals(1000, 2000, 4000, 6000, 8000));
    }
}

public abstract class BrokerConsumer<T> : BrokerConsumer, IBrokerConsumer<T> where T : class
{
    public BrokerConsumer()
        : base()
    {
    }

    protected abstract Task ConsumeAsync(T message);
    protected abstract ILogger Logger { get; }

    public async Task Consume(ConsumeContext<T> context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (context.Message == null)
            throw new ArgumentNullException(nameof(context.Message));
        await ConsumeAsync(context.Message);
    }

    public async Task Consume(ConsumeContext<Fault<T>> context)
    {
        await ConsumerFault(context);
    }

    private async Task ConsumerFault(ConsumeContext<Fault<T>> context)
    {
        if (context == null || context.Message == null)
            return;
        await ConsumerFaultAsync(context.Message);
    }

    protected virtual async Task ConsumerFaultAsync(Fault<T> message)
    {
        await Task.CompletedTask;
    }

    protected async Task<TResult> TryExecute<TResult>(Func<Task<TResult>> action, string failMessage = null)
        where TResult : class
    {
        return await Resilience.TryExecute(action, Logger, failMessage, RetryCount, RetryIntevalInMilliseconds);
    }
}