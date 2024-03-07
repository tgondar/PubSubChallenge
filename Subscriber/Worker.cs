using Microsoft.Extensions.Hosting;

namespace Subscriber
{
    public class Worker : BackgroundService
    {
        readonly RabbitReader _rabbitReader = new();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _rabbitReader.Read();

            return Task.CompletedTask;
        }
    }
}
