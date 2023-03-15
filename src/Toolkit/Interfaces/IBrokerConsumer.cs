using MassTransit;

namespace Mttechne.Toolkit.Interfaces;

public interface IBrokerConsumer : IConsumer
{
}

public interface IBrokerConsumer<in T> : IConsumer<T>, IConsumer<Fault<T>>, IBrokerConsumer where T : class
{
}