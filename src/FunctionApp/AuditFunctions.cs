using System;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Audit.Commands.CreateAudit;
using CleanFunc.FunctionApp.Base;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CleanFunc.FunctionApp
{
    /// <summary>
    /// NOTE: Just an example of consuming messages from service bus. Would normally be in a different service.
    /// </summary>
    public class AuditFunctions : ServiceBusFunctionBase
    {
        public AuditFunctions(IMediator mediator, 
                                ICallContextProvider callContext) 
            : base(mediator, 
                    callContext)
        {
        }
        
        [FunctionName("DoAudit")]
        public async Task DoAudit(
            [ServiceBusTrigger(Endpoints.CreateAuditCommand, Connection = "ServiceBusConnectionString")] 
                                                                                    Message message,
                                                                                    Microsoft.Azure.WebJobs.ExecutionContext context,
                                                                                    ILogger log)
        {
            await this.ExecuteAsync<CreateAuditCommand>(context, message);
        }


        private static class Endpoints
        {
            public const string CreateAuditCommand = "cleanfunc.application.audit.commands.createaudit.createauditcommand";
        }
    }

    
}