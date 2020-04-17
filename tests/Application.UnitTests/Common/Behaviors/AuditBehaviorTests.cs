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
        private Mock<IAuditor> mockAuditor;
        private Mock<RequestHandlerDelegate<Guid>> mockSuccessHandler;
        private Mock<RequestHandlerDelegate<Guid>> mockFailHandler;
        private static readonly AuditEntry auditEntry = new AuditEntry("Some", "Create");

        public AuditBehaviorTests()
        {
            mockAuditor = new Mock<IAuditor>();

            mockSuccessHandler = new Mock<RequestHandlerDelegate<Guid>>();
            mockSuccessHandler.Setup(_ => _()).ReturnsAsync(Guid.NewGuid());

            mockFailHandler = new Mock<RequestHandlerDelegate<Guid>>();
            mockFailHandler.Setup(_ => _()).ThrowsAsync(new ArgumentException(ErrorMessage));
        }

        [Fact]
        public async Task WhenAuditableRequest_ShouldCreateAuditWithSuccessOutcome()
        {
            var sut = new RequestAuditBehavior<SomeAuditableCommand, Guid>(mockAuditor.Object);
            
            var result = await sut.Handle(
                                            new SomeAuditableCommand(), 
                                            new CancellationToken(),
                                            mockSuccessHandler.Object);

            mockSuccessHandler.Verify(_ => _(), Times.Once);
            mockAuditor.Verify(x => x.AddAsync(It.Is<Audit>(a => a.Outcome == AuditOutcome.Success
                                                                && a.Entry == auditEntry)), Times.Once);
        }

        [Fact]
        public async Task WhenAuditableRequestAndErrorOccurs_ShouldCreateAuditWithFailureOutcome()
        {            
            var sut = new RequestAuditBehavior<SomeAuditableCommand, Guid>(mockAuditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeAuditableCommand(), 
                                                        new CancellationToken(),
                                                        mockFailHandler.Object)
            );

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(ErrorMessage);

            mockAuditor.Verify(x => x.AddAsync(It.Is<Audit>(a => a.Outcome == AuditOutcome.Failure 
                                                                && a.Reason == ErrorMessage
                                                                && a.Entry == auditEntry)), Times.Once);

            mockFailHandler.Verify(_ => _(), Times.Once);
        }

        [Fact]
        public async Task WhenNotAnAuditableRequestAndNoErrorOccured_ShouldNotCreateAudit()
        {
            var sut = new RequestAuditBehavior<SomeQuery, Guid>(mockAuditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeQuery() , 
                                                        new CancellationToken(),
                                                        mockSuccessHandler.Object)
            );

            exception.ShouldBeNull();
            mockAuditor.Verify(x => x.AddAsync(It.IsAny<Audit>()), Times.Never);
            mockSuccessHandler.Verify(_ => _(), Times.Once);
        }


        [Fact]
        public async Task WhenNotAnAuditableRequestAndErrorOccureds_ShouldReturnErrorButNotCreateAudit()
        {
            var sut = new RequestAuditBehavior<SomeQuery, Guid>(mockAuditor.Object);
            
            var exception = await Record.ExceptionAsync(() => 
                                             sut.Handle(
                                                        new SomeQuery() , 
                                                        new CancellationToken(),
                                                        mockFailHandler.Object)
            );

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(ErrorMessage);

            mockAuditor.Verify(x => x.AddAsync(It.IsAny<Audit>()), Times.Never);
            mockFailHandler.Verify(_ => _(), Times.Once);
        }

        private class SomeQuery : IRequest<Guid> {}

        private class SomeAuditableCommand : IRequest<Guid>, IAuditableRequest<SomeAuditableCommand>
        {
            public Task<AuditEntry> CreateEntryAsync(SomeAuditableCommand request)
            {
                return Task.FromResult(auditEntry);
            }
        }
    }


}