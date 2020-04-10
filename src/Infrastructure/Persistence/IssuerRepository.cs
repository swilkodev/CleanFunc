using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Infrastructure.Persistence
{
    public class IssuerRepository : IIssuerRepository
    {
        private readonly IDateTime dateTime;

        public IssuerRepository(IDateTime dateTime)
        {
            this.dateTime = dateTime;
        }
        
        public Task Add(Issuer entity)
        {
            return Task.CompletedTask;
        }

        public Task Add(IEnumerable<Issuer> records)
        {
            // TODO Bulk Insert
            return Task.CompletedTask;
        }

        public Task<int> CountAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountWhere(Expression<Func<Issuer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Issuer> FirstOrDefault(Expression<Func<Issuer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Issuer>> GetAll()
        {
            var issuers = new List<Issuer>();
            issuers.Add(new Issuer { Id=new Guid("5f95d690-513a-497f-bba2-76bc286bf2af"), Name="SAW Beer Pty Ltd", CreatedDate=dateTime.Now});
            issuers.Add(new Issuer { Id=new Guid("de891235-405e-4e72-912d-7bd51b4c92b7"), Name="Microsoft Corporation", CreatedDate=dateTime.Now.AddDays(- 1)});
            issuers.Add(new Issuer { Id=new Guid("9e0dd491-cc18-4fe1-8f13-41a7e7270aef"), Name="Amazon.com, Inc.", CreatedDate=dateTime.Now.AddHours(-12)});
            return Task.FromResult(issuers.AsEnumerable());
        }

        public async Task<Issuer> GetById(Guid id)
        {
            // emulate database call
            return (await GetAll()).FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Issuer>> GetWhere(Expression<Func<Issuer, bool>> predicate)
        {
            // emulate database call
            return (await GetAll()).Where(predicate.Compile());
        }

        public Task Remove(Issuer entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(Issuer entity)
        {
            throw new NotImplementedException();
        }
    }
}