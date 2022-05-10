using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Consumer
{
	EventingBasicConsumer consumer;
	IModel channel;
	string instanceId;

	public Consumer(IModel channel, string queue, string instanceId)
	{
		this.channel = channel;
		this.instanceId = instanceId;
		consumer = new EventingBasicConsumer(channel);
		consumer.Received += OnReceived;

		channel.BasicConsume(
			queue: queue,
			autoAck: true,
			consumer: consumer
		);
	}

	private void OnReceived(object? sender, BasicDeliverEventArgs ea)
	{
		var body = ea.Body.ToArray();
		var message = Encoding.UTF8.GetString(body);
		Console.WriteLine(" [x] {0}", message);

		var props = ea.BasicProperties;
		if (props.CorrelationId != null && props.ReplyTo != null)
		{
			var replyProps = channel.CreateBasicProperties();
			replyProps.CorrelationId = props.CorrelationId;

			var replyMessage = $"MQ Reply from: {instanceId} - message: {message}";

			channel.BasicPublish(
				exchange: "",
				routingKey: props.ReplyTo,
				basicProperties: replyProps,
				body: Encoding.UTF8.GetBytes(replyMessage));
		}
	}
}