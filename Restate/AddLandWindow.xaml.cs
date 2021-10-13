using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Restate
{
    /// <summary>
    /// Логика взаимодействия для AddLandWindow.xaml
    /// </summary>
    public partial class AddLandWindow : Window
    {
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        public AddLandWindow()
        {
            InitializeComponent();
        }
        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            string city = city_textbox.Text;
            string street = street_textbox.Text;
            int latitude = Convert.ToInt32(latitude_textbox.Text);
            int longitude = Convert.ToInt32(longitude_textbox.Text);
            double totalarea = Convert.ToDouble(totalarea_textbox.Text);
            string query1 = String.Format("INSERT INTO RealEstateSet VALUES('" + city + "', '" + street + "', null, null, '" + latitude + "', '" + longitude + "');");
            string query2 = String.Format("Insert into RealEstateSet_Land Values('" + totalarea + "', (select max(Id) from RealEstateSet)); ");
            string query = String.Format(query1 + query2);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            MessageBox.Show("Земельный участок успешно добавлен!");
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Double width = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
            this.Top = (height - this.Height) / 2;
            this.Left = (width - this.Width) / 2;
        }
    }
}
