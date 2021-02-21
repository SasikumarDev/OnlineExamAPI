using System;
using System.Security.Cryptography;
using System.Text;

namespace OnlineExamAPI.common
{
    public class PasswordDecrpt
    {
        public static string DecryptPassword(string Password)
        {
            StringBuilder Passwordrtn = new StringBuilder();
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] Passbytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                foreach (byte Bts in Passbytes)
                {
                    Passwordrtn.Append(Bts.ToString("X2"));
                }
            }
            return Passwordrtn.ToString();
        }
    }
}