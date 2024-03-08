using Common.Entities;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitReader : IMessageRepository, IRabbitMQService
{
    private readonly ILogger<RabbitReader> _logger;
    private IConnection _connection;
    private IModel _channel;

    public RabbitReader(ILogger<RabbitReader> logger)
    {
        CreateConnection();
        _logger = logger;
    }

    public void CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "host.docker.internal",
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

            string workerid = Environment.GetEnvironmentVariable("workerid");

            _logger.LogInformation($"WORKER {workerid} Received: {product?.Id}, {product?.Name} - {product?.CreateDate}");

        };

        _channel.BasicConsume(queue: "productQueue", autoAck: true, consumer: consumer);
    }

    public void ReadFromTopic(string exchange, string routingKey)
    {
        // Declare the exchange
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

        // Declare a queue
        var queueName = _channel.QueueDeclare().QueueName;

        // Bind the queue to the exchange with the routing key
        _channel.QueueBind(queue: queueName,
                           exchange: exchange,
                           routingKey: routingKey);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            ProductRequest product = JsonSerializer.Deserialize<ProductRequest>(message);

            string workerid = Environment.GetEnvironmentVariable("workerid");

            _logger.LogInformation($"WORKER {workerid} Received: {product?.Id}, {product?.Name} - {product?.CreateDate}");
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }


    public void Publish(string message)
    {
        _channel.QueueDeclare(queue: "productQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: "productQueue",
                             basicProperties: null,
                             body: body);
    }

    public void PublishToTopic(string exchange, string routingKey, string message)
    {
        // Declare the exchange
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: exchange,
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);
    }
}
