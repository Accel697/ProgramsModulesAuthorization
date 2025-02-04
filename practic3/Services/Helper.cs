using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using practic3.Models;

namespace practic3.Services
{
    internal class Helper
    {
        /// <summary>
        /// создает связь с базой данных
        /// </summary>
        /// <returns> контекст базы данных </returns>
        public static furniture_centreEntities GetContext()
        {
            return new furniture_centreEntities();

        }
    }
}
