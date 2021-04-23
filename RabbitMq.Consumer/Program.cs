using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace RabbitMq.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("RabbitMqTestQueue", true, false, false);

            //for fanout exchange type
            channel.QueueBind("RabbitMqTestQueue", "TestAppExchange", "");

            //for direct exchange type
            //channel.QueueBind("RabbitMqTestQueue", "TestAppExchange", "direct.routingKey");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var msg = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Console.WriteLine(msg);
            };

            channel.BasicConsume("RabbitMqTestQueue", true, consumer);

            Console.ReadLine();

            channel.Close();
            connection.Close();
        }
    }
}
