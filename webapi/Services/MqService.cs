using System.Text;
using RabbitMQ.Client;

public class MqService
{
	private IConnection connection;
	private IModel channel;

	private const string SEND_QUEUE = "send";
	private const string RECEIVE_QUEUE = "receive";

	public MqService()
	{
		var factory = new ConnectionFactory
		{
			HostName = "rabbitmq",
			Port = 5672
		};

		connection = factory.CreateConnection();
		channel = connection.CreateModel();

		channel.QueueDeclare(
			queue: SEND_QUEUE,
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null
		);
	}

	public Task<string> SendAsync(string message)
	{
		var body = Encoding.UTF8.GetBytes(message);
		var props = channel.CreateBasicProperties();
		props.Expiration = "30000";
		props.ReplyTo = RECEIVE_QUEUE;
		props.CorrelationId = Guid.NewGuid().ToString();

		channel.BasicPublish(
			exchange: "",
			routingKey: SEND_QUEUE,
			basicProperties: props,
			body: body
		);

		Console.WriteLine(" [x] Sent {0}", message);

		return Task.FromResult<string>("result");
	}

}