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
                employee.PhotoUrl = "Resources/default_photo.jpg";
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
    }
}
