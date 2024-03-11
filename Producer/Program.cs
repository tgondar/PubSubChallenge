using Producer.Entities;
using Producer.MessagePublish;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMessagePublisher, RabbitPublisher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/queue/product", (ProductRequestDto payload, IMessagePublisher messageRepository) =>
{
    ProductRequest product = DataManipulation(payload);

    messageRepository.PublishToQueue(JsonSerializer.Serialize(product));

    return TypedResults.Created();
})
.WithName("NewProduct")
.WithOpenApi();

app.MapPost("/topic/product", (ProductRequestDto payload, IMessagePublisher messageRepository) =>
{
    ProductRequest product = DataManipulation(payload);

    messageRepository.PublishToTopic(JsonSerializer.Serialize(product));

    return TypedResults.Created();
})
.WithName("NewProductToTopic")
.WithOpenApi();


app.Run();

static ProductRequest DataManipulation(ProductRequestDto payload)
{
    return new()
    {
        Id = payload.Id,
        Name = payload.Name,
        CreateDate = DateTime.UtcNow
    };
}
