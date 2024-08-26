using System.Security.Cryptography;

namespace Core.Helpers
{
    public class ServicesHelpers
    {
        public string GenerateUniqueString()
        {
            var randomBytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
