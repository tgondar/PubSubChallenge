namespace PubRepositories.Publisher
{
    public interface IMessagePublisher
    {
        void PublishToQueue(string message);
        void PublishToTopic(string message);
    }
}
