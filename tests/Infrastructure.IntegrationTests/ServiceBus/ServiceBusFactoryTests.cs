using System.Linq;
using CleanFunc.Infrastructure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Infrastructure.IntegrationTests.Services
{
    public class ServiceBusFactoryTests
    {
        private const string DummyServiceBusConnectionString = "Endpoint=sb://dummy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dummy";
        

        [Fact]
        public void Create_GivenExistingPayload_ShouldReturnSameSenderInstance()
        {
                // arrange
                // note: We cannot mock IConfiguration.GetValue<T>() as it is an extension method
                // so a workaround is to mock the config section calls as this is what the underlying extension method calls
                var config = new Mock<IConfiguration>();
                var configurationSection = new Mock<IConfigurationSection>();
                configurationSection.Setup(a => a.Value).Returns(DummyServiceBusConnectionString);
                config.Setup(c => c.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);  

                var factory = new ServiceBusFactory(config.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = factory.Create<Foo>();
                var sender2 = factory.Create<Foo>();

                // assert
                Assert.Same(sender, sender2);
        }

        [Fact]
        public void Create_GivenDifferentPayload_ShouldReturnDifferentSenderInstance()
        {
                // arrange
                // note: We cannot mock IConfiguration.GetValue<T>() as it is an extension method
                // so a workaround is to mock the config section calls as this is what the underlying extension method calls
                var config = new Mock<IConfiguration>();
                var configurationSection = new Mock<IConfigurationSection>();
                configurationSection.Setup(a => a.Value).Returns(DummyServiceBusConnectionString);
                config.Setup(c => c.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);  

                var factory = new ServiceBusFactory(config.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = factory.Create<Foo>();
                var sender2 = factory.Create<AnotherFoo>();

                // assert
                Assert.NotSame(sender, sender2);
        }

        [Fact]
        public void Create_GivenSameQueue_ShouldReturnSameSenderInstance()
        {
                // arrange
                // note: We cannot mock IConfiguration.GetValue<T>() as it is an extension method
                // so a workaround is to mock the config section calls as this is what the underlying extension method calls
                var config = new Mock<IConfiguration>();
                var configurationSection = new Mock<IConfigurationSection>();
                configurationSection.Setup(a => a.Value).Returns(DummyServiceBusConnectionString);
                config.Setup(c => c.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);  

                var factory = new ServiceBusFactory(config.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = factory.Create("MyQueue");
                var sender2 = factory.Create("MyQueue");

                // assert
                Assert.Same(sender, sender2);
        }

         [Fact]
        public void Create_GivenDifferentQueue_ShouldReturnDifferentSenderInstance()
        {
                // arrange
                // note: We cannot mock IConfiguration.GetValue<T>() as it is an extension method
                // so a workaround is to mock the config section calls as this is what the underlying extension method calls
                var config = new Mock<IConfiguration>();
                var configurationSection = new Mock<IConfigurationSection>();
                configurationSection.Setup(a => a.Value).Returns(DummyServiceBusConnectionString);
                config.Setup(c => c.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);  

                var factory = new ServiceBusFactory(config.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = factory.Create("MyQueue");
                var sender2 = factory.Create("MyQueue2");

                // assert
                Assert.NotSame(sender, sender2);
        }
    }

    public class Foo
    {
    }

    public class AnotherFoo
    {
    }
}