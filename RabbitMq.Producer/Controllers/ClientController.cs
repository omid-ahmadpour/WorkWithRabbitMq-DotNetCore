using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;

namespace RabbitMq.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet("Fanout")]
        public void Fanout()
        {
            var message = "omid; 28, ahmadpooromid@gmail.com, Fanout Exchange";

            SendMessage("", message);
        }

        [HttpGet("Direct")]
        public void Direct()
        {
            var message = "This is a message for Direct Exchange.";

            SendMessage("direct.routingKey", message);
        }

        private void SendMessage(string routingKey, string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("TestAppExchange", ExchangeType.Direct, true);

            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("TestAppExchange", routingKey, null, bytes);

            channel.Close();
            connection.Close();
        }
    }
}
