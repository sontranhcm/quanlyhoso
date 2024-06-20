using System.Security.Cryptography;
using System.Text;

namespace SONTM.WEB.Helpers
{
    public static class PasswordHelper
    {
        private static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            // Logic để so sánh mật khẩu đã hash với mật khẩu người dùng nhập vào
            // Bạn có thể sử dụng các thư viện hash như BCrypt, PBKDF2, Argon2, etc.
            // Ví dụ đơn giản sử dụng hashing SHA256:
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                return storedHashedPassword == hashedPassword;
            }
        }
        public static bool IsValidPassword(string enteredPassword, string storedHashedPassword)
        {
            return VerifyPassword(enteredPassword, storedHashedPassword);
        }
    }
}
