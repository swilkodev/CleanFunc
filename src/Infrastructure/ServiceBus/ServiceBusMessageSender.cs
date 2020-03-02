using System.Linq;
using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CleanFunc.Application.Common.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;


namespace CleanFunc.Infrastructure.ServiceBus
{
    internal class ServiceBusMessageSender : IBusMessageSender, IAsyncDisposable
    {
        private readonly MessageSender _messageSender;
        private readonly IEnumerable<IMessageEnricher> _enrichers;

        public ServiceBusMessageSender(IEnumerable<IMessageEnricher> enrichers, string connectionString, string queueOrTopicName)
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

            var context= new MessageContext
            {
                // TODO Add further properties
            };

            // run each of the enrichers 
            foreach(var enricher in _enrichers)
                await enricher.EnrichAsync(context);
            
            // add back properties which were added to the ctx above
            // Bit hacky needs more thought
            foreach (var k in context.UserProperties.Keys)
                message.UserProperties.Add(k, context.UserProperties[k]);

            await _messageSender.SendAsync(message);    
        }   
    }

}