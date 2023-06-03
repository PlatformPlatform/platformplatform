using JetBrains.Annotations;
using PlatformPlatform.SharedKernel.DomainCore.Identity;
using StronglyTypedIds;

namespace PlatformPlatform.AccountManagement.Domain.Users;

[StronglyTypedId(StronglyTypedIdBackingType.Long, StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct UserId
{
    public static UserId NewId()
    {
        return new UserId(IdGenerator.NewId());
    }

    public static explicit operator UserId(string value)
    {
        return new UserId(Convert.ToInt64(value));
    }
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum UserRole
{
    TenantUser = 0,
    TenantAdmin = 1,
    TenantOwner = 2
}