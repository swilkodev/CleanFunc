using System;
using System.Collections.Generic;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface ICallContext
    {
        Guid CorrelationId
        {
            get;
            set;
        }

        string FunctionName
        {
            get;
            set;
        }

        string UserName
        {
            get;
            set;
        }

        string AuthenticationType
        {
            get;
            set;
        }

        IDictionary<string, string> AdditionalProperties
        {
            get;
        }
    }
}