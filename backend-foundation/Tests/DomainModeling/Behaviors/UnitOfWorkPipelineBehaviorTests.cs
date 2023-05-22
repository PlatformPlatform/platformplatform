using System.Net;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PlatformPlatform.Foundation.DomainModeling.Behaviors;
using PlatformPlatform.Foundation.DomainModeling.Cqrs;
using PlatformPlatform.Foundation.DomainModeling.Persistence;
using PlatformPlatform.Foundation.Tests.TestEntities;
using Xunit;

namespace PlatformPlatform.Foundation.Tests.DomainModeling.Behaviors;

public class UnitOfWorkPipelineBehaviorTests
{
    private readonly UnitOfWorkPipelineBehavior<TestCommand, CommandResult<TestAggregate>> _behavior;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkPipelineBehaviorTests()
    {
        var services = new ServiceCollection();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        services.AddSingleton(_unitOfWork);
        _behavior = new UnitOfWorkPipelineBehavior<TestCommand, CommandResult<TestAggregate>>(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenSuccessfulCommand_ShouldCallNextAndCommitChanges()
    {
        // Arrange
        var command = new TestCommand();
        var cancellationToken = new CancellationToken();
        var next = Substitute.For<RequestHandlerDelegate<CommandResult<TestAggregate>>>();
        var successfulCommandResult = CommandResult<TestAggregate>.Success(TestAggregate.Create("Foo"));
        next.Invoke().Returns(Task.FromResult(successfulCommandResult));

        // Act
        var _ = await _behavior.Handle(command, next, cancellationToken);

        // Assert
        await _unitOfWork.Received().CommitAsync(cancellationToken);
        Received.InOrder(() =>
        {
            next.Invoke();
            _unitOfWork.CommitAsync(cancellationToken);
        });
    }

    [Fact]
    public async Task Handle_WhenNonSuccessfulCommand_ShouldCallNextButNotCommitChanges()
    {
        // Arrange
        var command = new TestCommand();
        var cancellationToken = new CancellationToken();
        var next = Substitute.For<RequestHandlerDelegate<CommandResult<TestAggregate>>>();
        var successfulCommandResult = CommandResult<TestAggregate>.Failure("Fail", HttpStatusCode.BadRequest);
        next.Invoke().Returns(Task.FromResult(successfulCommandResult));

        // Act
        var _ = await _behavior.Handle(command, next, cancellationToken);

        // Assert
        await _unitOfWork.DidNotReceive().CommitAsync(cancellationToken);
        await next.Received().Invoke();
    }
}