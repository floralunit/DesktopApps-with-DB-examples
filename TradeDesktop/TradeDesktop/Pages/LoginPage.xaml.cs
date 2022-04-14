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

namespace TradeDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
            private byte _failedCounter = 0; // Счётчик попыток неправильной авторизации
            private DispatcherTimer _timer = new DispatcherTimer(); // Таймер для блокирования кнопки
            private string _answer; // Переменная для сохранения ответа на каптчу

            public LoginPage()
            {
                InitializeComponent();
                imgCaptcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4); // Создание каптчи с буквами и цифрами и 
                                                                                                // Длиной = 4 символам
                _answer = imgCaptcha.CaptchaText;
            }

            private  void btnEnter_Click(object sender, RoutedEventArgs e) // Кнопка авторизации
            {
            if (_answer.ToLower() == tbxCaptchaAnswer.Text.ToLower() == false) MessageBox.Show("Неправильно введена каптча!", "Ошибка авторизации",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            var currentUser = App.Context.User.FirstOrDefault(p => p.UserLogin == tbxLogin.Text &&
                    p.UserPassword == pbxPassword.Password); // Запрос к базе данных. Сравнение логина и пароля из БД с тем, что в приложении
                    if (currentUser != null && _answer.ToLower() == tbxCaptchaAnswer.Text.ToLower() == true) // Если всё верно
                    {
                        App.CurrentUser = currentUser; // Создаём юзера
                        NavigationService.Navigate(new ProductsPage()); // Перенаправление на страницу ServicePage
                    }

                    if (_failedCounter == 2) // Если не получилось зайти с двух раз
                    {
                        MessageBox.Show("Вы ввели данные неправильно более 2-ух раз,\nкнопка заблокирована на 10 секунд", "Блокировка кнопки",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        _failedCounter = 0;
                        btnEnter.IsEnabled = false;
                        _timer.Interval = TimeSpan.FromSeconds(10);
                        _timer.Tick += TimerTick; // Добавление события которое будет обрабатываться после окончания таймера
                        _timer.Start();
                    }
                    else if (currentUser == null) // Если не получилось зайти
                    {
                        MessageBox.Show("Логин или пароль введены неправильно", "Ошибка авторизации",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        _failedCounter++;
                    }
            }

            private void btnClear_Click(object sender, RoutedEventArgs e) // Кнопка очистки данных
            {
                tbxLogin.Text = "";
                tbxCaptchaAnswer.Text = "";
                pbxPassword.Password = "";
            }

            private void TimerTick(object sender, EventArgs e) // Метод для работы с таймером
            {
                _timer.Stop();
                btnEnter.IsEnabled = true;
                tbxLogin.Text = "";
                tbxCaptchaAnswer.Text = "";
                pbxPassword.Password = "";
            }
        }
}
