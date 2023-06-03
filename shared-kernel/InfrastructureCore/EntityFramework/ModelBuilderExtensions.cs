using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PlatformPlatform.SharedKernel.InfrastructureCore.EntityFramework;

public static class ModelBuilderExtensions
{
    /// <summary>
    ///     This method is used to tell Entity Framework to store all enum properties as strings in the database.
    /// </summary>
    [UsedImplicitly]
    public static ModelBuilder UseStringForEnums(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (!property.ClrType.IsEnum) continue;

                var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                var converterInstance = (ValueConverter) Activator.CreateInstance(converterType)!;
                property.SetValueConverter(converterInstance);
            }
        }

        return modelBuilder;
    }
}