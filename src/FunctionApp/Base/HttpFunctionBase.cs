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
                // NOTE: ValidationProblemDetails ctor(Dictionary<string, string>) is part of MvC.Core 2.2 so an explicit reference was added to pull in this class because Azure Functions uses v2.1
                var details = new ValidationProblemDetails(validationException.Errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };

                return new BadRequestObjectResult(details);
            }
            catch (Application.Common.Exceptions.NotFoundException notFoundException)
            {
                var details = new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "The specified resource was not found.",
                    Detail = notFoundException.Message
                };

                return new NotFoundObjectResult(details);
            }
            catch (Application.Common.Exceptions.DuplicateItemException duplicateException)
            {
                var details = new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    Title = "The request could not be completed due to a conflict.",
                    Detail = duplicateException.Message
                };

                return new ConflictObjectResult(details);
            }
            catch (Exception)
            {
                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An error occurred while processing your request.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };

                return new ObjectResult(details)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}