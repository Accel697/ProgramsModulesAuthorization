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
        private static furniture_centreEntities _context;

        public static furniture_centreEntities GetContext()
        {
            if (_context == null)
            {
                _context = new furniture_centreEntities();
            }
            return _context;
        }
    }
}
