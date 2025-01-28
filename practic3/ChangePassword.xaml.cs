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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using practic3.Models;
using practic3.Services;

namespace practic3
{
    /// <summary>
    /// Логика взаимодействия для ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        private User _user;
        private string _email;
        private string _confirmationCode;
        private DispatcherTimer timer;
        private int remainingTime;

        public ChangePassword(string login)
        {
            InitializeComponent();
            CreateTimer();
            FindUser(login);
        }

        private void FindUser(string login)
        {
            using (var context = Helper.GetContext())
            {
                _user = context.User.FirstOrDefault(u => u.Login == login);

                if (_user != null)
                {
                    _email = _user.Employee1.E_mail;
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void CreateTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime--;

            if (remainingTime <= 0)
            {
                timer.Stop();
                btnSend.IsEnabled = true;
                txtbTimer.Visibility = Visibility.Hidden;
                return;
            }

            txtbTimer.Text = $"Отправить код повторно \nчерез: {remainingTime} секунд";
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_email != null)
            {
                ConfirmationCode confCode = new ConfirmationCode();
                _confirmationCode = confCode.SendEmail(_email);
                btnSend.IsEnabled = false;
                remainingTime = 60;
                txtbTimer.Visibility = Visibility.Visible;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Email сотрудника не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = Helper.GetContext()) 
            {
                var existingUser = context.User.Find(_user.ID);
                string password;

                if (existingUser != null)
                {
                    if (txtbNewPassword.Text == txtbConfirmPassword.Text)
                    {
                        password = txtbConfirmPassword.Text;
                        string hashPassw = Hash.HashPassword(password);
                        if (hashPassw == existingUser.Password)
                        {
                            MessageBox.Show("Новый пароль не может быть таким же как старый", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else 
                        { 
                            existingUser.Password = hashPassw;
                        }
                    } else
                    {
                        MessageBox.Show("Введите одинаковый пароль в оба поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при смене пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            NavigationService.GoBack();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (txtbConfirmCode.Text == _confirmationCode)
            {
                txtbConfirmCode.IsEnabled = false;
                if (remainingTime > 0)
                {
                    timer.Stop();
                    txtbTimer.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnSend.IsEnabled = false;
                }
                btnContinue.IsEnabled = false;
                txtbNewPassword.IsEnabled = true;
                txtbConfirmPassword.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
