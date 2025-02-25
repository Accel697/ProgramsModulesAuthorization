using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using practic3.Models;
using practic3.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace practic3
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private List<Employees> _employees;
        private List<Employees> _filteredEmployees;
        private List<string> _jobTitles;

        public Admin(User user)
        {
            InitializeComponent();
            LoadEmployees();
            LoadJobTitles();
        }

        /// <summary>
        /// загрузка карточек сотрудников
        /// </summary>
        private void LoadEmployees()
        {
            _employees = Helper.GetContext().Employee.Select(e => new Employees
            {
                ID = e.ID.ToString(),
                FirstName = e.First_name,
                LastName = e.Last_name,
                PositionAtWork = e.Job_title.Name,
                PhoneNumber = e.Phone_number
            }).ToList();
            foreach (var employee in _employees)
            {
                employee.FullName = $"{employee.LastName} {employee.FirstName}";
                employee.PhotoUrl = "Resources\\default_photo.jpg";
            }
            EmployeesListView.ItemsSource = _employees;
        }

        /// <summary>
        /// загрузка должностей для сортировки сотрудников
        /// </summary>
        private void LoadJobTitles()
        {
            _jobTitles = Helper.GetContext().Job_title.Select(j => j.Name).Distinct().ToList();

            _jobTitles.Insert(0, "Все должности");

            cbJobTitle.ItemsSource = _jobTitles;

            cbJobTitle.SelectedIndex = 0;
        }

        /// <summary>
        /// поиск сотрудников по фио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEmployees();
        }

        /// <summary>
        /// сортировка сотрудников по должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbJobTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployees();
        }

        /// <summary>
        /// отображения сотрудников в зависимости от критериев поиска
        /// </summary>
        private void FilterEmployees()
        {
            string searchText = tbSearch.Text.ToLower();
            string selectedJobTitle = cbJobTitle.SelectedItem as string;

            _filteredEmployees = _employees.Where(emp =>
                (emp.LastName + " " + emp.FirstName + " " + emp.MiddleName).ToLower().Contains(searchText) &&
                (selectedJobTitle == "Все должности" || emp.PositionAtWork == selectedJobTitle))
                .ToList();

            EmployeesListView.ItemsSource = null;
            EmployeesListView.ItemsSource = _filteredEmployees;
        }

        /// <summary>
        /// обрабатывает двойной клик по карточке сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employees selectedEmployee)
            {
                try
                {
                    long employeeId = long.Parse(selectedEmployee.ID);

                    using (var db = Helper.GetContext())
                    {
                        var employeeExists = db.Employee.Find(employeeId) != null;

                        if (employeeExists)
                        {
                            NavigationService.Navigate(new EditEmployee(employeeId));
                        }
                        else
                        {
                            MessageBox.Show($"Сотрудник с ID = {employeeId} не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Ошибка формата ID: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// обрабатывет нажатие на кнопку добавления нового сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployee());
        }

        /// <summary>
        /// обрабатывает нажатие кнопки обновления страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        /// <summary>
        /// обрабатывает нажатие на кнопку печати списка сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintEmployee_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument docToPrint = new FlowDocument();
                foreach (var employee in _employees)
                {
                    var employeeBlock = new Paragraph();
                    employeeBlock.Inlines.Add(new Run($"ФИО: {employee.FullName}\n"));
                    employeeBlock.Inlines.Add(new Run($"Должность: {employee.PositionAtWork}\n"));
                    employeeBlock.Inlines.Add(new Run($"Телефон: {employee.PhoneNumber}\n"));
                    docToPrint.Blocks.Add(employeeBlock);
                }

                IDocumentPaginatorSource idpSource = docToPrint;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Список сотрудников");
            }
        }

        private void PrintFurniture_Click(object sender, RoutedEventArgs e)
        {
            using (var context = Helper.GetContext())
            {
                var _furnitureItems = context.Furniture.ToList();

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    FlowDocument docToPrint = new FlowDocument();
                    foreach (var furniture in _furnitureItems)
                    {
                        var furnitureBlock = new Paragraph();
                        furnitureBlock.Inlines.Add(new Run($"Название: {furniture.Name}\n"));
                        furnitureBlock.Inlines.Add(new Run($"Тип мебели: {furniture.Type_of_furniture.Name}\n"));
                        furnitureBlock.Inlines.Add(new Run($"ФИО дизайнера: {furniture.Employee.Last_name} {furniture.Employee.First_name}\n"));
                        furnitureBlock.Inlines.Add(new Run($"Цена: {furniture.Price}\n"));
                        docToPrint.Blocks.Add(furnitureBlock);
                    }

                    IDocumentPaginatorSource idpSource = docToPrint;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Список мебели");
                }
            }
        }

        private void DisplayExcelEmployee_Click(object sender, RoutedEventArgs e) 
        {
            using (var context = Helper.GetContext()) 
            {
                var employees = context.Employee.ToList();

                var excelApp = new Excel.Application();
                excelApp.Visible = true;
                excelApp.Workbooks.Add();
                Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

                workSheet.Cells[1, 1] = "ID";
                workSheet.Cells[1, 2] = "Имя";
                workSheet.Cells[1, 3] = "Фамилия";
                workSheet.Cells[1, 4] = "Отчество";
                workSheet.Cells[1, 5] = "Дата рождения";
                workSheet.Cells[1, 6] = "Пол";
                workSheet.Cells[1, 7] = "Должность";
                workSheet.Cells[1, 8] = "Зарплата";
                workSheet.Cells[1, 9] = "Серия паспорта";
                workSheet.Cells[1, 10] = "Номер паспорта";
                workSheet.Cells[1, 11] = "Регистрация";
                workSheet.Cells[1, 12] = "E-mail";
                workSheet.Cells[1, 13] = "Телефон";

                int row = 2;

                foreach (var employee in employees)
                {
                    workSheet.Cells[row, 1] = employee.ID.ToString();
                    workSheet.Cells[row, 2] = employee.First_name;
                    workSheet.Cells[row, 3] = employee.Last_name;
                    workSheet.Cells[row, 4] = employee.Midle_name;
                    workSheet.Cells[row, 5] = employee.Born_date.ToString();
                    workSheet.Cells[row, 6] = employee.Gender1.Name;
                    workSheet.Cells[row, 7] = employee.Job_title.Name;
                    workSheet.Cells[row, 8] = employee.Wages.ToString();
                    workSheet.Cells[row, 9] = employee.Passport_serial.ToString();
                    workSheet.Cells[row, 10] = employee.Passport_number.ToString();
                    workSheet.Cells[row, 11] = employee.Registration;
                    workSheet.Cells[row, 12] = employee.E_mail;
                    workSheet.Cells[row, 13] = employee.Phone_number;
                    row++;
                }

                workSheet.Columns[1].AutoFit();
                workSheet.Columns[2].AutoFit();
                workSheet.Columns[3].AutoFit();
                workSheet.Columns[4].AutoFit();
                workSheet.Columns[5].AutoFit();
                workSheet.Columns[6].AutoFit();
                workSheet.Columns[7].AutoFit();
                workSheet.Columns[8].AutoFit();
                workSheet.Columns[9].AutoFit();
                workSheet.Columns[10].AutoFit();
                workSheet.Columns[11].AutoFit();
                workSheet.Columns[12].AutoFit();
                workSheet.Columns[13].AutoFit();
            }
        }
    }
}
