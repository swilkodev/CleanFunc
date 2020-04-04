using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Application.Audit.Commands.CreateAudit
{
    public class CreateAuditCommand : IRequest
    {
        public string Name {get;set;}
        public string Action {get;set;}
        public string UserName {get;set;}
        public string AuthenticationType {get;set;}
        public Guid CorrelationId {get;set;}
        public DateTime DateOccuredUtc {get;set;}
        public string Reason {get;set;}
        public string Outcome {get; internal set;}
        public string EntityType {get;set;}
        public string EntityKey {get;set;}
        public object CustomData {get;set;}
        public string ExecutingApplication {get; internal set;}

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
                this.logger.LogCritical($"Audit:Name {request.Name}");
                this.logger.LogCritical($"Audit:Action {request.Action}");
                this.logger.LogCritical($"Audit:Outcome {request.Outcome}");
                this.logger.LogCritical($"Audit:Reason {request.Reason}");
                this.logger.LogCritical($"Audit:DateOccuredUtc {request.DateOccuredUtc}");
                this.logger.LogCritical($"Audit:UserName {request.UserName}");
                this.logger.LogCritical($"Audit:AuthenticationType {request.AuthenticationType}");
                this.logger.LogCritical($"Audit:CorrelationId {request.CorrelationId}");
                this.logger.LogCritical($"Audit:EntityType {request.EntityType}");
                this.logger.LogCritical($"Audit:EntityKey {request.EntityKey}");
                this.logger.LogCritical($"Audit:ExecutingApplication {request.ExecutingApplication}");
                //this.logger.LogCritical($"Audit:CustomData {JsonConvert.Serializer(request.CustomData)}");

                // TODO Write to audit system
                
                return Task.CompletedTask;
            }
        }
    }
}