namespace Producer.MessagePublish
{
    public interface IRabbitMQService
    {
        void PublishToQueue(string message);
        void PublishToTopic(string message);
    }
}
