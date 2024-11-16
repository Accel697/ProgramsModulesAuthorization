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

        int click;

        public Auto()
        {
            InitializeComponent();

            click = 0;
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
            string password = pswbPassword.Text.Trim();
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
                    LoadPage(user);
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
                if (user != null && tbCaptcha.Text == tblCaptcha.Text)
                {
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    tblCaptcha.Text = "Text";
                    tbCaptcha.Text = "";
                    tblCaptcha.Visibility= Visibility.Hidden;
                    MessageBox.Show("Вы вошли под: " + user.Login.ToString());
                    LoadPage(user);
                }
                else
                {

                    tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
                    tbCaptcha.Text = "";
                    MessageBox.Show("Пройдите капчу заново!");
                }
            }
        }

        private void LoadPage(User user)
        {
            click = 0;
            NavigationService.Navigate(new practic3.Client(user));
        }
    }
}
