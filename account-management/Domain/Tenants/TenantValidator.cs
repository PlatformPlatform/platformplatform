using FluentValidation;
using JetBrains.Annotations;
using PlatformPlatform.Foundation.DomainModeling.Validation;

namespace PlatformPlatform.AccountManagement.Domain.Tenants;

public interface ITenantValidation
{
    string Name { get; }

    string Email { get; }

    string? Phone { get; }
}

[UsedImplicitly]
public abstract class TenantValidatorBase<T> : AbstractValidator<T> where T : ITenantValidation
{
    protected TenantValidatorBase()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Length(1, 30).When(x => !string.IsNullOrEmpty(x.Name));
        RuleFor(x => x.Email).NotEmpty().SetValidator(new SharedValidations.Email());
        RuleFor(x => x.Phone).SetValidator(new SharedValidations.Phone());
    }
}