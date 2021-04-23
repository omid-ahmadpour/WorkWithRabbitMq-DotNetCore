using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;

namespace RabbitMq.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet("fanout")]
        public void Fanout()
        {
            var message = "Fanout Exchange: omid; 28, ahmadpooromid@gmail.com";

            SendMessage(ExchangeType.Fanout,"", message);
        }

        [HttpGet("direct")]
        public void Direct()
        {
            var message = "Direct Exchange: omid; 28, ahmadpooromid@gmail.com";

            SendMessage(ExchangeType.Direct,"direct.routingKey", message);
        }

        private void SendMessage(string exchangeType,string routingKey, string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("TestAppExchange", exchangeType, true);

            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("TestAppExchange", routingKey, null, bytes);

            channel.Close();
            connection.Close();
        }
    }
}
