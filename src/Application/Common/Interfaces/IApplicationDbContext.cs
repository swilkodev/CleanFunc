using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanFunc.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Issuer> Issuers {get;set;}

        Task BulkInsertAsync<T>(IList<T> entities, CancellationToken cancellationToken = default) where T: class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}