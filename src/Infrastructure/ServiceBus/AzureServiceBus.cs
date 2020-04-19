using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;

namespace CleanFunc.Infrastructure.ServiceBus
{
    internal class AzureServiceBus : IBus, IAsyncDisposable
    {
        private readonly MessageSender _messageSender;
        private readonly IEnumerable<IMessageEnricher> _enrichers;

        public AzureServiceBus(IEnumerable<IMessageEnricher> enrichers, string connectionString, string queueOrTopicName)
        {
            Guard.Against.Null(enrichers, nameof(enrichers));
            Guard.Against.NullOrEmpty(connectionString, nameof(connectionString));
            Guard.Against.NullOrEmpty(queueOrTopicName, nameof(queueOrTopicName));

            _messageSender = new MessageSender(connectionString, queueOrTopicName);
            _enrichers = enrichers;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(_messageSender.CloseAsync());
        }

        public async Task SendAsync<TPayload>(TPayload payload)  where TPayload: class
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));
            message.ContentType = "application/json";

            // run each of the enrichers 
            foreach(var enricher in _enrichers)
                await enricher.EnrichAsync(message);

            await _messageSender.SendAsync(message);    
        }   
    }

}