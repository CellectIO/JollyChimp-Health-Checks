using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JollyChimp.Core.Common.Encryption;

public class EncryptionConvertor : ValueConverter<string, string>
{
    public EncryptionConvertor(ConverterMappingHints mappingHints = null)
        : base(x => EncryptionExtension.Encrypt(x), x => EncryptionExtension.Decrypt(x), mappingHints)
    { }
}