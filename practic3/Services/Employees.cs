using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic3.Services
{
    /// <summary>
    /// класс для получения информации о сотрудниках, которая используется в карточках и поиске
    /// </summary>
    internal class Employees
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public string PositionAtWork { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
    }
}
