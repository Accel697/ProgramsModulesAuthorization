using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using practic3.Models;

namespace practic3.Services
{
    internal class ValidateEmployees
    {
        /// <summary>
        /// валидация информации о сотрудниках
        /// </summary>
        /// <param name="employee"> экземпляр класса сотрудника </param>
        /// <returns> список ошибок при заполнении полей данных для сотрудника </returns>
        public string ValidateEmployee(Employee employee)
        {
            var errorMessages = new List<string>();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(employee, null, null);
            bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

            if (!isValid)
            {
                errorMessages.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
            return string.Join("\n", errorMessages);
        }
    }
}
