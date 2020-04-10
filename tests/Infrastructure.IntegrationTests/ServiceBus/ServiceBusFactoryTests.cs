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
                var configuration = new Mock<IServiceBusConfiguration>();
                configuration.SetupGet(_ => _.DefaultConnectionString).Returns(DummyServiceBusConnectionString);
                configuration.SetupGet(_ => _.OtherConnectionStrings).Returns(new System.Collections.Generic.Dictionary<string, string>());

                var sut = new ServiceBusFactory(configuration.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = sut.Create<Foo>();
                var sender2 = sut.Create<Foo>();

                // assert
                Assert.Same(sender, sender2);
        }

        [Fact]
        public void Create_GivenDifferentPayload_ShouldReturnDifferentSenderInstance()
        {
                // arrange
                var configuration = new Mock<IServiceBusConfiguration>();
                configuration.SetupGet(_ => _.DefaultConnectionString).Returns(DummyServiceBusConnectionString);
                configuration.SetupGet(_ => _.OtherConnectionStrings).Returns(new System.Collections.Generic.Dictionary<string, string>());

                var sut = new ServiceBusFactory(configuration.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = sut.Create<Foo>();
                var sender2 = sut.Create<AnotherFoo>();

                // assert
                Assert.NotSame(sender, sender2);
        }

        [Fact]
        public void Create_GivenSameQueue_ShouldReturnSameSenderInstance()
        {
                // arrange
                var configuration = new Mock<IServiceBusConfiguration>();
                configuration.SetupGet(_ => _.DefaultConnectionString).Returns(DummyServiceBusConnectionString);
                configuration.SetupGet(_ => _.OtherConnectionStrings).Returns(new System.Collections.Generic.Dictionary<string, string>());

                var sut = new ServiceBusFactory(configuration.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = sut.Create("MyQueue");
                var sender2 = sut.Create("MyQueue");

                // assert
                Assert.Same(sender, sender2);
        }

         [Fact]
        public void Create_GivenDifferentQueue_ShouldReturnDifferentSenderInstance()
        {
                // arrange
                var configuration = new Mock<IServiceBusConfiguration>();
                configuration.SetupGet(_ => _.DefaultConnectionString).Returns(DummyServiceBusConnectionString);
                configuration.SetupGet(_ => _.OtherConnectionStrings).Returns(new System.Collections.Generic.Dictionary<string, string>());
                
                var sut = new ServiceBusFactory(configuration.Object, Enumerable.Empty<IMessageEnricher>());

                // act
                var sender = sut.Create("MyQueue");
                var sender2 = sut.Create("MyQueue2");

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