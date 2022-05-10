using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;

class Program
{
	static async Task Main(string[] args)
	{
		var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
		using (var connection = factory.CreateConnection())
		using (var channel = connection.CreateModel())
		{
			channel.QueueDeclare(
				queue: "send",
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			Console.WriteLine(" [*] Waiting for logs.");

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine(" [x] {0}", message);
			};
			channel.BasicConsume(queue: "send",
								 autoAck: true,
								 consumer: consumer);


			Console.WriteLine("Waiting for messages...");
			await new HostBuilder().Build().RunAsync();
		}
	}
}