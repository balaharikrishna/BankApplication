namespace BankApplicationServices.IServices
{
    public interface IEncryptionService
    {
        byte[] GenerateSalt();
        byte[] HashPassword(string password, byte[] salt);
    }
}