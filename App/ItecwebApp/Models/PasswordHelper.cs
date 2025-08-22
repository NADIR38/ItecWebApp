using System.Security.Cryptography;
using System.Text;

namespace ItecwebApp.Helpers
{
    public static class PasswordHelper
    {
        // Hash password using SHA256 (you can also use BCrypt or PBKDF2 for stronger security)
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Verify entered password against stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
