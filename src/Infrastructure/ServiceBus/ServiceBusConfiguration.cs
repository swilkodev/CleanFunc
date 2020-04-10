using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

[assembly: InternalsVisibleTo("CleanArch.Infrastructure.IntegrationTests")]
namespace CleanFunc.Infrastructure.ServiceBus
{
    /// <summary>
    /// Loads the service bus configuration. This class supports a convention where you can define default settings and
    /// specific settings per queue/topic.
    /// Examples are:
    ///     "ServiceBusConnectionString": "xxx"
    ///     "ServiceBusConnectionString:myqueue": "yyy"
    ///     "ServiceBusConnectionString:mytopic": "zzz"
    /// </summary>
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        private readonly IConfiguration _configuration;

        public ServiceBusConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            DefaultConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        }

        public string DefaultConnectionString { get; }

        private Dictionary<string, string> _OtherConnectionStrings;
        public Dictionary<string, string> OtherConnectionStrings
        {
            get
            {
                if (_OtherConnectionStrings == null)
                {
                    _OtherConnectionStrings = new Dictionary<string, string>();

                    var otherConnections = from configItem in Configurations(_configuration)
                                           where configItem.Key.StartsWith("ServiceBusConnectionString:")
                                           select new { Key = configItem.Key.Remove(0, "ServiceBusConnectionString:".Length), Value = configItem.Value };

                    foreach (var connectionConfig in otherConnections)
                    {
                        if (!OtherConnectionStrings.TryAdd(connectionConfig.Key, connectionConfig.Value))
                        {
                            throw new Exception($"Failed to load servicebus connectionstring with name {connectionConfig.Key}. Make sure there are not duplicate settings.");
                        }
                    }
                }
                return _OtherConnectionStrings;
            }
        }

        /// <summary>
        /// Required for unit testing due to IConfiguration.AsEnumerable being an extension method.
        /// </summary>
        private Func<IConfiguration, IEnumerable<KeyValuePair<string, string>>> _Configurations;
        internal Func<IConfiguration, IEnumerable<KeyValuePair<string, string>>> Configurations
        {
            get
            {
                return _Configurations ?? (_Configurations = (config) => { return config.AsEnumerable(); });
            }
            set
            {
                _Configurations = value;
            }
        }

    }
}