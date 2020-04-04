using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;

namespace CleanFunc.Infrastructure.ServiceBus
{
    // This enricher is used to enrich the service bus message with context information to correlate calls between 
    // azure functions and service bus. It is only required when azure service bus output bindings are not used because
    // normally the bindings would do this work.
    public class AzureFunctionServiceBusCausalityEnricher : IMessageEnricher
    {
        private readonly ICallContext context;

        public AzureFunctionServiceBusCausalityEnricher(ICallContext context)
        {
            this.context = context;
        }

        public Task EnrichAsync(MessageContext context)
        {
            context.UserProperties.Add("$AzureWebJobsParentId", this.context.CorrelationId);
            return Task.CompletedTask;
        }
    }
}