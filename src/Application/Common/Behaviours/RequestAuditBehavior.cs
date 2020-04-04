using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using MediatR;

namespace CleanFunc.Application.Common.Behaviours
{
    public class RequestAuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAuditor auditor;

        public RequestAuditBehavior(IAuditor auditor)
        {
            this.auditor = auditor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            bool requiresAudit = request is IAuditableRequest<TRequest>;
            Models.AuditEntry entry = null;
            if(requiresAudit)
            {
                // Create audit entry
                entry = await (request as IAuditableRequest<TRequest>).CreateEntryAsync(request);
            }

            try
            {
                var response = await next();

                if(requiresAudit)
                {
                    // record successful audit
                    await this.auditor.AddAsync(
                        new Models.Audit(outcome: AuditOutcome.Success, entry)
                    );
                }

                return response;
            }
            catch (System.Exception ex)
            {
                // Error
                if(requiresAudit)
                {
                    // record audit failure
                    await this.auditor.AddAsync(
                        new Models.Audit(outcome: AuditOutcome.Failure, entry, reason: ex.Message)
                    );
                }
                throw;
            }

        }
    }
}