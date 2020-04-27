using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Issuers.Queries.GetIssuer;
using CleanFunc.Application.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace CleanFunc.Application.UnitTests.Issuers.Queries.GetIssuer
{
    [Collection("QueryTests")]
    public class GetIssuerQueryTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public GetIssuerQueryTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_ReturnsCorrectResponseAndListCount()
        {
            var query = new GetIssuerQuery();
            query.Id = new Guid("5f95d690-513a-497f-bba2-76bc286bf2af");

            var sut = new GetIssuerQuery.GetIssuerQueryHandler(_context, _mapper);

            var result = await sut.Handle(query, CancellationToken.None);

            result.Should().BeOfType<GetIssuerResponse>();
            
            result.Issuer.Should().NotBeNull();
            result.Issuer.Name.Should().Be("SAW Beer Pty Ltd");
        }
    }
}