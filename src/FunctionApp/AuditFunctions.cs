using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.FunctionApp.Base;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using CleanFunc.Application.Audit.Messages;
using AutoMapper;

namespace CleanFunc.FunctionApp
{
    /// <summary>
    /// NOTE: Just an example of consuming messages from service bus. Would normally be in a different service/solution.
    /// </summary>
    public class AuditFunctions : ServiceBusFunctionBase
    {
        private readonly IMapper mapper;

        public AuditFunctions(IMediator mediator, 
                                ICallContext callContext,
                                IMapper mapper) 
            : base(mediator, 
                    callContext)
        {
            this.mapper = mapper;
        }
        
        [FunctionName("DoAudit")]
        public async Task DoAudit(
            [ServiceBusTrigger(Endpoints.AuditMessage, Connection = "ServiceBusConnectionString")] 
                                                                                    AuditMessage message,
                                                                                    Microsoft.Azure.WebJobs.ExecutionContext context,
                                                                                    ILogger log)
        {           
            // Map the integration message to a command for our handler
            var command = mapper.Map<CreateAuditCommand>(message); 

            // execute command
            await this.ExecuteAsync<CreateAuditCommand>(context, command);
        }


        private static class Endpoints
        {
            public const string AuditMessage = "cleanfunc.application.audit.messages.auditmessage";
        }
    }

    
}