using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using CleanFunc.Application.Issuers.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Application.Issuers.Commands.CreateIssuer
{
    public class IssuerCreated : INotification
    {
        public Guid IssuerId {get;set;}

        public string IssuerName {get;set;}
        
        public class IssuerCreatedHandler : INotificationHandler<IssuerCreated>
        {
            private readonly ILogger<IssuerCreatedHandler> logger;
            private readonly IEmailService emailService;
            private readonly IMapper mapper;
            private readonly IBusFactory busFactory;
            private readonly IDateTime dateTime;

            public IssuerCreatedHandler(ILogger<IssuerCreatedHandler> logger,
                                            IEmailService emailService, 
                                            IMapper mapper,
                                            IBusFactory busFactory,
                                            IDateTime dateTime)
            {
                Guard.Against.Null(logger, nameof(logger));
                Guard.Against.Null(emailService, nameof(emailService));
                Guard.Against.Null(mapper, nameof(mapper));
                Guard.Against.Null(busFactory, nameof(busFactory));

                this.logger = logger;
                this.emailService = emailService;
                this.mapper = mapper;
                this.busFactory = busFactory;
                this.dateTime = dateTime;
            }

            public async Task Handle(IssuerCreated notification, CancellationToken cancellationToken)
            {
                logger.LogInformation("IssuerCreated notification received.");

                // create integration event
                var issuerEvent = new IssuerChangedEvent()
                {
                    DateTimeOccuredUtc = dateTime.UtcNow,
                    IssuerName = notification.IssuerName
                };
                
                // send integration event to the service bus. Other systems can subscribe to this event if they wish
                var bus = busFactory.Create<IssuerChangedEvent>();
                //await bus.SendAsync(issuerEvent);

                // TODO use details from notification to fill in email dto
                await emailService.SendAsync(new EmailMessageDto());
            }
        }
    }
}