using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.OutBox;

public interface IBrokeable : IOutboxBuilder
{
    public IOutboxBuilder UseRabbitMq(string rabbitMqVariableName = "RABBIT_MQ");
    public IOutboxBuilder UseHarness();
}