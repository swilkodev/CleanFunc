using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace CleanFunc.Infrastructure.ServiceBus
{
    public interface IMessageEnricher
    {
        Task EnrichAsync(Message message);
    }
}