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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using practic3.Models;
using practic3.Services;

namespace practic3
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        public AddEmployee()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            using (var context = Helper.GetContext())
            {
                var genders = context.Gender.ToList();
                if (genders.Any())
                {
                    cbGender.ItemsSource = genders;
                    cbGender.DisplayMemberPath = "Name";
                    cbGender.SelectedValuePath = "ID";
                }
                else
                {
                    MessageBox.Show("Полы не найдены");
                }

                var jobs = context.Job_title.ToList();
                if (jobs.Any())
                {
                    cbPositionAtWork.ItemsSource = jobs;
                    cbPositionAtWork.DisplayMemberPath = "Name";
                    cbPositionAtWork.SelectedValuePath = "ID";
                }
                else
                {
                    MessageBox.Show("Должности не найдены");
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (cbGender.SelectedItem == null || cbPositionAtWork.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите пол и должность", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedGender = cbGender.SelectedItem as Gender;
                var selectedPosition = cbPositionAtWork.SelectedItem as Job_title;

                if (selectedGender == null || selectedPosition == null)
                {
                    MessageBox.Show("Не удалось получить выбранные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newEmployee = new Employee
                {
                    First_name = tbFirstName.Text,
                    Last_name = tbLastName.Text,
                    Midle_name = tbMiddleName.Text,
                    Born_date = DateTime.Parse(tbBornDate.Text),
                    Gender = selectedGender.ID,
                    Position_at_work = selectedPosition.ID,
                    Wages = decimal.Parse(tbWages.Text),
                    Passport_serial = decimal.Parse(tbPassportSerial.Text),
                    Passport_number = decimal.Parse(tbPassportNumber.Text),
                    Registration = tbRegistration.Text,
                    E_mail = tbEmail.Text,
                    Phone_number = tbPhoneNumber.Text
                };

                using (var context = Helper.GetContext())
                {
                    context.Employee.Add(newEmployee);
                    context.SaveChanges();
                }

                MessageBox.Show("Сотрудник успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
