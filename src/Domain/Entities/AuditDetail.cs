namespace CleanFunc.Domain.Entities
{
    public class AuditDetail
    {
        public string EntityType {get;set;}
        public string EntityKey1 {get;set;}
        public string EntityKey2 {get;set;}
        public object CustomData {get;set;}
    }
}