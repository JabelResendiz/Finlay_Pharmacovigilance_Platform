namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEncryptionService
{
    string Encrypt(string value);

    string Decrypt(string value);

    string CreateBlindIndex(string value);
}