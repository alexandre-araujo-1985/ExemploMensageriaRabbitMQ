using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

public class RabbitListener
{
    public RabbitListener()
    {
		var factory = new ConnectionFactory() { HostName = "localhost" };
		using var connection = factory.CreateConnection();
		using var channel = connection.CreateModel();
		
		channel.QueueDeclare(queue: "ClienteQueue",
							 durable: false,
							 exclusive: false,
							 autoDelete: false,
							 arguments: null);

		var consumer = new EventingBasicConsumer(channel);
		consumer.Received += (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			Console.WriteLine($"Mensagem recebida: {message}");
		};

		channel.BasicConsume(queue: "ClienteQueue",
							 autoAck: true,
							 consumer: consumer);
	}
}