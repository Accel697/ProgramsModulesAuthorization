using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace practic3.Services
{
    internal class LaborContractGenerator
    {
        private string templatePath = "D:\\programmesmodules\\blank_trudovogo_dogovora.docx";
        static string day;
        static string month;
        static string year;
        static string startDay;
        static string startMonth;
        static string startYear;

        private Dictionary<string, string> companyInfo = new Dictionary<string, string>
        {
            { "employerForm", "OOO" },
            { "employerName", "Тмыв Денег" },
            { "employerINN", "14152385185" },
            { "generalDirector", "Мастер Оф Фурнитур" },
            { "companyName", "IKEA" },
            { "companyAdress", "г. Новосибирск, ул. Ватутина 107" },
            { "normDoc", "Похвалы Императора" },
            { "normDocName", "Различные плюшки" }
        };

        public void GenerateContract(long employeeId)
        {
            GetCurrentDate();
            using (var context = Helper.GetContext())
            {
                var employee = context.Employee.Find(employeeId);
                if (employee != null)
                {
                    using (DocX document = DocX.Load(templatePath)) 
                    {
                        document.ReplaceText("{ID}", $"{employee.ID}");
                        document.ReplaceText("{City}", "Новосибирск");
                        document.ReplaceText("{Day}", $"{day}");
                        document.ReplaceText("{Month}", $"{month}");
                        document.ReplaceText("{Year}", $"{year}");
                        document.ReplaceText("{EmployerForm}", companyInfo["employerForm"]);
                        document.ReplaceText("{Employer}", companyInfo["employerName"]);
                        document.ReplaceText("{GeneralDirector}", companyInfo["generalDirector"]);
                        document.ReplaceText("{FullName}", $"{employee.Last_name} {employee.First_name}" + (string.IsNullOrEmpty(employee.Midle_name) ? "" : $" {employee.Midle_name}"));
                        document.ReplaceText("{CompanyName}", companyInfo["companyName"]);
                        document.ReplaceText("{JobTitle}", $"{employee.Job_title.Name}");
                        document.ReplaceText("{CompanyAdress}", companyInfo["companyAdress"]);
                        document.ReplaceText("{StartDay}", $"{startDay}");
                        document.ReplaceText("{StartMonth}", $"{startMonth}");
                        document.ReplaceText("{StartYear}", $"{startYear}");
                        document.ReplaceText("{TestDuration}", "1");
                        document.ReplaceText("{Wages}", $"{employee.Wages: ##,##}");
                        document.ReplaceText("{NormDoc}", companyInfo["normDoc"]);
                        document.ReplaceText("{NormDocName}", companyInfo["normDocName"]);
                        document.ReplaceText("{Employee}", $"{employee.Last_name} {employee.First_name[0]}. {(string.IsNullOrEmpty(employee.Midle_name) ? "" : $"{employee.Midle_name[0]}.")}");
                        document.ReplaceText("{Serial}", $"{employee.Passport_serial}");
                        document.ReplaceText("{Number}", $"{employee.Passport_number}");
                        document.ReplaceText("{Issued}", "Императором России");
                        document.ReplaceText("{INN}", companyInfo["employerINN"]);
                        document.SaveAs(System.IO.Path.Combine("C:\\Users\\Accel\\Downloads\\", $"Labor_Contract_{employee.Last_name}_{employee.First_name}.docx"));
                    }
                }
            }
        }

        static void GetCurrentDate()
        {
            DateTime now = DateTime.Now;
            DateTime startDate = now.AddDays(3);

            day = now.Day.ToString();
            month = now.ToString("MMMM", CultureInfo.CurrentCulture);
            year = now.Year.ToString().Substring(2);

            if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday) 
            { 
                startDate = startDate.AddDays(1);
            }

            startDay = startDate.Day.ToString();
            startMonth = startDate.ToString("MMMM", CultureInfo.CurrentCulture);
            startYear = startDate.Year.ToString();
        }
    }
}
