using System.Security.Cryptography;
using System.Text;

namespace capygram.Auth.Domain.Services
{
    public class Encrypter : IEncrypter
    {
        private static readonly int SaltSize = 40;
        private static readonly int DeriveBytesIterationsCount = 10000;

        public string GetHash(string value, string salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(value, Convert.FromBase64String(salt), DeriveBytesIterationsCount);

            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }


        public string GetSalt()
        {
            var bytes = new byte[SaltSize];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

    }
}
