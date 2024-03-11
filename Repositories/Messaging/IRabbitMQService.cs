
public interface IRabbitMQService
{
    void ReadFromQueue();
    void ReadFromTopic(string exchange, string routingKey);
}

