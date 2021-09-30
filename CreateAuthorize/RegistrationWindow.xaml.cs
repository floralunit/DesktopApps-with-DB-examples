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
using System.Windows.Shapes;

namespace CreateAuthorize
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// DBContainer db;
    public partial class RegistrationWindow : Window
    {
        DBContainer db;
        public RegistrationWindow()
        {
            InitializeComponent();
            db = new DBContainer();
        }

        private void RegistrationClick(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" || password.Password == "" || lastname.Text == "" || firstname.Text == "" || middlename.Text == "")
            {
                MessageBox.Show("Пустые поля");
                return;
            }
            if (db.Users.Select(Item => Item.Login).Contains(login.Text))
            {
                MessageBox.Show("Такой логин существует в системе");
            }
            User newUser = new User()
            {
                Login = login.Text,
                Password = password.Password,
                LastName = lastname.Text,
                FirstName = firstname.Text,
                MiddleName = middlename.Text,
            };
            db.Users.Add(newUser);
            db.SaveChanges();
            MessageBox.Show("Вы успешно зарегестрировались!");
            AuthorizationWindow aw = new AuthorizationWindow();
            aw.Show();
            this.Close();
            }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow aw = new AuthorizationWindow();
            aw.Show();
            this.Close();
        }
    }
}
