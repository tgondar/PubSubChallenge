using Common.Dto;
using Common.Entities;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMessageRepository, RabbitReader>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/queue/product", (ProductRequestDto payload) =>
{
    //Some manipulation to the data ?!
    ProductRequest product = DataManipulation(payload);

    // TODO: save to database

    var messageRepository = app.Services.GetRequiredService<IMessageRepository>();
    messageRepository.Publish(JsonSerializer.Serialize(product));

    return TypedResults.Created();
})
.WithName("NewProduct")
.WithOpenApi();

app.MapPost("/topic/product", (ProductRequestDto payload) =>
{
    //Some manipulation to the data ?!
    ProductRequest product = DataManipulation(payload);

    // TODO: save to database

    var messageRepository = app.Services.GetRequiredService<IMessageRepository>();
    // Assuming RabbitReader has been updated with the PublishToTopic method
    // Also assuming you have a predefined exchange and routing key
    string exchange = "logs";
    string routingKey = "info";
    messageRepository.PublishToTopic(exchange, routingKey, JsonSerializer.Serialize(product));

    return TypedResults.Created();
})
.WithName("NewProductToTopic")
.WithOpenApi();


app.Run();

static ProductRequest DataManipulation(ProductRequestDto payload)
{
    ProductRequest product = new()
    {
        Id = payload.Id,
        Name = payload.Name
    };
 
    Random random = new();

    product.CreateDate = random.Next(1, 100) switch
    {
        < 25 => DateTime.UtcNow.AddMinutes(random.Next(1, 10)),
        < 50 => DateTime.UtcNow.AddHours(random.Next(1, 10)),
        < 75 => DateTime.UtcNow.AddDays(random.Next(1, 10)),
        _ => DateTime.UtcNow.AddMonths(random.Next(1, 10)),
    };

    return product;
}
