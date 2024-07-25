using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ExemploMensageriaRabbitMQ
{
	public class Publisher
	{
		public Publisher()
		{
			var factory = new ConnectionFactory { HostName = "localhost" };

			var connection = factory.CreateConnection();

			using var channel = connection.CreateModel();

			channel.QueueDeclare(
				queue: "ClienteQueue",
				durable: false,
				exclusive: false,
				autoDelete: false);

			var consumer = new EventingBasicConsumer(channel);

			consumer.Received += (model, eventArgs) =>
			{
				var body = eventArgs.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				Console.WriteLine($"Mensagem recebida do cliente: {message}");
			};

			channel.BasicConsume(
				queue: "ClienteQueue",
				autoAck: true, 
				consumer: consumer);
		}
	}
}
