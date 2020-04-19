using System.Text;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace CleanFunc.FunctionApp.Base
{
    public class ServiceBusFunctionBase
    {
        private readonly IMediator mediator;
        private readonly ICallContext context;

        protected ServiceBusFunctionBase(IMediator mediator, ICallContext context)
        {
            this.mediator = mediator;
            this.context = context;
        }

        protected async Task ExecuteAsync<TRequest>(ExecutionContext executionContext, 
                                                                    TRequest request) 
            where TRequest : IRequest
        {

            this.context.CorrelationId = executionContext.InvocationId;
            this.context.FunctionName = executionContext.FunctionName;
            
            await mediator.Send(request);
        }
    }
}