using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
using MediatR;

namespace CleanFunc.Application.Issuers.Commands.CreateIssuer
{
    public class CreateIssuerCommand : IRequest<Guid>, IAuditHandler<CreateIssuerCommand>
    {
        public string Name {get;set;}

        public Task<AuditDetail> HandleAsync(AuditContext<CreateIssuerCommand> context)
        {
            return Task.FromResult(new AuditDetail
            {
                EntityType = "ISSUER",
                EntityKey1 = context.Request.Name,
                CustomData = new {
                    SomeProperty = "Foo"
                }
            });
        }

        public class CreateIssuerCommandHandler : IRequestHandler<CreateIssuerCommand, Guid>
        {
            private readonly IIssuerRepository issuerRepository;
            private readonly IMediator mediator;

            public CreateIssuerCommandHandler(IIssuerRepository issuerRepository,
                                                IMediator mediator)
            {
                Guard.Against.Null(issuerRepository, nameof(issuerRepository));
                Guard.Against.Null(mediator, nameof(mediator));

                this.issuerRepository = issuerRepository; 
                
                this.mediator = mediator;
            }

            public async Task<Guid> Handle(CreateIssuerCommand request, CancellationToken cancellationToken)
            {
                var entity = new Issuer
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    CreatedDate = DateTime.Now
                };

                await issuerRepository.Add(entity);

                // publish event
                await mediator.Publish(new IssuerCreated { IssuerId = entity.Id }, cancellationToken);

                return entity.Id;
            }
        }
    }
}