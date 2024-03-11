using Common.Entities;

public interface IMessageRepository
{
    void CreateConnection();
    void ReadFromQueue();
    void ReadFromTopic(string exchange, string routingKey);
}
