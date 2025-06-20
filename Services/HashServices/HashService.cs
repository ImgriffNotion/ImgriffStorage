using System.Security.Cryptography;
using System.Text;

namespace ImgriffStorage.Services.HashServices
{
    public class HashService : IHashService
    {
        public string HashString(string originalString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(originalString);

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
