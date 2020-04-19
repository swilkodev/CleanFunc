using System.Linq;
using CleanFunc.Infrastructure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;

namespace Infrastructure.IntegrationTests.Services
{
    public class ServiceBusFactoryTests
    {
        private const string DummyServiceBusConnectionString = "Endpoint=sb://dummy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dummy";
        

        [Fact]
        public void WhenGivenExistingPayload_ShouldReturnSameSenderInstance()
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
                sender.Should().BeSameAs(sender2);
        }

        [Fact]
        public void WhenGivenDifferentPayload_ShouldReturnDifferentSenderInstance()
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
                sender.Should().NotBeSameAs(sender2);
        }

        [Fact]
        public void WhenGivenSameQueue_ShouldReturnSameSenderInstance()
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
                sender.Should().BeSameAs(sender2);
        }

         [Fact]
        public void WhenGivenDifferentQueue_ShouldReturnDifferentSenderInstance()
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
                sender.Should().NotBeSameAs(sender2);
        }
    }

    public class Foo
    {
    }

    public class AnotherFoo
    {
    }
}