using System.Collections.Generic;
using CleanFunc.Infrastructure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;

namespace Infrastructure.IntegrationTests.ServiceBus
{
    public class ServiceBusConfigurationTests
    {
        private const string DummyServiceBusConnectionString = "Endpoint=sb://dummy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dummy";

        [Fact]
        public void EnsureNoOtherConnectionStringsAreLoaded_WhenNoSettingsExist()
        {
            // arrange
            var configSettings = new List<KeyValuePair<string,string>>();
            IConfiguration config = SetupConfiguration(configSettings);

            // act
            var sut = new ServiceBusConfiguration(config);
            sut.Configurations = (config) =>
            {
                return configSettings;
            };

            // assert
            sut.DefaultConnectionString.Should().Be(DummyServiceBusConnectionString);
            sut.OtherConnectionStrings.Should().BeEmpty();
        }

        [Fact]
        public void EnsureOtherConnectionStringsAreLoaded_WhenMultipleSettingsExist()
        {
            // arrange
            var configSettings = new List<KeyValuePair<string, string>>();
            configSettings.Add(new KeyValuePair<string, string>("ServiceBusConnectionString:Queue1", "SomeConnectionStringForQueue"));
            configSettings.Add(new KeyValuePair<string, string>("ServiceBusConnectionString:Topic1", "SomeConnectionStringForTopic"));
            IConfiguration config = SetupConfiguration(configSettings);

            // act
            var sut = new ServiceBusConfiguration(config);
            sut.Configurations = (config) =>
            {
                return configSettings;
            };

            // assert
            sut.DefaultConnectionString.Should().Be(DummyServiceBusConnectionString);
            sut.OtherConnectionStrings.Count.Should().Be(2);
            sut.OtherConnectionStrings.Should().ContainKey("Queue1");
            sut.OtherConnectionStrings.Should().ContainKey("Topic1");
        }

        private static IConfiguration SetupConfiguration(List<KeyValuePair<string, string>> configSettings)
        {
            var config = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns(DummyServiceBusConnectionString);
            
            config.Setup(c => c.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);
            return config.Object;
        }
    }
}