using System.Security.Cryptography;
using System.Text;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class EncryptionService : IEncryptionService
    {
        public const int SALT_SIZE = 32;
        public const int HASH_SIZE = 32;
        public const int ITERATIONS = 10000;
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[SALT_SIZE];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS))
            {
                return pbkdf2.GetBytes(HASH_SIZE);
            }
        }
    }
}
