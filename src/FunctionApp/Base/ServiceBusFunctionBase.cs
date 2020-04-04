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
                                                                    Message message) 
            where TRequest : IRequest
        {
            string contents = Encoding.UTF8.GetString(message.Body);
            var request = JsonConvert.DeserializeObject<TRequest>(contents);

            this.context.CorrelationId = executionContext.InvocationId;
            message.UserProperties.TryGetValue("UserName", out object userName);
            this.context.UserName = (string)userName;
            message.UserProperties.TryGetValue("AuthenticationType", out object authenticationType);
            this.context.AuthenticationType = (string)authenticationType;

            await mediator.Send(request);

        }
    }
}