using System.Collections.Concurrent;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using CleanFunc.Application.Common.Interfaces;
using System;

namespace CleanFunc.Infrastructure.ServiceBus
{

    public class ServiceBusFactory : IBusFactory
    {
        private IServiceBusConfiguration _configuration;
        private readonly IEnumerable<IMessageEnricher> _enrichers;
        private readonly ConcurrentDictionary<string, Lazy<IBusMessageSender>> _messageSenderCache = new ConcurrentDictionary<string, Lazy<IBusMessageSender>>();

        public ServiceBusFactory(IServiceBusConfiguration configuration, IEnumerable<IMessageEnricher> enrichers)
        {
            Guard.Against.Null(configuration, nameof(configuration));
            Guard.Against.Null(enrichers, nameof(enrichers));

            _configuration = configuration;
            _enrichers = enrichers;
        }

        /// <summary>
        /// Create Sender using supplied queue or topic name.
        /// </summary>
        /// <param name="queueOrTopicName"></param>
        /// <returns></returns>
        public IBusMessageSender Create(string queueOrTopicName)
        {
            Guard.Against.NullOrEmpty(queueOrTopicName, nameof(queueOrTopicName));

            var connectionString = GetConnectionString(queueOrTopicName);

            string cacheKey = $"{queueOrTopicName}-{connectionString}";

            // Cache sender so we do not create endless senders and run out of available connections
            // Use of Lazy here is to guarantee value factory is only executed once as it is not idempotent
            var sender = _messageSenderCache.GetOrAdd(cacheKey, new Lazy<IBusMessageSender>(()
                                                                        => new ServiceBusMessageSender(_enrichers,
                                                                                connectionString, 
                                                                                queueOrTopicName)));
            return sender.Value;
        }

        /// <summary>
        /// Create Sender using convention based method based on TPayload 
        /// </summary>
        /// <param name="entityType"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public IBusMessageSender Create<TPayload>() where TPayload: class
        {
            var connectionString = GetConnectionString(typeof(TPayload).Name);

            var entityPath = typeof(TPayload).FullName.ToLowerInvariant();

            string cacheKey = $"{entityPath}-{connectionString}";

            // Cache sender so we do not create endless senders and run out of available connections
            // Use of Lazy here is to guarantee value factory is only executed once as it is not idempotent
            var sender = _messageSenderCache.GetOrAdd(cacheKey, new Lazy<IBusMessageSender>(() 
                                                                            => new ServiceBusMessageSender(_enrichers, 
                                                                                connectionString, 
                                                                                entityPath)));
            return sender.Value;
        }

        private string GetConnectionString(string suffix)
        {
            // lookup connectionstring by suffix. If its not found, use default connectionstring
            return _configuration.OtherConnectionStrings.GetValueOrDefault(suffix.ToLowerInvariant(), _configuration.DefaultConnectionString);
        }
    }
}