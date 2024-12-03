using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JollyChimp.Core.Common.Encryption;

public static class ModelPropertyEncrypterExtension
{
    public static void UseEncryption(this ModelBuilder modelBuilder)
    {
        // Instantiate the EncryptionConverter
        var converter = new EncryptionConvertor();
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string) && !IsDiscriminator(property))
                {
                    var attributes = property.PropertyInfo?.GetCustomAttributes(typeof(EncryptPropertyAttribute), false);
                    if (attributes != null && attributes.Any())
                    {
                        property.SetValueConverter(converter);
                    }
                }
            }
        }
    }

    // A helper function to ignore EF Core Discriminator
    private static bool IsDiscriminator(IMutableProperty property)
    {
        return property.Name == "Discriminator" || property.PropertyInfo == null;
    }
}