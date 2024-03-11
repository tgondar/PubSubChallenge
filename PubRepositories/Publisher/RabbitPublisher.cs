using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace PubRepositories.Publisher
{
    public class RabbitPublisher : IMessagePublisher
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQSettings _rabbitSettings;

        public RabbitPublisher(IOptions<RabbitMQSettings> rabbitSettings)
        {
            _rabbitSettings = rabbitSettings.Value;

            CreateConnection();
        }

        public void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName =_rabbitSettings.HostName,
                Port = _rabbitSettings.Port,
                UserName = _rabbitSettings.UserName,
                Password = _rabbitSettings.Password,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublishToQueue(string message)
        {
            _channel.QueueDeclare(queue: "productQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "productQueue",
                                 basicProperties: null,
                                 body: body);
        }

        public void PublishToTopic(string message)
        {
            string exchange = _rabbitSettings.Exchange;
            string routingKey = _rabbitSettings.RoutingKey;

            // Declare the exchange
            _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
