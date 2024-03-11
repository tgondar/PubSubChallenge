namespace Producer.MessagePublish
{
    public interface IMessagePublisher
    {
        void PublishToQueue(string message);
        void PublishToTopic(string message);
    }
}
