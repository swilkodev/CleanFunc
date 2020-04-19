using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IBus 
    {
        Task SendAsync<TPayload>(TPayload payload) where TPayload: class;
    }
}