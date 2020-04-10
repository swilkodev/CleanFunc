using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Mappings;
using CleanFunc.Domain.Entities;
using CleanFunc.Infrastructure.Persistence;
using Moq;
using Xunit;

namespace CleanFunc.Application.UnitTests.Common
{
    public sealed class QueryTestFixture : IDisposable
    {
        public QueryTestFixture()
        {
            var dateTimeService = new Mock<IDateTime>();
            dateTimeService.Setup(_ => _.Now).Returns(new DateTime(3000, 1, 1));
            
            Context = new IssuerRepository(dateTimeService.Object); // ApplicationDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        // public TRepository GetRepository<TRepository>()
        // {
            
        // }

        public IIssuerRepository Context { get; }

        public IMapper Mapper { get; }

        public void Dispose()
        {
        //     ApplicationDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryTests")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }

}