
public interface IRabbitMQService
{
    void Publish(string message);
    void PublishToTopic(string exchange, string routingKey, string message);
    void Read();
    void ReadFromTopic(string exchange, string routingKey);
}

