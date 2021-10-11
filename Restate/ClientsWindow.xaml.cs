using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    { 
        List<Client> clients = new List<Client>();
        public ClientsWindow()
    {
        InitializeComponent();
        //var agents = App.Context.PersonSet_Agent.ToList();
        //agents_listbox.ItemsSource = agents;
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        string sqlExpression =
            "select FirstName, MiddleName, LastName, Phone, Email from PersonSet, PersonSet_Client where PersonSet.Id = PersonSet_Client.Id; ";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) // если есть данные
            {

                while (reader.Read()) // построчно считываем данные
                {
                    object firstname = reader.GetValue(0);
                    object middlename = reader.GetValue(1);
                    object lastname = reader.GetValue(2);
                    object phone = reader.GetValue(3);
                        object email = reader.GetValue(4);
                        clients.Add(new Client()
                    {
                        FirstName = Convert.ToString(firstname),
                        MiddleName = Convert.ToString(middlename),
                        LastName = Convert.ToString(lastname),
                        Phone = Convert.ToString(phone),
                            Email = Convert.ToString(email),
                        });
                }
            }

            reader.Close();
        }
        clients_listbox.ItemsSource = clients;

    }


    private void clients_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int i = clients_listbox.SelectedIndex;
        firstname_textbox.Text = clients[i].FirstName;
        middlename_textbox.Text = clients[i].MiddleName;
        lastname_textbox.Text = clients[i].LastName;
        phone_textbox.Text = clients[i].Phone;
            email_textbox.Text = clients[i].Email;
            string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
            string name = clients[i].FirstName;
            string query = String.Format("select FirstName, MiddleName, LastName, Phone, Email from PersonSet, PersonSet_Client where PersonSet.Id = PersonSet_Client.Id; ");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(
                                query, connection))
                {
                    // 3
                    // Use DataAdapter to fill DataTable
                    DataTable t = new DataTable();
                    a.Fill(t);
                    DataView tt = t.DefaultView;
                    // 4
                    // Render data onto the screen
                    demands_datagrid.ItemsSource = tt;
                }
            }
            clients_listbox.ItemsSource = clients;
        }
    public class Client
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
            public string Email { get; set; }

        }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Double width = SystemParameters.FullPrimaryScreenWidth;
        Double height = SystemParameters.FullPrimaryScreenHeight;
        this.Top = (height - this.Height) / 2;
        this.Left = (width - this.Width) / 2;
    }

    private void exit_button_Click(object sender, RoutedEventArgs e)
    {
        Close();
        new MainWindow().Show();
    }
}
}
