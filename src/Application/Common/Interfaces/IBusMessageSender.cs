using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IBusMessageSender 
    {
        Task SendAsync<TPayload>(TPayload payload) where TPayload: class;
    }
}