using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanFunc.Infrastructure.ServiceBus
{
    public interface IMessageEnricher
    {
        Task EnrichAsync(MessageContext context);
    }

    public class MessageContext
    {
        private Dictionary<string, object> _userProperties = new Dictionary<string, object>();

        public Dictionary<string, object> UserProperties => _userProperties;
    }
}