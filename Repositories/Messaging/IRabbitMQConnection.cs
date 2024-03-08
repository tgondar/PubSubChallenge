using RabbitMQ.Client;

public interface IRabbitMQConnection
{
    IModel CreateModel();
    // Add other necessary methods
}

public interface IRabbitMQModel
{
    void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);
    // Add other necessary methods
}
