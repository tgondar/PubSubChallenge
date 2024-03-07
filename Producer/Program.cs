using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/product", (ProductRequest payload) =>
{
    // TODO: save to database

    // Create a factory for connections to the RabbitMQ server
    var factory = new ConnectionFactory()
    {
        HostName = "host.docker.internal",
        Port = 5672,
        UserName = "user",
        Password = "password"
    };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "productQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        string message = JsonSerializer.Serialize(payload);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "productQueue",
                             basicProperties: null,
                             body: body);
    }

    return TypedResults.Created();
})
.WithName("NewProduct")
.WithOpenApi();

app.Run();

public record ProductRequest(Guid Id, string Name);