using System.Collections.Generic;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EFCore.BulkExtensions;
using System.Reflection;
using System.Threading;

namespace CleanFunc.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTime dateTime;

        public ApplicationDbContext(DbContextOptions options,
                                    IDateTime dateTime) : base(options)
        {
            this.dateTime = dateTime;
        }

        public DbSet<Issuer> Issuers{ get; set;}

        public async Task BulkInsertAsync<T>(IList<T> entities, CancellationToken cancellationToken = default) where T: class
        {
            // cosmos db and inmemory does not allow bulk insert
            if(this.Database.IsCosmos() || this.Database.IsInMemory())
            {
                Set<T>().AddRange(entities);
                
                await SaveChangesAsync(cancellationToken);
            }
            else
            {
                await DbContextBulkExtensions.BulkInsertAsync<T>(this, entities);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            if(this.Database.IsCosmos())
            {
                // CosmosDb specifics
                builder.Entity<Issuer>()
                    .ToContainer("Issuer");

                builder.Entity<Issuer>()
                    .HasPartitionKey(o => o.Name);
                
            }

            base.OnModelCreating(builder);
        }
    }
}