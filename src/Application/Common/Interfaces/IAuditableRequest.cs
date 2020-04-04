using System.Threading.Tasks;


namespace CleanFunc.Application.Common.Interfaces
{
    public interface IAuditableRequest<TRequest>
    {
        Task<Models.AuditEntry> CreateEntryAsync(TRequest request);
    }
}