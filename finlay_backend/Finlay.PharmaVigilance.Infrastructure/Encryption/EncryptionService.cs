using System.Security.Cryptography;
using System.Text;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.Encryption;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _aesKey;
    private readonly byte[] _blindIndexKey;

    public EncryptionService(
        IOptions<EncryptionOptions> options)
    {
        _aesKey =
            Convert.FromBase64String(
                options.Value.AesKey);

        _blindIndexKey =
            Convert.FromBase64String(
                options.Value.BlindIndexKey);
    }

    public string Encrypt(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        using var aes = Aes.Create();

        aes.KeySize = 256;
        aes.Key = _aesKey;

        aes.GenerateIV();

        using var encryptor =
            aes.CreateEncryptor(
                aes.Key,
                aes.IV);

        using var memoryStream =
            new MemoryStream();

        // Guardamos el IV al inicio
        memoryStream.Write(
            aes.IV,
            0,
            aes.IV.Length);

        using (var cryptoStream =
            new CryptoStream(
                memoryStream,
                encryptor,
                CryptoStreamMode.Write))
        using (var writer =
            new StreamWriter(cryptoStream))
        {
            writer.Write(value);
        }

        return Convert.ToBase64String(
            memoryStream.ToArray());
    }

    public string Decrypt(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var fullCipher =
            Convert.FromBase64String(value);

        using var aes = Aes.Create();

        aes.KeySize = 256;
        aes.Key = _aesKey;

        var iv = new byte[16];

        Array.Copy(
            fullCipher,
            0,
            iv,
            0,
            iv.Length);

        aes.IV = iv;

        using var decryptor =
            aes.CreateDecryptor(
                aes.Key,
                aes.IV);

        using var memoryStream =
            new MemoryStream(
                fullCipher,
                16,
                fullCipher.Length - 16);

        using var cryptoStream =
            new CryptoStream(
                memoryStream,
                decryptor,
                CryptoStreamMode.Read);

        using var reader =
            new StreamReader(
                cryptoStream);

        return reader.ReadToEnd();
    }

    public string CreateBlindIndex(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        using var hmac =
            new HMACSHA256(
                _blindIndexKey);

        var hash =
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(value));

        return Convert.ToHexString(hash);
    }
}