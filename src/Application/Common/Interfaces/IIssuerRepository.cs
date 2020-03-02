using System;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IIssuerRepository : IAsyncRepository<Issuer, Guid> {}
}