using Common.Entities;
using Moq;
using System.Text.Json;

namespace TestProject
{
    public class RabbitMQTopicTest
    {
        [Fact]
        public void TestPublishToTopic()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();
            var message = "Test message";
            var exchange = "TestExchange";
            var routingKey = "TestRoutingKey";

            // Act
            mockRabbitMQService.Object.PublishToTopic(exchange, routingKey, message);

            // Assert
            mockRabbitMQService.Verify(service => service.PublishToTopic(exchange, routingKey, message), Times.Once);
        }

        [Fact]
        public void TestReadFromTopic()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();
            var exchange = "TestExchange";
            var routingKey = "TestRoutingKey";

            // Act
            mockRabbitMQService.Object.ReadFromTopic(exchange, routingKey);

            // Assert
            mockRabbitMQService.Verify(service => service.ReadFromTopic(exchange, routingKey), Times.Once);
        }

        [Fact]
        public void TestPublishToTopic_ErrorHandling()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();
            var message = "Test message";
            var exchange = "TestExchange";
            var routingKey = "TestRoutingKey";

            // Set up the mock to throw an exception when PublishToTopic is called
            mockRabbitMQService.Setup(service => service.PublishToTopic(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Test exception"));

            // Act and Assert
            Exception ex = Assert.Throws<Exception>(() => mockRabbitMQService.Object.PublishToTopic(exchange, routingKey, message));
            Assert.Equal("Test exception", ex.Message);
        }

        [Fact]
        public void TestReadFromTopic_EnvironmentVariable()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();
            var exchange = "TestExchange";
            var routingKey = "TestRoutingKey";

            // Set the environment variable to a known value
            Environment.SetEnvironmentVariable("workerid", "TestWorker");

            // Set up the mock to capture the workerid when ReadFromTopic is called
            string capturedWorkerId = null;
            mockRabbitMQService.Setup(service => service.ReadFromTopic(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => capturedWorkerId = Environment.GetEnvironmentVariable("workerid"));

            // Act
            mockRabbitMQService.Object.ReadFromTopic(exchange, routingKey);

            // Assert
            Assert.Equal("TestWorker", capturedWorkerId);
        }

        [Fact]
        public void TestPublishToTopic_MessageSerialization()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();
            var message = new ProductRequest { Id = Guid.NewGuid(), Name = "Test Product" };
            var exchange = "TestExchange";
            var routingKey = "TestRoutingKey";

            // Set up the mock to capture the serialized message when PublishToTopic is called
            string capturedMessage = null;
            mockRabbitMQService.Setup(service => service.PublishToTopic(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((ex, rk, msg) => capturedMessage = msg);

            // Act
            mockRabbitMQService.Object.PublishToTopic(exchange, routingKey, JsonSerializer.Serialize(message));

            // Assert
            var deserializedMessage = JsonSerializer.Deserialize<ProductRequest>(capturedMessage);
            Assert.Equal(message.Id, deserializedMessage.Id);
            Assert.Equal(message.Name, deserializedMessage.Name);
        }
    }
}
