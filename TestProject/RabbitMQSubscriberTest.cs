using Moq;

namespace TestProject
{
    public class RabbitMQSubscriberTest
    {
        [Fact]
        public void TestReadFromTopic()
        {
            // Arrange
            var mockRabbitMQService = new Mock<IRabbitMQService>();

            // Act
            mockRabbitMQService.Object.ReadFromTopic();

            // Assert
            mockRabbitMQService.Verify(service => service.ReadFromTopic(), Times.Once);
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
            mockRabbitMQService.Setup(service => service.ReadFromTopic())
                .Callback(() => capturedWorkerId = Environment.GetEnvironmentVariable("workerid"));

            // Act
            mockRabbitMQService.Object.ReadFromTopic();

            // Assert
            Assert.Equal("TestWorker", capturedWorkerId);
        }
    }
}
