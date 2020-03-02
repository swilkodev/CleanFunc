using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.Domain.Entities;
using IBusMessageSender = CleanFunc.Application.Common.Interfaces.IBusMessageSender;

namespace CleanFunc.Infrastructure.Services
{
    public class Auditor : IAuditor
    {
        private readonly ICallContextProvider callContext;
        private readonly IBusMessageSender messageSender;

        public Auditor(ICallContextProvider callContext, IBusFactory busFactory)
        {
            Guard.Against.Null(callContext, nameof(callContext));
            Guard.Against.Null(busFactory, nameof(busFactory));

            this.callContext = callContext;
            this.messageSender = busFactory.Create<CreateAuditCommand>();
        }

        public async Task AddAsync(AuditRecord record)
        {
            // create command
            var cmd = new CreateAuditCommand
            {
                ActionName = record.ActionName,
                User = callContext.UserName,
                UserType = callContext.UserType,
                CorrelationId = callContext.CorrelationId
            };
            await this.messageSender.SendAsync(cmd);
        }
    }

}