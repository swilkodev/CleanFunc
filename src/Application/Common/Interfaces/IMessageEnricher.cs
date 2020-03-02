using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    //TODO: Do these interfaces belong in the application layer as they are not used here.
    // They are only used in infrastructure.
    public interface IMessageEnricher
    {
        public Task EnrichAsync(MessageContext context);
    }
    
    
    public class MessageContext
    {
        private Dictionary<string, object> _userProperties = new Dictionary<string, object>();

        public Dictionary<string, object> UserProperties => _userProperties;
    }
}