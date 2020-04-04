namespace CleanFunc.Application.Common.Models
{
    public class Audit
    {
        public AuditOutcome Outcome { get; }
        public AuditEntry Entry { get; }
        public string Reason { get; }
        public Audit(AuditOutcome outcome, AuditEntry entry, string reason = "")
        {
            this.Reason = reason;
            this.Entry = entry;
            this.Outcome = outcome;
        }
    }
}