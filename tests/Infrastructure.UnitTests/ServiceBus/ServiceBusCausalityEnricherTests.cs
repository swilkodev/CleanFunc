using System.Collections.Generic;
using System;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Infrastructure.ServiceBus;
using Moq;
using Xunit;
using Shouldly;
using Microsoft.Azure.ServiceBus;

namespace Infrastructure.IntegrationTests.ServiceBus
{
    public class ServiceBusCausalityEnricherTests
    {
        [Fact]
        public void EnsureServiceBusMessagContext_IsEnriched_WithCorrectValues()
        {
            var guid = new Guid("3752bb6d-1474-49b6-8081-3c88b45305f9");
            // arrange
            var callContext = new Mock<ICallContext>();
            callContext.SetupGet(_ => _.CorrelationId).Returns(guid);

            Message message = new Message();

            var sut = new ServiceBusCausalityEnricher(callContext.Object);
            // act
            sut.EnrichAsync(message);

            // assert
            message.UserProperties.ShouldContainKey("$AzureWebJobsParentId");
            message.UserProperties["$AzureWebJobsParentId"].ShouldBe(guid);
        }
    }
}