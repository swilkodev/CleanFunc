using System.Threading.Tasks;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IAuditor
    {
         Task AddAsync(AuditRecord record);
    }
}