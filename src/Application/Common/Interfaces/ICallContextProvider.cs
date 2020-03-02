using System;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface ICallContextProvider
    {
        Guid CorrelationId
        {
            get;
            set;
        }

        string UserName
        {
            get;
            set;
        }

        string UserType 
        {
            get;
            set;
        }
    }
}