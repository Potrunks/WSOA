using System.Security.Cryptography;
using System.Text;

namespace WSOA.Server.Business.Utils
{
    public static class SecurityUtil
    {
        /// <summary>
        /// Convert string to SHA256 hash.
        /// </summary>
        public static string ToSha256(this string toHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(toHash);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

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
