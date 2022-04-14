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

namespace Demo2.Pages
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
        }
        // Обработка нажатия на кнопку "Войти"
        private async void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.currentUser = await App.db.Users.FirstOrDefaultAsync(e => e.UserLogin == textBoxLogin.Text &&
                e.UserPassword == passwordBoxPassword.Password);
                if (App.currentUser != null)
                {
                    _failedCounter = 0;
                    NavigationService.Navigate(new ProductPage());
                }
                else if (App.currentUser != null && _answer.ToLower() == captcha.CaptchaText.ToLower())
                {
                    _failedCounter = 0;
                    NavigationService.Navigate(new ProductPage());
                }
                else if (App.currentUser == null && _failedCounter == 0)
                {
                    MessageBox.Show("Введён неверный логин или пароль", "Ошибка при авторизации", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    _failedCounter++;
                    UpdateCaptcha();
                    captcha.Visibility = Visibility.Visible;
                    textBoxAnswerCaptcha.Visibility = Visibility.Visible;
                }
                else if (App.currentUser == null && _failedCounter == 1)
                {
                    MessageBox.Show("Авторизация не пройдена, кнопка заблокирована на 10 секунд", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    buttonLogin.IsEnabled = false;
                    _timer.Interval = TimeSpan.FromSeconds(10);
                    _timer.Tick += TimerTick;
                    _timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при авторизации");
            }
        }

        // Обработка нажатия на кнопку "Очистить"
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            CleanupTextBoxes();
        }

        // Метод который сработает по окончанию таймера
        private void TimerTick(object? sender, EventArgs e)
        {
            _timer.Stop();
            buttonLogin.IsEnabled = true;
            CleanupTextBoxes();
        }

        // Метод очищающий все данные с формы
        private void CleanupTextBoxes()
        {
            textBoxLogin.Text = "";
            passwordBoxPassword.Password = "";
            textBoxAnswerCaptcha.Text = "";
        }

        private void UpdateCaptcha()
        {
            captcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
            _answer = captcha.CaptchaText;
        }
    }
}
