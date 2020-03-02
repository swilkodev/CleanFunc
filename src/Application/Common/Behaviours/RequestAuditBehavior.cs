using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
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
            bool requiresAudit = request is IAuditHandler<TRequest>;
            AuditDetail detail = null;
            if(requiresAudit)
            {
                detail = await (request as IAuditHandler<TRequest>).HandleAsync(new AuditContext<TRequest>(request));
            }

            try
            {
                var response = await next();

                if(requiresAudit)
                {
                    var entry=new AuditRecord(request.GetType().Name, detail)
                    {
                        ActionStatus = AuditActionStatus.Success
                    };
                    await this.auditor.AddAsync(entry);
                }

                return response;
            }
            catch (System.Exception ex)
            {
                // Error
                if(requiresAudit)
                {
                    var entry=new AuditRecord(request.GetType().Name, detail)
                    {
                        ActionStatus = AuditActionStatus.Failure,
                        Reason = ex.Message
                    };
                    await this.auditor.AddAsync(entry);
                }
                throw;
            }

        }
    }
}