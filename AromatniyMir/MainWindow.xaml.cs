using AromatniyMir.Pages;
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

namespace AromatniyMir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frameMain.Content = new LoginPage();
            frameMain.LoadCompleted += FrameMain_LoadCompleated;
        }

        private void FrameMain_LoadCompleated(object sender, NavigationEventArgs e)
        {
            if (!frameMain.CanGoBack)
            {
                btnBack.Content = "Показать товары";
                userDataBlock.Text = "";
                userDataBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnBack.Content = "Назад";
                if (App.CurrentUser != null)
                {
                    userDataBlock.Text = App.CurrentUser.UserSurname + " "
                        + App.CurrentUser.UserName
                        + " " + App.CurrentUser.UserPatronymic;
                    userDataBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.CanGoBack)
                frameMain.GoBack();
            else if (!frameMain.CanGoBack && App.CurrentUser != null)
            {
                App.CurrentUser = null;
                frameMain.Navigate(new ProductPage());
            }
            else
                frameMain.Navigate(new ProductPage());
        }
    }
}
