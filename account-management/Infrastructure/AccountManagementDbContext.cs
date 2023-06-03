using Microsoft.EntityFrameworkCore;
using PlatformPlatform.AccountManagement.Domain.Tenants;
using PlatformPlatform.AccountManagement.Domain.Users;
using PlatformPlatform.SharedKernel.InfrastructureCore.EntityFramework;

namespace PlatformPlatform.AccountManagement.Infrastructure;

public sealed class AccountManagementDbContext : SharedKernelDbContext<AccountManagementDbContext>
{
    public AccountManagementDbContext(DbContextOptions<AccountManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tenant
        modelBuilder.Entity<Tenant>().Property(x => x.Id).HasConversion<TenantId.EfCoreValueConverter>();

        // User
        modelBuilder.Entity<User>().Property(x => x.Id).HasConversion<UserId.EfCoreValueConverter>();
        modelBuilder.Entity<User>().HasOne<Tenant>().WithMany().HasForeignKey(u => u.TenantId)
            .HasPrincipalKey(t => t.Id);
    }
}