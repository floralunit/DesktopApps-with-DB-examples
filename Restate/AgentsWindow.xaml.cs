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
    /// Логика взаимодействия для AgentsWindow.xaml
    /// </summary>
    public partial class AgentsWindow : Window
    {
        List<Agent> agents = new List<Agent>();
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        public AgentsWindow()
        {
            InitializeComponent();
            //var agents = App.Context.PersonSet_Agent.ToList();
            //agents_listbox.ItemsSource = agents;

            string sqlExpression =
                "select FirstName, MiddleName, LastName, DealShare, PersonSet.Id from PersonSet, PersonSet_Agent where PersonSet.Id = PersonSet_Agent.Id; ";
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
                        object dealshare = reader.GetValue(3);
                        object id = reader.GetValue(4);
                        agents.Add(new Agent() { FirstName = Convert.ToString(firstname),
                            MiddleName = Convert.ToString(middlename),
                            LastName = Convert.ToString(lastname),
                            DealShare = Convert.ToInt32(dealshare),
                            Id = Convert.ToInt32(id)
                        });
                    }
                }

                reader.Close();
                connection.Close();
            }
            agents_listbox.ItemsSource = agents;

        }


        private void agents_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = agents_listbox.SelectedIndex;
            firstname_textbox.Text = agents[i].FirstName;
            middlename_textbox.Text = agents[i].MiddleName;
            lastname_textbox.Text = agents[i].LastName;
            dealshare_textbox.Text = Convert.ToString(agents[i].DealShare);
            string id = Convert.ToString(agents[i].Id);
            string supquery = String.Format("select Address_City as 'Город', Address_Street as 'Улица', Address_House as 'Дом', Address_Number as 'Квартира', Price as 'Цена', concat(c.FirstName, ' ', c.MiddleName, ' ', c.LastName) as 'Клиент', concat(a.FirstName, ' ', a.MiddleName, ' ', a.LastName) as 'Риэлтор' from PersonSet as c, PersonSet as a, PersonSet_Client, PersonSet_Agent, SupplySet, RealEstateSet where c.Id = PersonSet_Client.Id and a.Id = PersonSet_Agent.Id and PersonSet_Client.Id = ClientId and PersonSet_Agent.Id = AgentId and RealEstateSet.Id = RealEstateId and a.Id=" + id + ";");
            string demquery = String.Format("select distinct (case when DemandSet.Id =af.Id  then 'Квартира' when DemandSet.Id = hf.Id  then 'Дом' when DemandSet.Id = lf.Id then 'Земля' End) as 'Тип', Address_City as 'Город', concat(c.FirstName, ' ', c.MiddleName, ' ', c.LastName) as 'Клиент', concat(a.FirstName, ' ', a.MiddleName, ' ', a.LastName) as 'Риэлтор', MaxPrice as 'Максимальная цена' from PersonSet as c, PersonSet as a, PersonSet_Client, PersonSet_Agent, DemandSet, RealEstateSet_Apartment as af, RealEstateSet_House as hf, RealEstateSet_Land as lf where c.Id = PersonSet_Client.Id and a.Id = PersonSet_Agent.Id and PersonSet_Client.Id = ClientId and PersonSet_Agent.Id = AgentId and (case when DemandSet.Id = af.Id  then 'Apartment' when DemandSet.Id = hf.Id  then 'House' when DemandSet.Id = lf.Id then 'Land' End) is not null and a.Id=" + id + ";");
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
            new AddAgentWindow().Show();

        }
        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AgentsWindow().Show();
        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            int i = agents_listbox.SelectedIndex;
            string id_del = Convert.ToString(agents[i].Id);
            string query1 = String.Format("DELETE from PersonSet where Id='" + id_del + "';");
            string query2 = String.Format("DELETE from PersonSet_Agent where Id='" + id_del + "';");
            string query = String.Format(query1 + query2);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            MessageBox.Show("Риэлтор удален!");
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
            dealshare_textbox.IsEnabled = true;
            save_button.IsEnabled = true;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            int i = agents_listbox.SelectedIndex;
            string firstname_edit = firstname_textbox.Text;
            string middlename_edit = middlename_textbox.Text;
            string lastname_edit = lastname_textbox.Text;
            string deal_edit = dealshare_textbox.Text;
            string id_edit = Convert.ToString(agents[i].Id);
            string query1 = String.Format("Update PersonSet Set FirstName = '" + firstname_edit + "', MiddleName = '" + middlename_edit + "',LastName = '" + lastname_edit + "' where Id = '" + id_edit + "';");
            string query2 = String.Format("UPDATE PersonSet_Agent SET DealShare = '" + deal_edit + "'where Id = '" + id_edit + "'; ");

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
            dealshare_textbox.IsEnabled = false;
            save_button.IsEnabled = false;
        }
    }
}
