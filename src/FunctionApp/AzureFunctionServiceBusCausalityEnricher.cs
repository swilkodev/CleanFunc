using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;

namespace CleanFunc.FunctionApp
{
    // This enricher is used to enrich the service bus message with context information to correlate calls between 
    // azure functions and service bus. It is only required when azure service bus output bindings are not used because
    // normally the bindings would do this work.
    public class AzureFunctionServiceBusCausalityEnricher : IMessageEnricher
    {
        private readonly ICallContextProvider provider;

        public AzureFunctionServiceBusCausalityEnricher(ICallContextProvider provider)
        {
            this.provider = provider;
        }

        public Task EnrichAsync(MessageContext context)
        {
            context.UserProperties.Add("$AzureWebJobsParentId", this.provider.CorrelationId);
            return Task.CompletedTask;
        }
    }
}