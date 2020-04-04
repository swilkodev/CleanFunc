using System;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace CleanFunc.FunctionApp.Base
{
    /// <summary>
    /// Represents the base class for an azure http triggered function, which allows us to initialize the call context prior to using mediator
    /// to call the application. We can also translate exceptions back into their HTTP equivilents. Normally this sort of thing would be done
    /// in a middleware layer however azure functons lack middleware support at this time.
    /// </summary>
    public abstract class HttpFunctionBase
    {
        private readonly IMediator mediator;
        private readonly ICallContext context;

        protected HttpFunctionBase(IMediator mediator, ICallContext context)
        {
            this.mediator = mediator;
            this.context = context;
        }

        protected async Task<IActionResult> ExecuteAsync<TRequest, TResponse>(ExecutionContext executionContext, 
                                                                                HttpRequest httpRequest,
                                                                                TRequest request, 
                                                                                Func<TResponse, Task<IActionResult>> resultMethod = null) 
            where TRequest : IRequest<TResponse>
        {
            try
            {
                // Populate the context with information. This can be used by injecting the call context into any class
                this.context.CorrelationId = executionContext.InvocationId;
                this.context.FunctionName = executionContext.FunctionName;
                this.context.UserName = httpRequest.HttpContext.User?.Identity?.Name;
                this.context.AuthenticationType = httpRequest.HttpContext.User?.Identity?.AuthenticationType;

                var response = await mediator.Send(request);

                if(resultMethod != null)
                    return await resultMethod(response);

                return new OkObjectResult(response);
            }
            catch (Application.Common.Exceptions.ValidationException validationException)
            {
                var result = JsonConvert.SerializeObject(validationException.Failures);

                return new BadRequestObjectResult(result);
            }
            catch (Application.Common.Exceptions.NotFoundException)
            {
                return new NotFoundResult();
            }
        }
    }
}