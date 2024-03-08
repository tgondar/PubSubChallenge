using Common.Entities;

public interface IMessageRepository
{
    void CreateConnection();
    void Read();
    void Publish(string message);
}
