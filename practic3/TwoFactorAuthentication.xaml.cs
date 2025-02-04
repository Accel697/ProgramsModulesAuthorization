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
using System.Windows.Threading;
using practic3.Models;
using practic3.Services;

namespace practic3
{
    /// <summary>
    /// Логика взаимодействия для TwoFactorAuthentication.xaml
    /// </summary>
    public partial class TwoFactorAuthentication : Page
    {
        private User _user;
        private string _positionAtWork;
        private string _email;
        private string _confirmationCode;
        private DispatcherTimer timer;
        private int remainingTime;

        public TwoFactorAuthentication(User user, string idPositionAtWork)
        {
            InitializeComponent();
            CreateTimer();
            _user = user;
            _positionAtWork = idPositionAtWork;
            _email = user.Employee1.E_mail;
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

        /// <summary>
        /// обрабатывает нажатие на кнопку подтверждения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtbConfirmCode.Text == _confirmationCode)// сравнивается отправленный на почту код и введенный пользователем код
            {
                LoadPage(_user, _positionAtWork);
            }
        }

        /// <summary>
        /// обрабатывает нажатие на кнопку отправки кода подтверждения на почту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show("Email сотрудника не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// направляет пользователя на нужную страницу в зависимости от должности
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idPositionAtWork"></param>
        private void LoadPage(User user, string idPositionAtWork)
        {
            switch (idPositionAtWork)
            {
                case "1":
                    NavigationService.Navigate(new Admin(user));
                    break;
                default:
                    NavigationService.Navigate(new practic3.Client(user));
                    break;
            }
        }
    }
}
