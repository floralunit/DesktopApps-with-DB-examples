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
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        public ClientsWindow()
    {
        InitializeComponent();
        //var agents = App.Context.PersonSet_Agent.ToList();
        //agents_listbox.ItemsSource = agents;
        string sqlExpression =
            "select FirstName, MiddleName, LastName, Phone, Email, PersonSet.Id from PersonSet, PersonSet_Client where PersonSet.Id = PersonSet_Client.Id; ";
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
                        object id = reader.GetValue(5);
                        clients.Add(new Client()
                    {
                        FirstName = Convert.ToString(firstname),
                        MiddleName = Convert.ToString(middlename),
                        LastName = Convert.ToString(lastname),
                        Phone = Convert.ToString(phone),
                            Email = Convert.ToString(email),
                            Id = Convert.ToInt32(id)
                        });
                }
            }

            reader.Close();
                connection.Close();
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
            string id =Convert.ToString(clients[i].Id);
            string supquery = String.Format("select Address_City as 'Город', Address_Street as 'Улица', Address_House as 'Дом', Address_Number as 'Квартира', Price as 'Цена', concat(c.FirstName, ' ', c.MiddleName, ' ', c.LastName) as 'Клиент', concat(a.FirstName, ' ', a.MiddleName, ' ', a.LastName) as 'Риэлтор' from PersonSet as c, PersonSet as a, PersonSet_Client, PersonSet_Agent, SupplySet, RealEstateSet where c.Id = PersonSet_Client.Id and a.Id = PersonSet_Agent.Id and PersonSet_Client.Id = ClientId and PersonSet_Agent.Id = AgentId and RealEstateSet.Id = RealEstateId and c.Id=" + id +";");
            string demquery = String.Format("select distinct (case when DemandSet.Id =af.Id  then 'Квартира' when DemandSet.Id = hf.Id  then 'Дом' when DemandSet.Id = lf.Id then 'Земля' End) as 'Тип', Address_City as 'Город', concat(c.FirstName, ' ', c.MiddleName, ' ', c.LastName) as 'Клиент', concat(a.FirstName, ' ', a.MiddleName, ' ', a.LastName) as 'Риэлтор', MaxPrice as 'Максимальная цена' from PersonSet as c, PersonSet as a, PersonSet_Client, PersonSet_Agent, DemandSet, RealEstateSet_Apartment as af, RealEstateSet_House as hf, RealEstateSet_Land as lf where c.Id = PersonSet_Client.Id and a.Id = PersonSet_Agent.Id and PersonSet_Client.Id = ClientId and PersonSet_Agent.Id = AgentId and (case when DemandSet.Id = af.Id  then 'Apartment' when DemandSet.Id = hf.Id  then 'House' when DemandSet.Id = lf.Id then 'Land' End) is not null and c.Id=" + id + ";");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlDataAdapter sup = new SqlDataAdapter(
                                supquery, connection))
                {
                    // 3
                    // Use DataAdapter to fill DataTable
                    DataTable s = new DataTable();
                    sup.Fill(s);
                    DataView ss = s.DefaultView;
                    // 4
                    // Render data onto the screen
                    supplies_datagrid.ItemsSource = ss;
                }
                using (SqlDataAdapter dem = new SqlDataAdapter(
                demquery, connection))
                {
                    // 3
                    // Use DataAdapter to fill DataTable
                    DataTable d = new DataTable();
                    dem.Fill(d);
                    DataView dd = d.DefaultView;
                    // 4
                    // Render data onto the screen
                    demands_datagrid.ItemsSource = dd;
                }
                connection.Close();
            }

        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            new AddClientWindow().Show();

        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            int i = clients_listbox.SelectedIndex;
            string id_del = Convert.ToString(clients[i].Id);
            string query1 = String.Format("DELETE from PersonSet where Id='"+id_del+"';");
            string query2 = String.Format("DELETE from PersonSet_Client where Id='" + id_del + "';");
            string query = String.Format(query1 + query2);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            MessageBox.Show("Клиент удален!");
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
    }

        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            firstname_textbox.IsEnabled = true;
            middlename_textbox.IsEnabled = true;
            lastname_textbox.IsEnabled = true;
            phone_textbox.IsEnabled = true;
            email_textbox.IsEnabled = true;
            save_button.IsEnabled = true;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            int i = clients_listbox.SelectedIndex;
            string firstname_edit = firstname_textbox.Text;
            string middlename_edit = middlename_textbox.Text;
            string lastname_edit = lastname_textbox.Text;
            string phone_edit = phone_textbox.Text;
            string email_edit = email_textbox.Text;
            string id_edit = Convert.ToString(clients[i].Id);
            string query1 = String.Format("Update PersonSet Set FirstName = '"+ firstname_edit + "', MiddleName = '" + middlename_edit + "',LastName = '" + lastname_edit + "' where Id = '" + id_edit + "';" );
            string query2 = String.Format("UPDATE PersonSet_Client SET Phone = '" + phone_edit + "', Email = '" + email_edit + "'where Id = '" + id_edit + "'; ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query1, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                connection.Open();
                SqlCommand command1 = new SqlCommand(query2, connection);
                SqlDataReader reader1 = command1.ExecuteReader();
                connection.Close();
            }
            MessageBox.Show("Изменения были успешно сохранены!");
            firstname_textbox.IsEnabled = false;
            middlename_textbox.IsEnabled = false;
            lastname_textbox.IsEnabled = false;
            phone_textbox.IsEnabled = false;
            email_textbox.IsEnabled = false;
            save_button.IsEnabled = false;
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new ClientsWindow().Show();
        }
    }
}
