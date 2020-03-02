using System.Diagnostics.CodeAnalysis;
using System;
using CleanFunc.Application.Common.Interfaces;

namespace CleanFunc.Infrastructure.Context
{
    [ExcludeFromCodeCoverageAttribute]
    public class MutableCallContextProvider : ICallContextProvider
    {
        public Guid CorrelationId {get; set;}
        public string UserName { get; set; }
        public string UserType {get;set;}
    }
}