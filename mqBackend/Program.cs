using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;

class Program
{
	static async Task Main(string[] args)
	{
		var instanceId = Guid.NewGuid().ToString();

		Console.WriteLine($"Running with instance ID: {instanceId}");

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

			var sendConsumer = new Consumer(channel, "send", instanceId);

			Console.WriteLine("Waiting for messages...");
			await new HostBuilder().Build().RunAsync();
		}
	}
}