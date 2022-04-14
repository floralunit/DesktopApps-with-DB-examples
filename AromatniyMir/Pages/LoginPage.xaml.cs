using Microsoft.EntityFrameworkCore;
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

namespace AromatniyMir.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private byte _failedCounter = 0;
        private DispatcherTimer _timer = new DispatcherTimer();
        private string _answer;

        public LoginPage()
        {
            InitializeComponent();
            UpdateCaptcha();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbxCaptchaAnswer.Text = "";
            tbxLogin.Text = "";
            pbxPassword.Password = "";
        }

        private async void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = await App.db.Users.FirstOrDefaultAsync(e => e.UserLogin == tbxLogin.Text && e.UserPassword == pbxPassword.Password);
            try
            {
                if (currentUser != null && _answer.ToLower() == tbxCaptchaAnswer.Text.ToLower() == true)
                {
                    App.CurrentUser = currentUser;
                    NavigationService.Navigate(new ProductPage());
                }
                if (_failedCounter == 2)
                {
                    MessageBox.Show("Вы ввели данные неправильно более 2 раз, \nкнопка заблокирована на 10 секунд", "Блокировка кнопки", MessageBoxButton.OK, MessageBoxImage.Information);
                    imgCaptcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
                    _answer = imgCaptcha.CaptchaText;
                    _failedCounter = 0;
                    btnEnter.IsEnabled = false;
                    _timer.Interval = TimeSpan.FromSeconds(10);
                    _timer.Tick += TimerTick;
                    _timer.Start();
                }
                else if (currentUser == null)
                {
                    MessageBox.Show("Логин или пароль введены неправильно", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateCaptcha();
                    _failedCounter++;
                }
                else if (_answer.ToLower() != tbxCaptchaAnswer.Text.ToLower())
                {
                    MessageBox.Show("Проверьте ввод каптчи", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateCaptcha();
                    _failedCounter++;
                }
            }
            catch (Exception ex) // Простая защита от вылета
            {
                MessageBox.Show(ex.Message, "Ошибка при авторизации");
            }

        }
        private void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            btnEnter.IsEnabled = true;
            tbxLogin.Text = "";
            tbxCaptchaAnswer.Text = "";
            pbxPassword.Password = "";
        }
        private void UpdateCaptcha()
        {
            imgCaptcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
            _answer = imgCaptcha.CaptchaText;
        }

    }
}
