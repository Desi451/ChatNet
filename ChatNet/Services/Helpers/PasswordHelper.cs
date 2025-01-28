using System.Security.Cryptography;
using System.Text;

namespace ChatNet.Services.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {

        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }

            return result.ToString();
        }
    }

    public static bool VerifyPassword(string inputPassword, string storedHashedPassword)
    {
        string hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedHashedPassword;
    }
}
