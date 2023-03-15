using RabbitMQ.Client;

namespace Mttechne.Toolkit.Rabbit;

public abstract class BaseRabbit : DisposableObject
{
    public BaseRabbit(string stringConnection, string queueName)
    {
        _StringConnection = stringConnection;
        QueueName = queueName;
    }

    protected readonly string QueueName;
    private readonly string _StringConnection;
    protected IModel Channel;

    public void Start()
    {
        var chanel = CreateChannel();
        chanel.QueueDeclare(QueueName, true, false, false, null);
        DoExecute();
    }

    protected virtual void DoExecute()
    {
    }

    private IModel CreateChannel()
    {
        if (Channel != null)
            return Channel;
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_StringConnection)
        };
        var connection = factory.CreateConnection();
        return Channel = connection.CreateModel();
    }

    public void Stop()
    {
        if (Channel?.IsOpen == true)
            Channel?.Close();
        Channel?.Dispose();
        Channel = null;
    }

    protected override void DoDispose()
    {
        Stop();
    }
}