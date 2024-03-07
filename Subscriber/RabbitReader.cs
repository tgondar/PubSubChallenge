using Common.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Subscriber
{
    public class RabbitReader
    {
        private IConnection _connection;
        private IModel _channel;

        public RabbitReader()
        {
            CreateConnection();
        }

        public void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "password",

            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Read()
        {
            _channel.QueueDeclare(queue: "productQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ProductRequest product = JsonSerializer.Deserialize<ProductRequest>(message);
                Console.WriteLine(" [x] Received: {0}, {1}", product.Id, product.Name);
            };

            _channel.BasicConsume(queue: "productQueue", autoAck: true, consumer: consumer);
        }
    }
}