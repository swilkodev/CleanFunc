using System.Reflection;
namespace CleanFunc.Domain.Entities
{
    public class AuditRecord
    {
        public AuditRecord(string action, AuditDetail detail)
        {
            ActionName = action;
            ExecutingApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
            Detail=detail;
        }

        public string ActionName {get; internal set;}

        public string ExecutingApplicationName {get; internal set;}

        public AuditActionStatus ActionStatus {get; set;}

        public string Reason {get;set;}

        public object CustomData {get; set;}

        public AuditDetail Detail {get; internal set;}
    }
}