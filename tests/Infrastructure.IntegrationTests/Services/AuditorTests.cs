using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.Infrastructure.ServiceBus;
using CleanFunc.Infrastructure.Services;
using Moq;
using Xunit;

namespace Infrastructure.IntegrationTests.Services
{
    public class AuditorTests
    {
        [Fact]
        public async Task AddAuditMessage_ShouldCallSenderSendAsync()
        {
            var callContext = new Mock<ICallContextProvider>();
            var factory = new Mock<IBusFactory>();
            var sender = new Mock<IBusMessageSender>();
            factory.Setup(x => x.Create<CreateAuditCommand>()).Returns(sender.Object);

            var auditor = new Auditor(callContext.Object, factory.Object);
            await auditor.AddAsync(new CleanFunc.Domain.Entities.AuditRecord("TEST", null)
            {
            });

            sender.Verify(x => x.SendAsync<CreateAuditCommand>(It.IsAny<CreateAuditCommand>()), Times.Once);
        }
    }
}