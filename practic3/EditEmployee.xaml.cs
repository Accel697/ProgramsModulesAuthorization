﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Page
    {
        private Employee _employee;

        public EditEmployee(long employeeId)
        {
            InitializeComponent();
            LoadEmployeeData(employeeId);
        }

        private void LoadEmployeeData(long employeeId)
        {
            using (var db = Helper.GetContext())
            {
                _employee = db.Employee.Find(employeeId);

                if (_employee != null)
                {
                    tbFirstName.Text = _employee.First_name.ToString();
                    tbLastName.Text = _employee.Last_name.ToString();
                    tbMiddleName.Text = _employee.Midle_name.ToString();
                    tbBornDate.Text = _employee.Born_date.ToString();
                    tbGender.Text = _employee.Gender.ToString();
                    tbPositionAtWork.Text = _employee.Position_at_work.ToString();
                    tbWages.Text = _employee.Wages.ToString();
                    tbPassportSerial.Text = _employee.Passport_serial.ToString();
                    tbPassportNumber.Text = _employee.Passport_number.ToString();
                    tbRegistration.Text = _employee.Registration.ToString();
                    tbEmail.Text = _employee.E_mail.ToString();
                    tbPhoneNumber.Text = _employee.Phone_number.ToString();
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = Helper.GetContext())
                {
                    var existingEmployee = db.Employee.Find(_employee.ID);

                    if (existingEmployee != null)
                    {
                        existingEmployee.First_name = tbFirstName.Text;
                        existingEmployee.Last_name = tbLastName.Text;
                        existingEmployee.Midle_name = tbMiddleName.Text;
                        existingEmployee.Born_date = DateTime.Parse(tbBornDate.Text);
                        existingEmployee.Gender = long.Parse(tbGender.Text);
                        existingEmployee.Position_at_work = long.Parse(tbPositionAtWork.Text);
                        existingEmployee.Wages = decimal.Parse(tbWages.Text);
                        existingEmployee.Passport_serial = decimal.Parse(tbPassportSerial.Text);
                        existingEmployee.Passport_number = decimal.Parse(tbPassportNumber.Text);
                        existingEmployee.Registration = tbRegistration.Text;
                        existingEmployee.E_mail = tbEmail.Text;
                        existingEmployee.Phone_number = tbPhoneNumber.Text;

                        db.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = Helper.GetContext())
                    {
                        var employeeToDelete = db.Employee.Find(_employee.ID);

                        if (employeeToDelete != null)
                        {
                            db.Employee.Remove(employeeToDelete);
                            db.SaveChanges();

                            MessageBox.Show("Сотрудник успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
