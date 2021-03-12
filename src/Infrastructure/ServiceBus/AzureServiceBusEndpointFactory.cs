using System.Collections.Concurrent;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using CleanFunc.Application.Common.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CleanFunc.Infrastructure.ServiceBus
{
    public class AzureServiceBusEndpointFactory : IBusEndpointFactory
    {
        private IServiceBusConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, Lazy<IBusEndpoint>> _endpointCache = new ConcurrentDictionary<string, Lazy<IBusEndpoint>>();

        public AzureServiceBusEndpointFactory(IServiceBusConfiguration configuration, IServiceProvider serviceProvider)
        {
            Guard.Against.Null(configuration, nameof(configuration));
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create Sender using supplied queue or topic name.
        /// </summary>
        /// <param name="queueOrTopicName"></param>
        /// <returns></returns>
        public IBusEndpoint Create(string queueOrTopicName)
        {
            Guard.Against.NullOrEmpty(queueOrTopicName, nameof(queueOrTopicName));

            var enrichers = _serviceProvider.GetService<IEnumerable<IMessageEnricher>>();

            var connectionString = GetConnectionString(queueOrTopicName);

            string cacheKey = $"{queueOrTopicName}-{connectionString}";

            // Cache endpoint so we do not run out of available connections
            // Use of Lazy here is to guarantee value factory is only executed once as it is not idempotent
            var endpoint = _endpointCache.GetOrAdd(cacheKey, new Lazy<IBusEndpoint>(()
                                                                        => new AzureServiceBusEndpoint(enrichers,
                                                                                connectionString, 
                                                                                queueOrTopicName)));
            return endpoint.Value;
        }

        /// <summary>
        /// Create Sender using convention based method based on TPayload 
        /// </summary>
        /// <param name="entityType"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public IBusEndpoint Create<TPayload>() where TPayload: class
        {
            var enrichers = _serviceProvider.GetService<IEnumerable<IMessageEnricher>>();

            var connectionString = GetConnectionString(typeof(TPayload).Name);

            var entityPath = typeof(TPayload).FullName.ToLowerInvariant();

            string cacheKey = $"{entityPath}-{connectionString}";

            // Cache endpoint so we do not run out of available connections
            // Use of Lazy here is to guarantee value factory is only executed once as it is not idempotent
            var endpoint = _endpointCache.GetOrAdd(cacheKey, new Lazy<IBusEndpoint>(() 
                                                                            => new AzureServiceBusEndpoint(enrichers, 
                                                                                connectionString, 
                                                                                entityPath)));
            return endpoint.Value;
        }

        private string GetConnectionString(string suffix)
        {
            // lookup connectionstring by suffix. If its not found, use default connectionstring
            return _configuration.OtherConnectionStrings.GetValueOrDefault(suffix.ToLowerInvariant(), _configuration.DefaultConnectionString);
        }
    }
}