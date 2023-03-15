using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mttechne.Toolkit.Rabbit;

public sealed class RabbitConsumer : BaseRabbit
{
    public RabbitConsumer(string stringConnection, string queueName, Func<string, Task<bool>> handler)
        : base(stringConnection, queueName)
    {
        _Handler = handler;
    }

    private readonly Func<string, Task<bool>> _Handler;

    protected override void DoExecute()
    {
        var consumer = new EventingBasicConsumer(Channel);
        consumer.Received += (sender, e) =>
        {
            if (DoReceiveMessage(e))
                Channel.BasicAck(e.DeliveryTag, false);
        };
        Channel.BasicConsume(QueueName, false, consumer);
    }

    private bool DoReceiveMessage(BasicDeliverEventArgs eventArgs)
    {
        var content = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(content);
        return _Handler(message).Result;
    }
}