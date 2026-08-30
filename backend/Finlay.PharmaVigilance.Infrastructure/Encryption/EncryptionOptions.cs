namespace Finlay.PharmaVigilance.Infrastructure.Encryption;

public class EncryptionOptions
{
    public string AesKey { get; set; } = null!;

    public string BlindIndexKey { get; set; } = null!;
}