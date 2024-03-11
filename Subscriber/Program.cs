using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Subscriber;

Console.WriteLine("Rabbit reader: ");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMessageRepository, RabbitReader>();
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));

var host = builder.Build();
host.Run();

Console.ReadLine();
