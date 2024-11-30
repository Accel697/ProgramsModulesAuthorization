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
using System.Windows.Threading;
using practic3;
using practic3.Models;
using practic3.Services;

namespace AutoservicesRul.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auto.xaml
    /// </summary>
    public partial class Auto : Page
    {

        private DispatcherTimer timer;
        private int remainingTime;
        int click;

        public Auto()
        {
            InitializeComponent();
            CreateTimer();
            click = 0;
        }
        private void CreateTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new practic3.Client(null));
        }

        private void GenerateCapctcha()
        {
            tbCaptcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = capctchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password.Trim();
            string hashPassw = Hash.HashPassword(password);

            furniture_centreEntities db = Helper.GetContext();

            var user = db.User.Where(x => x.Login == login && x.Password == hashPassw).FirstOrDefault();
            if (click == 1)
            {
                if (user != null)
                {
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    MessageBox.Show("Вы вошли под: " + user.Login.ToString());
                    LoadPage(user, user.Employee.ToString());
                }
                else
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    GenerateCapctcha();

                    pswbPassword.Clear();

                    tblCaptcha.Visibility= Visibility.Visible;
                    tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
                }
            }
            else if (click > 1)
            {
                if (click == 3)
                {
                    BlockControls();

                    remainingTime = 10;
                    txtbTimer.Visibility = Visibility.Visible;
                    txtbTimer.Text = $"Оставшееся время: {remainingTime} секунд";

                    timer.Start();
                }

                if (user != null && tbCaptcha.Text == tblCaptcha.Text)
                {
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    tblCaptcha.Text = "Text";
                    tbCaptcha.Text = "";
                    tbCaptcha.Visibility = Visibility.Hidden;
                    tblCaptcha.Visibility= Visibility.Hidden;
                    MessageBox.Show("Вы вошли под: " + user.Login.ToString());
                    LoadPage(user, user.Employee.ToString());
                }
                else
                {

                    tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
                    tbCaptcha.Text = "";
                    MessageBox.Show("Пройдите капчу заново!");
                }
            }
        }

        private void LoadPage(User user, string idEmployee)
        {
            click = 0;
            switch (idEmployee)
            {
                case "1":
                    NavigationService.Navigate(new Admin(user));
                    break;
                default:
                    NavigationService.Navigate(new practic3.Client(user));
                    break;
            }
        }

        private void BlockControls()
        {
            txtbLogin.IsEnabled = false;
            pswbPassword.IsEnabled = false;
            tbCaptcha.IsEnabled = false;
            btnEnterGuests.IsEnabled = false;
            btnEnter.IsEnabled = false;
        }

        private void UnlockControls()
        {
            txtbLogin.IsEnabled = true;
            pswbPassword.IsEnabled = true;
            tbCaptcha.IsEnabled = true;
            btnEnterGuests.IsEnabled = true;
            btnEnter.IsEnabled = true;
            txtbLogin.Clear();
            pswbPassword.Clear();
            tblCaptcha.Text = "Text";
            tbCaptcha.Text = "";
            tbCaptcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;
            click = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime--;

            if (remainingTime <= 0)
            {
                timer.Stop();
                UnlockControls();
                txtbTimer.Visibility = Visibility.Hidden;
                return;
            }

            txtbTimer.Text = $"Оставшееся время: {remainingTime} секунд";
        }
    }
}
