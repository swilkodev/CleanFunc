using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Application.Audit.Commands.CreateAudit
{
    public class CreateAuditCommand : IRequest
    {
        public string ActionName {get;set;}
        public string User {get;set;}
        public string UserType {get;set;}
        public Guid CorrelationId {get;set;}
        
        public CreateAuditCommand()
        {
        }

        public class CreateAuditCommandHander : AsyncRequestHandler<CreateAuditCommand>
        {
            private readonly ILogger<CreateAuditCommandHander> logger;

            public CreateAuditCommandHander(ILogger<CreateAuditCommandHander> logger)
            {
                Guard.Against.Null(logger, nameof(logger));
                
                this.logger = logger;
            }

            protected override Task Handle(CreateAuditCommand request, CancellationToken cancellationToken)
            {
                this.logger.LogWarning($"Audit:ActionName {request.ActionName}");
                this.logger.LogWarning($"Audit:User {request.User}");
                this.logger.LogWarning($"Audit:UserType {request.UserType}");
                this.logger.LogWarning($"Audit:CorrelationId {request.CorrelationId}");

                // TODO Write to audit database
                
                return Task.CompletedTask;
            }
        }
    }


}