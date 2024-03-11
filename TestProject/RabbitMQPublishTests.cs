using Moq;
using System.Text.Json;

namespace TestProject
{
    public class RabbitMQPublishTests
    {
        [Fact]
        public void TestPublishToTopic()
        {
            // Arrange
            var mockRabbitMQService = new Mock<Producer.MessagePublish.IRabbitMQService>();
            var message = "Test message";

            // Act
            mockRabbitMQService.Object.PublishToTopic(message);

            // Assert
            mockRabbitMQService.Verify(service => service.PublishToTopic(message), Times.Once);
        }

        [Fact]
        public void TestPublishToTopic_ErrorHandling()
        {
            // Arrange
            var mockRabbitMQService = new Mock<Producer.MessagePublish.IRabbitMQService>();
            var message = "Test message";

            // Set up the mock to throw an exception when PublishToTopic is called
            mockRabbitMQService.Setup(service => service.PublishToTopic(It.IsAny<string>()))
                .Throws(new Exception("Test exception"));

            // Act and Assert
            Exception ex = Assert.Throws<Exception>(() => mockRabbitMQService.Object.PublishToTopic(message));
            Assert.Equal("Test exception", ex.Message);
        }

        [Fact]
        public void TestPublishToTopic_MessageSerialization()
        {
            // Arrange
            var mockRabbitMQService = new Mock<Producer.MessagePublish.IRabbitMQService>();
            var message = new Producer.Entities.ProductRequest { Id = Guid.NewGuid(), Name = "Test Product" };

            // Set up the mock to capture the serialized message when PublishToTopic is called
            string capturedMessage = null;
            mockRabbitMQService.Setup(service => service.PublishToTopic(It.IsAny<string>()))
                .Callback<string>((msg) => capturedMessage = msg);

            // Act
            mockRabbitMQService.Object.PublishToTopic(JsonSerializer.Serialize(message));

            // Assert
            var deserializedMessage = JsonSerializer.Deserialize<Producer.Entities.ProductRequest>(capturedMessage);
            Assert.Equal(message.Id, deserializedMessage.Id);
            Assert.Equal(message.Name, deserializedMessage.Name);
        }
    }
}
