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

            _messageRepository.Read();

            string exchange = "logs";
            string routingKey = "info";

            _messageRepository.ReadFromTopic(exchange, routingKey);

            return Task.CompletedTask;
        }
    }
}
