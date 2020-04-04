using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using IBusMessageSender = CleanFunc.Application.Common.Interfaces.IBusMessageSender;
using CleanFunc.Application.Common.Models;

namespace CleanFunc.Infrastructure.Services
{
    public class Auditor : IAuditor
    {
        private readonly ICallContext callContext;
        private readonly IBusMessageSender messageSender;

        public Auditor(ICallContext callContext, IBusFactory busFactory)
        {
            Guard.Against.Null(callContext, nameof(callContext));
            Guard.Against.Null(busFactory, nameof(busFactory));

            this.callContext = callContext;
            this.messageSender = busFactory.Create<CreateAuditCommand>();
        }

        public async Task AddAsync(Audit audit)
        {
            // create command
            // at this point we will include some call context data
            var cmd = new CreateAuditCommand
            {
                Name = audit.Entry.Name,
                Action = audit.Entry.Action,
                UserName = callContext.UserName,
                AuthenticationType = callContext.AuthenticationType,
                CorrelationId = callContext.CorrelationId,
                DateOccuredUtc = audit.Entry.DateOccuredUtc,
                Reason = audit.Reason,
                CustomData = audit.Entry.CustomData
            };
            // send audit command to service bus
            await this.messageSender.SendAsync(cmd);
        }
    }

}