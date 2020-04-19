using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.Infrastructure.Services;
using Moq;
using Xunit;
using CleanFunc.Application.Common.Models;
using CleanFunc.Application.Audit.Messages;

namespace Infrastructure.IntegrationTests.Services
{
    public class AuditorTests
    {
        [Fact]
        public async Task AddAuditMessage_ShouldCallSenderSendAsync()
        {
            // arrange
            var callContext = new Mock<ICallContext>();
            var factory = new Mock<IBusEndpointFactory>();
            var sender = new Mock<IBusEndpoint>();
            factory.Setup(x => x.Create<AuditMessage>()).Returns(sender.Object);

            var auditor = new Auditor(callContext.Object, factory.Object);
            var auditEntry = new AuditEntry("Issuer", action: "Modify")
            {
                ActionTarget = new ActionTarget
                {
                    EntityType = "Customer",
                    EntityKey = "12345678"
                }
            };
            // act
            await auditor.AddAsync(new Audit(AuditOutcome.Success, auditEntry));

            // assert
            sender.Verify(x => x.SendAsync<AuditMessage>(It.IsAny<AuditMessage>()), Times.Once);
        }
    }
}