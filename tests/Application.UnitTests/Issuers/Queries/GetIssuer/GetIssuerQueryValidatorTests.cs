using System;
using CleanFunc.Application.Issuers.Queries.GetIssuer;
using Shouldly;
using Xunit;

namespace Application.UnitTests.Issuers.Queries.GetIssuer
{
    public class GetIssuerQueryValidatorTests
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenIdIsProvided()
        {
            var query = new GetIssuerQuery();
            query.Id = new Guid("5f95d690-513a-497f-bba2-76bc286bf2af");

            var sut = new GetIssuerQueryValidator();

            var result = sut.Validate(query);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenIdIsEmpty()
        {
            var query = new GetIssuerQuery();
            query.Id = Guid.Empty;

            var sut = new GetIssuerQueryValidator();

            var result = sut.Validate(query);

            result.IsValid.ShouldBe(false);
        }
    }
}