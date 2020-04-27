using System.Linq;
using System.Collections;
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
using FluentAssertions;
using Xunit;
using CleanFunc.Application.Issuers.Models;

namespace CleanFunc.Application.UnitTests.Issuers.Queries.ExportIssuer
{
    [Collection("QueryTests")]
    public class ExportIssuerQuerysTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public ExportIssuerQuerysTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task ExportIssuers_EnsureFileBuilderIsCalledAndCorrectResultIsReturned()
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
            
            result.Should().BeOfType<ExportIssuersResponse>();
            result.Should().NotBeNull();
            result.ContentType.Should().Be("text/csv");
            result.Content.Should().BeSameAs(fileData);
            result.FileName.Should().Be("Issuers.csv");
        }

        [Fact]
        public async Task ExportIssuers_EnsureAllRecordsGivenToFileBuilderMatchDatabase()
        {
            IEnumerable<IssuerRecord> records = Enumerable.Empty<IssuerRecord>();
            var logger = new Mock<ILogger<ExportIssuersQuery.ExportIssuersQueryHandler>>();
            var fileBuilder = new Mock<ICsvFileBuilder>();
            fileBuilder.Setup(_ => _.BuildFileAsync(It.IsAny<IEnumerable<IssuerRecord>>()))
                        .Callback<IEnumerable<IssuerRecord>>(r => records = r);

            var query = new ExportIssuersQuery();
            
            var sut = new ExportIssuersQuery.ExportIssuersQueryHandler(_context, _mapper, fileBuilder.Object, logger.Object);
            // act
            await sut.Handle(query, CancellationToken.None);

            records.Should().NotBeEmpty();
            records.Count().Should().Be(_context.Issuers.Count());
        }

        [Fact]
        public async Task ExportIssuers_EnsureExactRecordsGivenToFileBuilderMatchesDatabase()
        {
            IEnumerable<IssuerRecord> records = Enumerable.Empty<IssuerRecord>();
            var logger = new Mock<ILogger<ExportIssuersQuery.ExportIssuersQueryHandler>>();
            var fileBuilder = new Mock<ICsvFileBuilder>();
            fileBuilder.Setup(_ => _.BuildFileAsync(It.IsAny<IEnumerable<IssuerRecord>>()))
                        .Callback<IEnumerable<IssuerRecord>>(r => records = r);

            var query = new ExportIssuersQuery();
            query.Id = new Guid("de891235-405e-4e72-912d-7bd51b4c92b7");
            var sut = new ExportIssuersQuery.ExportIssuersQueryHandler(_context, _mapper, fileBuilder.Object, logger.Object);
            // act
            await sut.Handle(query, CancellationToken.None);

            var databaseRecords = _context.Issuers.Where(_ => _.Id == query.Id);
            
            records.Should().NotBeEmpty();
            records.Count().Should().Be(databaseRecords.Count());
            records.First().Name.Should().Be(databaseRecords.First().Name);
            records.First().CreatedDate.Should().Be(databaseRecords.First().CreatedDate);
        }
    }
}