using System;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}