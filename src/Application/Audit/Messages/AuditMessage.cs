using System;

namespace CleanFunc.Application.Audit.Messages
{
    /// <summary>
    /// Represents the integration message which enables services to publish audit messages to the audit system.
    /// </summary>
    public class AuditMessage
    {
        public string Name {get;set;}
        public string Action {get;set;}
        public string UserName {get;set;}
        public string AuthenticationType {get;set;}
        public Guid CorrelationId {get;set;}
        public DateTime DateOccuredUtc {get;set;}
        public string Reason {get;set;}
        public string Outcome {get; set;}
        public string EntityType {get;set;}
        public string EntityKey {get;set;}
        public object CustomData {get;set;}
        public string ExecutingApplication {get; set;}
    }
}