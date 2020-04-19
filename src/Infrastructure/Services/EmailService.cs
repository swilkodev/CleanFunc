using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
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
            
            // Likely use something like SendGrid here.

            // Consider sending the email message to a queue so that retries can be handled.

            return Task.CompletedTask;
        }
    }
}