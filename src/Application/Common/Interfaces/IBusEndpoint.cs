using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IBusEndpoint 
    {
        /// <summary>
        /// Send command
        /// </summary>
        /// <typeparam name="TPayload"></typeparam>
        Task SendAsync<TPayload>(TPayload payload) where TPayload: class;
    }
}