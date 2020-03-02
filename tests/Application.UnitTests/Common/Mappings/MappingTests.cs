using AutoMapper;
using CleanFunc.Application.Issuers.Models;
using CleanFunc.Application.Issuers.Queries.GetIssuer;
using CleanFunc.Domain.Entities;
using System;
using Xunit;

namespace CleanArch.Application.UnitTests.Common.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Theory]
        [InlineData(typeof(Issuer), typeof(IssuerDto))]
        [InlineData(typeof(Issuer), typeof(IssuerRecord))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
