public interface IMessageRepository
{
    void CreateConnection();
    void ReadFromQueue();
    void ReadFromTopic();
}
