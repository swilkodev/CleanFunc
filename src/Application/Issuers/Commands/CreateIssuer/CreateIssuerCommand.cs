using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using CleanFunc.Domain.Entities;
using MediatR;

namespace CleanFunc.Application.Issuers.Commands.CreateIssuer
{
    public class CreateIssuerCommand : IRequest<Guid>, IAuditableRequest<CreateIssuerCommand>
    {
        public string Name {get;set;}

        public Task<Common.Models.AuditEntry> CreateEntryAsync(CreateIssuerCommand request)
        {
            var auditEntry = new Common.Models.AuditEntry(name: nameof(CreateIssuerCommand), action: "Create")
            {
                ActionTarget = new ActionTarget
                {
                    EntityType = nameof(Issuer),
                    EntityKey = request.Name
                },
                CustomData = new {
                    SomeProperty = "Foo"
                },
            };
            return Task.FromResult(auditEntry);
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

                // publish application event
                await mediator.Publish(new IssuerCreated { IssuerId = entity.Id }, cancellationToken);

                return entity.Id;
            }
        }
    }
}