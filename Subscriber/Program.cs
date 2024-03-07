﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Subscriber;

Console.WriteLine("Rabbit reader: ");

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IMessageRepository, RabbitReader>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

Console.ReadLine();
