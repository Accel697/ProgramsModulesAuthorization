using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace practic3.Services
{
    internal class Hash
    {
        /// <summary>
        /// хэширует пароль
        /// </summary>
        /// <param name="password"> пароль пользователя </param>
        /// <returns> хэшированный пароль </returns>
        public static string HashPassword(string password)
        {
            using (SHA256 shs256Hash = SHA256.Create())
            {
                byte[] sourceBytePassword = Encoding.UTF8.GetBytes(password);
                byte[] hash = shs256Hash.ComputeHash(sourceBytePassword);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
