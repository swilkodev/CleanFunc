using System.Diagnostics.CodeAnalysis;
using System;
using CleanFunc.Application.Common.Interfaces;
using System.Collections.Generic;

namespace CleanFunc.Infrastructure.Context
{
    [ExcludeFromCodeCoverageAttribute]
    public class MutableCallContext : ICallContext
    {
        public Guid CorrelationId {get; set;}
        public string UserName { get; set; }
        public string AuthenticationType {get;set;}
        public string FunctionName { get; set; }
        public IDictionary<string, string> AdditionalProperties { get; } = new Dictionary<string, string>();
    }
}