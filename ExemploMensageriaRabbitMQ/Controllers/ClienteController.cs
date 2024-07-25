using ExemploMensageriaRabbitMQ.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace ExemploMensageriaRabbitMQ.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class ClienteController : ControllerBase
	{
		[HttpPost]
		public IActionResult ObterCliente([FromBody] Cliente cliente)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			var connection = factory.CreateConnection();
			var channel = connection.CreateModel();
			channel.QueueDeclare(queue: "cliente-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
			var json = System.Text.Json.JsonSerializer.Serialize(cliente);
			var body = System.Text.Encoding.UTF8.GetBytes(json);
			channel.BasicPublish(exchange: "", routingKey: "cliente-queue", basicProperties: null, body: body);

			return Ok();
		}
	}
}
