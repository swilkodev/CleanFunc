using System.Collections.Generic;

namespace CleanFunc.Infrastructure.ServiceBus
{
    public interface IServiceBusConfiguration
    {
        public string DefaultConnectionString {get;}
        public Dictionary<string, string> OtherConnectionStrings {get;}
    }
}