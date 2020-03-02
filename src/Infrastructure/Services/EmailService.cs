using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;
        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public Task SendAsync(EmailMessageDto message)
        {
            Guard.Against.Null(message, nameof(message));

            logger.LogCritical("Sending email");

            return Task.CompletedTask;
        }
    }
}