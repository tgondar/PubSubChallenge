using Common.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitReader : IMessageRepository
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
            Console.WriteLine("[x] Received: {0}, {1} - {2}", product?.Id, product?.Name, product?.CreateDate);
        };

        _channel.BasicConsume(queue: "productQueue", autoAck: true, consumer: consumer);
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
}
