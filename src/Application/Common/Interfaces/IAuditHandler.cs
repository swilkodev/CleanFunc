using System.Threading.Tasks;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IAuditHandler<T>
    {
        Task<AuditDetail> HandleAsync(AuditContext<T> context);
    }

    public class AuditContext<T>
    {
        public T Request { get; internal set; }
        public AuditContext(T request)
        {
            this.Request = request;
        }
    }
}