using Microsoft.Extensions.Hosting;

namespace Subscriber
{
    public class Worker : BackgroundService
    {
        private readonly IMessageRepository _messageRepository;

        public Worker(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _messageRepository.ReadFromQueue();

            _messageRepository.ReadFromTopic();

            return Task.CompletedTask;
        }
    }
}
