using System.Text;
using RabbitMQ.Client;

namespace Mttechne.Toolkit.Rabbit;

public sealed class RabbitProducer : BaseRabbit
{
    public RabbitProducer(string stringConnection, string queueName)
        : base(stringConnection, queueName)
    {
    }

    public bool Publish(string message)
    {
        if (Channel == null)
            return false;
        var content = Encoding.UTF8.GetBytes(message);
        Channel.BasicPublish(string.Empty, QueueName, null, content);
        return true;
    }
}