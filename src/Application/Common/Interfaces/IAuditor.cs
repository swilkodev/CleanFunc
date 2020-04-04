using System.Threading.Tasks;
using CleanFunc.Application.Common.Models;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IAuditor
    {
         Task AddAsync(Models.Audit record);
    }
}