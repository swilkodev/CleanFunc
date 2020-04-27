using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Mappings;
using CleanFunc.Domain.Entities;
using CleanFunc.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CleanFunc.Application.UnitTests
{
    public sealed class QueryTestFixture : IDisposable
    {
        public QueryTestFixture()
        {
            Context = ApplicationDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public ApplicationDbContext  Context { get; }

        public IMapper Mapper { get; }

        public void Dispose()
        {    
             ApplicationDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryTests")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }

}