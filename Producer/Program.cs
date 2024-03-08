using Common.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMessageRepository, RabbitReader>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/product", (ProductRequest payload) =>
{
    // TODO: save to database

    var messageRepository = app.Services.GetRequiredService<IMessageRepository>();
    messageRepository.Publish(JsonSerializer.Serialize(payload));

    return TypedResults.Created();
})
.WithName("NewProduct")
.WithOpenApi();

app.Run();

