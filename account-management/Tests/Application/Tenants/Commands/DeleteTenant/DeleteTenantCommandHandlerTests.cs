using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PlatformPlatform.AccountManagement.Application;
using PlatformPlatform.AccountManagement.Application.Tenants.Commands.DeleteTenant;
using PlatformPlatform.AccountManagement.Domain.Tenants;
using Xunit;

namespace PlatformPlatform.AccountManagement.Tests.Application.Tenants.Commands.DeleteTenant;

public class DeleteTenantCommandHandlerTests
{
    private readonly ITenantRepository _tenantRepository;

    public DeleteTenantCommandHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddApplicationServices();

        _tenantRepository = Substitute.For<ITenantRepository>();
    }

    [Fact]
    public async Task DeleteTenantCommandHandler_WhenTenantExists_ShouldDeleteTenantFromRepository()
    {
        // Arrange
        var existingTenant = Tenant.Create("ExistingTenant", "tenant1", "foo@tenant1.com", "1234567890");
        var existingTenantId = existingTenant.Id;
        _tenantRepository.GetByIdAsync(existingTenantId, Arg.Any<CancellationToken>()).Returns(existingTenant);
        var handler = new DeleteTenantCommandHandler(_tenantRepository);

        // Act
        var command = new DeleteTenantCommand(existingTenantId);
        var deleteTenantCommandResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        deleteTenantCommandResult.IsSuccess.Should().BeTrue();
        _tenantRepository.Received().Remove(existingTenant);
    }

    [Fact]
    public async Task DeleteTenantCommandHandler_WhenTenantDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var nonExistingTenantId = TenantId.NewId();
        _tenantRepository.GetByIdAsync(nonExistingTenantId, Arg.Any<CancellationToken>()).Returns(null as Tenant);
        var handler = new DeleteTenantCommandHandler(_tenantRepository);

        // Act
        var command = new DeleteTenantCommand(nonExistingTenantId);
        var deleteTenantCommandResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        deleteTenantCommandResult.IsSuccess.Should().BeFalse();
    }
}