using System.Text;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Issuers.Queries.ExportIssuers;
using CleanFunc.Application.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;
using CleanFunc.Application.Issuers.Models;

namespace Application.UnitTests.Issuers.Queries.ExportIssuer
{
    [Collection("QueryTests")]
    public class ExportIssuerQuerysTests
    {
        private readonly IIssuerRepository _context;
        private readonly IMapper _mapper;
        
        public ExportIssuerQuerysTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task ExportIssuers_ReturnsCorrectResponseAndData()
        {
            var logger = new Mock<ILogger<ExportIssuersQuery.ExportIssuersQueryHandler>>();
            var fileBuilder = new Mock<ICsvFileBuilder>();

            byte[] fileData = Encoding.UTF8.GetBytes("This is the file data");
            fileBuilder.Setup(_ => _.BuildFileAsync(It.IsAny<IEnumerable<IssuerRecord>>())).ReturnsAsync(fileData);

            var query = new ExportIssuersQuery();
            var sut = new ExportIssuersQuery.ExportIssuersQueryHandler(_context, _mapper, fileBuilder.Object, logger.Object);
            // act
            var result = await sut.Handle(query, CancellationToken.None);

            // assert
            fileBuilder.Verify();
            
            result.ShouldBeOfType<ExportIssuersResponse>();
            result.ShouldNotBeNull();
            result.ContentType.ShouldBe("text/csv");
            result.Content.ShouldBe(fileData);
            result.FileName.ShouldBe("Issuers.csv");
        }
    }
}