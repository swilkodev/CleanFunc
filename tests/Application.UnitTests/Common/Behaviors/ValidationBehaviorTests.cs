using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTests.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        private Mock<RequestHandlerDelegate<Guid>> mockHandler;
        private readonly List<IValidator<SomeQuery>> validators;

        public ValidationBehaviorTests()
        {
            validators = new List<IValidator<SomeQuery>>()
            {
                new SomeQueryValidator()
            };
            mockHandler = new Mock<RequestHandlerDelegate<Guid>>();
            mockHandler.Setup(_ => _()).ReturnsAsync(Guid.NewGuid());
        }

         [Fact]
        public async Task WhenNoValidatorsPresent_BehaviorShouldCallHandlerAndNotReturnAnError()
        {
            var sut = new RequestValidationBehavior<SomeQuery, Guid>(Enumerable.Empty<IValidator<SomeQuery>>());
            var query = new SomeQuery()
            {
                Name = ""
            };

            var exception = await Record.ExceptionAsync(() =>
                                     sut.Handle(
                                            query, 
                                            new CancellationToken(),
                                            mockHandler.Object));

            exception.ShouldBeNull();
            mockHandler.Verify(_ => _(), Times.Once);
        }

        [Fact]
        public async Task WhenNameIsNotBlank_BehaviorShouldNotReturnAnError()
        {
            var sut = new RequestValidationBehavior<SomeQuery, Guid>(validators);
            var query = new SomeQuery()
            {
                Name = "Scott"
            };

            var exception = await Record.ExceptionAsync(() =>
                                     sut.Handle(
                                            query, 
                                            new CancellationToken(),
                                            mockHandler.Object));

            exception.ShouldBeNull();
            mockHandler.Verify(_ => _(), Times.Once);
        }

        [Fact]
        public async Task NameIsBlank_BehaviorShouldReturnAnErrorTypeOfValidationException()
        {
            var sut = new RequestValidationBehavior<SomeQuery, Guid>(validators);
            var query = new SomeQuery()
            {
                Name = ""
            };

            var exception = await Record.ExceptionAsync(() =>
                                     sut.Handle(
                                            query, 
                                            new CancellationToken(),
                                            mockHandler.Object));


            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CleanFunc.Application.Common.Exceptions.ValidationException>();
            (exception as CleanFunc.Application.Common.Exceptions.ValidationException).Errors.Count().ShouldBe(1);
            mockHandler.Verify(_ => _(), Times.Never);
        }

        private class SomeQuery : IRequest<Guid> 
        {
            public string Name {get;set;}
        }

        private class SomeQueryValidator : AbstractValidator<SomeQuery>
        {
            public SomeQueryValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }

    }
}