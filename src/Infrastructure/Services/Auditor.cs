using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using CleanFunc.Application.Audit.Messages;

namespace CleanFunc.Infrastructure.Services
{
    public class Auditor : IAuditor
    {
        private readonly ICallContext callContext;
        private readonly IBus bus;

        public Auditor(ICallContext callContext, IBusFactory busFactory)
        {
            Guard.Against.Null(callContext, nameof(callContext));
            Guard.Against.Null(busFactory, nameof(busFactory));

            this.callContext = callContext;
            this.bus = busFactory.Create<AuditMessage>();
        }

        public async Task AddAsync(Audit audit)
        {
            // create audit integration message
            
            // at this point we will also include some call context data
            var cmd = new AuditMessage
            {
                Name = audit.Entry.Name,
                Action = audit.Entry.Action,
                UserName = callContext.UserName,
                AuthenticationType = callContext.AuthenticationType,
                CorrelationId = callContext.CorrelationId,
                DateOccuredUtc = audit.Entry.DateOccuredUtc,
                Outcome = audit.Outcome.ToString(),
                Reason = audit.Reason,
                EntityType = audit.Entry.ActionTarget.EntityType,
                EntityKey = audit.Entry.ActionTarget.EntityKey,
                ExecutingApplication = audit.Entry.ExecutingApplication,
                CustomData = audit.Entry.CustomData
            };

            // send message command to integration bus
            await this.bus.SendAsync(cmd);
        }
    }

}