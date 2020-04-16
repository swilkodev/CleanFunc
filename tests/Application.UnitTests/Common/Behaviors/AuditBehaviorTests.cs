using System;
using System.Threading;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Behaviours;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Common.Models;
using CleanFunc.Application.Issuers.Commands.CreateIssuer;
using MediatR;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTests.Common.Behaviors
{
    public class AuditBehaviorTests
    {
        private const string ErrorMessage = "An error occured";
        private Mock<IAuditor> auditor;
        private RequestHandlerDelegate<Guid> successHandler;
        private RequestHandlerDelegate<Guid> failhandler;
        private static readonly AuditEntry auditEntry = new AuditEntry("Some", "Create");

        public AuditBehaviorTests()
        {
            auditor = new Mock<IAuditor>();

            successHandler = () => Task.FromResult(Guid.NewGuid());

            failhandler = () => throw new Exception(ErrorMessage);
        }

        [Fact]
        public async Task ShouldCreateAuditWithSuccessOutcome()
        {
            var sut = new RequestAuditBehavior<SomeCommand, Guid>(auditor.Object);
            
            var result = await sut.Handle(
                                            new SomeCommand(), 
                                            new CancellationToken(),
                                            successHandler);

            auditor.Verify(x => x.AddAsync(It.Is<Audit>(a => a.Outcome == AuditOutcome.Success
                                                                && a.Entry == auditEntry)), Times.Once);
        }

        [Fact]
        public async Task ShouldCreateAuditWithFailureOutcome()
        {            
            var sut = new RequestAuditBehavior<SomeCommand, Guid>(auditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeCommand(), 
                                                        new CancellationToken(),
                                                        failhandler)
            );

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(ErrorMessage);

            auditor.Verify(x => x.AddAsync(It.Is<Audit>(a => a.Outcome == AuditOutcome.Failure 
                                                                && a.Reason == ErrorMessage
                                                                && a.Entry == auditEntry)), Times.Once);
        }

        [Fact]
        public async Task ShouldNotCreateAnAudit_AsDoesNotImplementAuditableRequest_ShouldNotThrowException()
        {
            var sut = new RequestAuditBehavior<SomeQuery, Guid>(auditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeQuery() , 
                                                        new CancellationToken(),
                                                        successHandler)
            );

            exception.ShouldBeNull();
            auditor.Verify(x => x.AddAsync(It.IsAny<Audit>()), Times.Never);
        }


        [Fact]
        public async Task ShouldNotCreateAnAudit_AsDoesNotImplementAuditableRequest_MustThrowException()
        {
            var sut = new RequestAuditBehavior<SomeQuery, Guid>(auditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeQuery() , 
                                                        new CancellationToken(),
                                                        failhandler)
            );

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(ErrorMessage);

            auditor.Verify(x => x.AddAsync(It.IsAny<Audit>()), Times.Never);
        }

        private class SomeQuery : IRequest<Guid> {}

        private class SomeCommand : IRequest<Guid>, IAuditableRequest<SomeCommand>
        {
            public Task<AuditEntry> CreateEntryAsync(SomeCommand request)
            {
                return Task.FromResult(auditEntry);
            }
        }
    }


}