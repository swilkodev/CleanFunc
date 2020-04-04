using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Application.Issuers.Commands.CreateIssuer
{
    public class IssuerCreated : INotification
    {
        public Guid IssuerId {get;set;}
        
        public class IssuerCreatedHandler : INotificationHandler<IssuerCreated>
        {
            private readonly ILogger<IssuerCreatedHandler> logger;
            private readonly IEmailService emailService;
            private readonly IBusFactory messageSenderFactory;

            public IssuerCreatedHandler(ILogger<IssuerCreatedHandler> logger,
                                            IEmailService emailService, 
                                            IBusFactory messageSenderFactory)
            {
                Guard.Against.Null(logger, nameof(logger));
                Guard.Against.Null(emailService, nameof(emailService));
                Guard.Against.Null(messageSenderFactory, nameof(messageSenderFactory));
                this.logger = logger;
                this.emailService = emailService;
                this.messageSenderFactory = messageSenderFactory;
            }

            public async Task Handle(IssuerCreated notification, CancellationToken cancellationToken)
            {
                logger.LogInformation("IssuerCreated notification received.");

                // send event to the service bus
                var sender = messageSenderFactory.Create<IssuerCreated>();
                //await sender.SendAsync(notification);

                // TODO use details from notification to fill in email dto
                await emailService.SendAsync(new EmailMessageDto());
            }
        }
    }
}