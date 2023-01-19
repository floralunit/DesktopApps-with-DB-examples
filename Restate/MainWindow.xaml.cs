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

namespace Restate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/logo.png"));
        }

        private void agents_button_Click(object sender, RoutedEventArgs e)
        {
            new AgentsWindow().Show();
        }

        private void clients_button_Click(object sender, RoutedEventArgs e)
        {
            new ClientsWindow().Show();
        }

        private void restate_button_Click(object sender, RoutedEventArgs e)
        {
            new RealEstate().Show();
        }

        private void supplies_button_Click(object sender, RoutedEventArgs e)
        {
            new SuppliesWindow().Show();
        }

        private void demands_button_Click(object sender, RoutedEventArgs e)
        {
            new DemandsWindow().Show();
        }

        private void deals_button_Click(object sender, RoutedEventArgs e)
        {
            new DealsWindow().Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Double width = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
            this.Top = (height - this.Height) / 2;
            this.Left = (width - this.Width) / 2;
        }
    }
}
