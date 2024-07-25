using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ExemploMensageriaRabbitMQ.Subscribe.Models;

namespace ExemploMensageriaRabbitMQ.Subscribe.Listeners
{
	public class ClienteListener : BackgroundService
	{
		private readonly IConnection _connection;
		private readonly IModel _model;
		private readonly IServiceProvider _serviceProvider;
		private const string Queue = "cliente-queue";
		private const string RoutingKeySubscribe = "cliente-queue";
		private const string TrackingsExchange = "trackings-service";


		public ClienteListener(IServiceProvider serviceProvider)
		{
			var connectionFactory = new ConnectionFactory
			{
				HostName = "localhost",
			};

			_connection = connectionFactory.CreateConnection("cliente-mensagem-consumer");

			_model = _connection.CreateModel();

			_model.QueueDeclare(
				queue: Queue, 
				durable: false, 
				exclusive: false, 
				autoDelete: false, 
				arguments: null);

			//_model.QueueBind(Queue, TrackingsExchange, RoutingKeySubscribe);
			_serviceProvider = serviceProvider;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_model);

			consumer.Received += (sender, eventArgs) =>
			{
				var contentArray = eventArgs.Body.ToArray();
				var contentString = Encoding.UTF8.GetString(contentArray);
				var @event = JsonConvert.DeserializeObject<Cliente>(contentString);

                Console.WriteLine($"Mensagem recebida: {@event?.Id}, {@event?.Nome}");

                _model.BasicAck(eventArgs.DeliveryTag, false);
			};

			_model.BasicConsume(Queue, false, consumer);

			return Task.CompletedTask;
		}
	}
}
