using PlatformPlatform.AccountManagement.Domain.Tenants;

namespace PlatformPlatform.AccountManagement.Application.Tenants.Queries;

public record TenantResponseDto
{
    public required string Id { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime? ModifiedAt { get; init; }

    public required string Name { get; init; }

    public static TenantResponseDto? CreateFrom(Tenant? tenant)
    {
        if (tenant is null) return null;
        return new TenantResponseDto
        {
            Id = tenant.Id.AsRawString()!, CreatedAt = tenant.CreatedAt, ModifiedAt = tenant.ModifiedAt,
            Name = tenant.Name
        };
    }
}