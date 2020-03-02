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
    public abstract class HttpFunctionBase
    {
        private readonly IMediator mediator;
        private readonly ICallContextProvider context;

        protected HttpFunctionBase(IMediator mediator, ICallContextProvider context)
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
                this.context.CorrelationId = executionContext.InvocationId;
                this.context.UserName = httpRequest.HttpContext.User?.Identity?.Name;
                this.context.UserType = httpRequest.HttpContext.User?.Identity?.AuthenticationType;
                // if(httpRequest.Headers.TryGetValue("X-API-KEY", out var apiKey))
                // {
                    
                // }

                var response = await mediator.Send(request);

                if(resultMethod != null)
                    return await resultMethod(response);

                return new OkObjectResult(response);
            }
            catch (Application.Common.Exceptions.ValidationException validationException)
            {
                // var result = new
                // {
                //     message = "Validation failed.",
                //     errors = validationException.Failures.Select(x => new
                //     {
                //         x.PropertyName,
                //         x.ErrorMessage,
                //         x.ErrorCode
                //     })
                // };

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