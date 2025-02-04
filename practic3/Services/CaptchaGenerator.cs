using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic3.Services
{
    internal class CaptchaGenerator
    {
        private static readonly Random random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// генерирует текст для каптчи
        /// </summary>
        /// <param name="length"> длину каптчи </param>
        /// <returns> текст каптчи </returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GenerateCaptchaText(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Длина текста капчи должна быть больше нуля.");

            StringBuilder captchaText = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(Characters.Length);
                captchaText.Append(Characters[index]);
            }

            return captchaText.ToString();
        }
    }
}
