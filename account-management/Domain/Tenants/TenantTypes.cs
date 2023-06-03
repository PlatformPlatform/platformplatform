using JetBrains.Annotations;
using PlatformPlatform.SharedKernel.DomainCore.Identity;
using StronglyTypedIds;

namespace PlatformPlatform.AccountManagement.Domain.Tenants;

[StronglyTypedId(StronglyTypedIdBackingType.Long, StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct TenantId
{
    public static TenantId NewId()
    {
        return new TenantId(IdGenerator.NewId());
    }

    public static explicit operator TenantId(string value)
    {
        return new TenantId(Convert.ToInt64(value));
    }
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum TenantState
{
    Trial,
    Active,
    Suspended
}