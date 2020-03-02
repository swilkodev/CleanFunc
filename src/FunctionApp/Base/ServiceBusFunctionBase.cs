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
        private readonly ICallContextProvider context;

        protected ServiceBusFunctionBase(IMediator mediator, ICallContextProvider context)
        {
            this.mediator = mediator;
            this.context = context;
        }

        protected async Task ExecuteAsync<TRequest>(ExecutionContext executionContext, 
                                                                    Message message) 
            where TRequest : IRequest
        {
            //try
            //{
                string contents = Encoding.UTF8.GetString(message.Body);
                var request = JsonConvert.DeserializeObject<TRequest>(contents);

                this.context.CorrelationId = executionContext.InvocationId;
                message.UserProperties.TryGetValue("UserName", out object userName);
                this.context.UserName = (string)userName;
                message.UserProperties.TryGetValue("UserType", out object userType);
                this.context.UserType = (string)userType;

                await mediator.Send(request);

                // if(resultMethod != null)
                //     return resultMethod(response);

                // return new OkObjectResult(response);
            //}
            // catch (Application.Common.Exceptions.ValidationException validationException)
            // {
            //     // var result = new
            //     // {
            //     //     message = "Validation failed.",
            //     //     errors = validationException.Failures.Select(x => new
            //     //     {
            //     //         x.PropertyName,
            //     //         x.ErrorMessage,
            //     //         x.ErrorCode
            //     //     })
            //     // };


            //     // var result = JsonConvert.SerializeObject(validationException.Failures);

            //     // return new BadRequestObjectResult(result);
            // }
            // catch (Application.Common.Exceptions.NotFoundException)
            // {
                
            // }
        }
    }
}