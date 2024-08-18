using System.Security.Cryptography;

namespace Core.Helpers
{
    public static class ServicesHelpers
    {
        public static string GenerateUniqueString()
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
