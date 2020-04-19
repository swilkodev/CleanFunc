using System;
using CleanFunc.Application.Issuers.Queries.GetIssuer;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Issuers.Queries.GetIssuer
{
    public class GetIssuerQueryValidatorTests
    {
        [Fact]
        public void WhenIdIsProvided_IsValidShouldBeTrue()
        {
            var query = new GetIssuerQuery();
            query.Id = new Guid("5f95d690-513a-497f-bba2-76bc286bf2af");

            var sut = new GetIssuerQueryValidator();

            var result = sut.Validate(query);

            result.IsValid.Should().Be(true);
        }

        [Fact]
        public void WhenIdIsEmpty_IsValidShouldBeFalse()
        {
            var query = new GetIssuerQuery();
            query.Id = Guid.Empty;

            var sut = new GetIssuerQueryValidator();

            var result = sut.Validate(query);

            result.IsValid.Should().Be(false);
        }
    }
}