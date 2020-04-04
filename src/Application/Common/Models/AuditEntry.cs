using System;
using System.Diagnostics;
using System.Reflection;
using Ardalis.GuardClauses;

namespace CleanFunc.Application.Common.Models
{
    public class AuditEntry
    {
        public AuditEntry(string name, string action)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(action, nameof(action));
            
            Name = name;
            Action = action;
            ExecutingApplication =  new StackFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;
        }

        public string Name {get;}

        public string Action {get; internal set;}

        public DateTime DateOccuredUtc {get;} = DateTime.UtcNow;

        public ActionTarget ActionTarget {get;set;}

        public object CustomData {get;set;}

        public string ExecutingApplication {get; internal set;}
        
    }
}