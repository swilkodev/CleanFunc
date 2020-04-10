using System;
using CleanFunc.Application.Common.Interfaces;

namespace CleanFunc.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}