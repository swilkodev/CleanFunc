using System;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Mappings;
using CleanFunc.Application.Issuers.Commands.CreateIssuer;
using AutoMapper;

namespace CleanFunc.Application.Issuers.Messages
{
    public class IssuerChangedEvent
    {
        public IssuerChangedEvent()
        {
            EventId = Guid.NewGuid();
        }

        public Guid EventId {get;}
        public DateTime DateTimeOccuredUtc {get;set;}
        public string IssuerName {get;set;}
    }
}