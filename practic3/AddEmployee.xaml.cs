using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        /// <summary>
        /// загружает списки названий должностей и названий полов из базы 
        /// </summary>
        private void LoadComboBoxes()
        {
            using (var context = Helper.GetContext())
            {
                var genders = context.Gender.ToList();
                if (genders.Any())
                {
                    cbGender.ItemsSource = genders;// получаем данные из таблицы
                    cbGender.DisplayMemberPath = "Name";// отображает название пола
                    cbGender.SelectedValuePath = "ID";// заполняет id пола
                }
                else
                {
                    MessageBox.Show("Полы не найдены");
                }

                var jobs = context.Job_title.ToList();// полчаем данные из таблицы
                if (jobs.Any())
                {
                    cbPositionAtWork.ItemsSource = jobs;
                    cbPositionAtWork.DisplayMemberPath = "Name";// отображает название должности
                    cbPositionAtWork.SelectedValuePath = "ID";// заполняет id должности
                }
                else
                {
                    MessageBox.Show("Должности не найдены");
                }
            }
        }

        /// <summary>
        /// обрабатывает нажатие на кнопку добавления нового сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
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

            /*
             * выше проверяряется заполненость полей с полом и должностью
             * поля должны быть не пустыми и в них не должно быть неверных значений
             */

            var newEmployee = new Employee
            {
                First_name = tbFirstName.Text,
                Last_name = tbLastName.Text,
                Midle_name = tbMiddleName.Text,
                Born_date = DateTime.TryParse(tbBornDate.Text, out var bornDate) ? bornDate : DateTime.MinValue,
                Gender = selectedGender.ID,
                Position_at_work = selectedPosition.ID,
                Wages = decimal.TryParse(tbWages.Text, out var wages) ? wages : 0,
                Passport_serial = decimal.TryParse(tbPassportSerial.Text, out var passportSerial) ? passportSerial : 0,
                Passport_number = decimal.TryParse(tbPassportNumber.Text, out var passportNumber) ? passportNumber : 0,
                Registration = tbRegistration.Text,
                E_mail = tbEmail.Text,
                Phone_number = tbPhoneNumber.Text
            }; //создается новый экземпляр сотрудника

            ValidateEmployees validate = new ValidateEmployees(); //проверяется заполненность полей
            string validationMessage = validate.ValidateEmployee(newEmployee);
            if (!string.IsNullOrEmpty(newEmployee.Midle_name))
            {
                if (newEmployee.Midle_name.Length < 2 || newEmployee.Midle_name.Length > 20)
                {
                    validationMessage += "\nДлина отчества должна быть от 2 до 20 символов.";
                } // отчество проверяется отдельно, т.к. это единственное необязательное поле
            }
            if (!string.IsNullOrEmpty(validationMessage))
            {
                MessageBox.Show(validationMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }// проверяется результат вылидации

            try
            {
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
