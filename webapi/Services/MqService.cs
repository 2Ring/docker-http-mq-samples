using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MqService
{
	private IConnection connection;
	private IModel channel;

	private const string SEND_QUEUE = "send";
	private readonly string receiveQueue;

	private Dictionary<string, TaskCompletionSource<string>> queuedMessages = new Dictionary<string, TaskCompletionSource<string>>();
	private EventingBasicConsumer consumer;

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

		receiveQueue = channel.QueueDeclare().QueueName;

		consumer = new EventingBasicConsumer(channel);
		consumer.Received += OnReceived;
		channel.BasicConsume(
			queue: receiveQueue,
			autoAck: true,
			consumer: consumer
		);
	}

	public void Send(string message)
	{
		var body = Encoding.UTF8.GetBytes(message);
		var props = channel.CreateBasicProperties();
		props.Expiration = "30000";


		channel.BasicPublish(
			exchange: "",
			routingKey: SEND_QUEUE,
			basicProperties: props,
			body: body
		);
	}

	public Task<string> SendAndGetResponse(string message)
	{
		var body = Encoding.UTF8.GetBytes(message);
		var props = channel.CreateBasicProperties();
		props.Expiration = "30000";
		props.ReplyTo = receiveQueue;
		var correlationId = Guid.NewGuid().ToString();
		props.CorrelationId = correlationId;

		channel.BasicPublish(
			exchange: "",
			routingKey: SEND_QUEUE,
			basicProperties: props,
			body: body
		);

		var queuedMessage = new TaskCompletionSource<string>();

		queuedMessages.Add(correlationId, queuedMessage);

		return queuedMessage.Task;
	}

	private void OnReceived(object? sender, BasicDeliverEventArgs ea)
	{
		var body = ea.Body.ToArray();
		var message = Encoding.UTF8.GetString(body);

		var correlationId = ea.BasicProperties.CorrelationId;

		if (queuedMessages.TryGetValue(correlationId, out var queuedMessage))
		{
			queuedMessages.Remove(correlationId);
			queuedMessage.TrySetResult(message);
		}
		else
		{
			Console.WriteLine("Unknown correlation ID received");
		}
	}

}