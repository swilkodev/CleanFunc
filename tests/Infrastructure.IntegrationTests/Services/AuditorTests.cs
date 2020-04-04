using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.Infrastructure.Services;
using Moq;
using Xunit;
using CleanFunc.Application.Common.Models;

namespace Infrastructure.IntegrationTests.Services
{
    public class AuditorTests
    {
        [Fact]
        public async Task AddAuditMessage_ShouldCallSenderSendAsync()
        {
            // arrange
            var callContext = new Mock<ICallContext>();
            var factory = new Mock<IBusFactory>();
            var sender = new Mock<IBusMessageSender>();
            factory.Setup(x => x.Create<CreateAuditCommand>()).Returns(sender.Object);

            var auditor = new Auditor(callContext.Object, factory.Object);
            var auditEntry = new AuditEntry("CustomerCreate", action: "Modify")
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
            sender.Verify(x => x.SendAsync<CreateAuditCommand>(It.IsAny<CreateAuditCommand>()), Times.Once);
        }
    }
}