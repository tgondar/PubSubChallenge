using Common.Entities;

public interface IMessageRepository
{
    void CreateConnection();
    void Read();
    void ReadFromTopic(string exchange, string routingKey);
    void Publish(string message);
    void PublishToTopic(string exchange, string routingKey, string message);
}
