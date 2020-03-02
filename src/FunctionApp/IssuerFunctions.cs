using System.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using CleanFunc.Application.Issuers.Queries.GetIssuer;
using System.Linq;
using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using CleanFunc.Application.Issuers.Queries.ExportIssuers;
using CleanFunc.Application.Issuers.Commands.CreateIssuer;
using CleanFunc.Application.Common.Interfaces;
using System.IO.Compression;
using System.Collections.Generic;
using CleanFunc.Application.Issuers.Commands.ImportIssuers;
using CleanFunc.FunctionApp.Base;

namespace CleanFunc.FunctionApp
{
    public class IssuerFunctions : HttpFunctionBase
    {
        public IssuerFunctions(IMediator mediator, 
                                ICallContextProvider callContext) 
            : base(mediator, 
                    callContext)
        {
        }
        
        [FunctionName("GetIssuer")]
        public async Task<IActionResult> GetIssuer(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "issuers/{id}")] GetIssuerQuery queryArg, 
                                                                                        HttpRequest req, 
                                                                                        Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            return await ExecuteAsync<GetIssuerQuery, GetIssuerResponse>(context, 
                                                                                req, 
                                                                                queryArg, 
                                                                                (r) => new OkObjectResult(r).ToTask());
        }


        [FunctionName("ImportIssuers")]
        public async Task<IActionResult> ImportIssuers(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "issuers/import")] HttpRequest req,
                                                                                            Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            var command = new ImportIssuersCommand
            {
                Data = req.Body
            };
            
            return await ExecuteAsync<ImportIssuersCommand, long>(context, 
                                                                                req, 
                                                                                command, 
                                                                                (r) => new OkObjectResult(r).ToTask());
        }

        [FunctionName("ExportIssuers")]
        public async Task<IActionResult> ExportIssuers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "issuers/export")] ExportIssuersQuery queryArg, 
                                                                                            HttpRequest req,
                                                                                            IDictionary<string, string> headers,
                                                                                            Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            return await ExecuteAsync<ExportIssuersQuery, ExportIssuersResponse>(context,
                                                                                        req,
                                                                                        queryArg, 
                                                                                    (r) => {

                                                                                        var result = new FileContentResult(r.Content, r.ContentType)  
                                                                                        { 
                                                                                            FileDownloadName = r.FileName 
                                                                                        };
                                                                                        return result.ToTask();


                                                                                    });
        }

        [FunctionName("ExportIssuersZip")]
        public async Task<IActionResult> ExportIssuersZip(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "issuers/export/zip")] ExportIssuersQuery queryArg, 
                                                                                            HttpRequest req,
                                                                                            Microsoft.Azure.WebJobs.ExecutionContext context,
                                                                                            ILogger log)
        {

            return await ExecuteAsync<ExportIssuersQuery, ExportIssuersResponse>(context,
                                                                                        req,
                                                                                        queryArg, 
                                                                                     async (r) => {
                                                                                        
                                                                                        var outputStream=new MemoryStream();
                                                                                        using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
                                                                                        {
                                                                                            var zipEntry = zipArchive.CreateEntry(r.FileName);
                                                                                            using (var zipStream = zipEntry.Open())
                                                                                            {
                                                                                                using(var ms = new MemoryStream(r.Content))
                                                                                                {
                                                                                                    await ms.CopyToAsync(zipStream);
                                                                                                }
                                                                                            }
                                                                                        }

                                                                                        outputStream.Position=0;
                                                                                        var result = new FileStreamResult(outputStream, "application/octet-stream")  
                                                                                        { 
                                                                                            FileDownloadName = "export.zip" 
                                                                                        };
                                                                                        return result;

                                                                                    });
        }

        [FunctionName("CreateIssuer")]
        public async Task<IActionResult> CreateIssuer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "issuers")] CreateIssuerCommand commandArg, 
                                                                                        HttpRequest req,
                                                                                        Microsoft.Azure.WebJobs.ExecutionContext context,
                                                                                        ILogger log)
        {
            var result =  await ExecuteAsync<CreateIssuerCommand, Guid>(context, 
                                                                                req,
                                                                                commandArg, 
                                                                                (r) => new OkObjectResult(r).ToTask());
                                                                                


            return result;
        }
    }

    // public class FunctionValidationAttribute : Attribute, IFunctionInvocationFilter
    // {
    //     public FunctionAuthorizeAttribute()
    //     {
    //     }

    //     public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
    //     {
    //         return Task.CompletedTask;
    //     }

    //     public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
    //     {
    //         return Task.CompletedTask;
    //     }
    // }

    
}
