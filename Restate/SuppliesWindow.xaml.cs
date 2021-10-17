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
    /// Логика взаимодействия для SuppliesWindow.xaml
    /// </summary>
    public partial class SuppliesWindow : Window
    {
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        List<Client> clients = new List<Client>();
        List<Agent> agents = new List<Agent>();
        List<String> types = new List<String>() { "Земельный участок", "Дом", "Квартира" };
        List<Supply> supplies = new List<Supply>();
        List<Object> restates = new List<Object>();
        public SuppliesWindow()
        {
            InitializeComponent();
            var persons = App.Context.PersonSet.ToList();
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            var restate_db = App.Context.RealEstateSet.ToList();
            var land_db = App.Context.RealEstateSet_Land.ToList();
            var house_db = App.Context.RealEstateSet_House.ToList();
            var apartment_db = App.Context.RealEstateSet_Apartment.ToList();
            for (int i = 0; i < persons.Count; i++)
            {
                for (int c = 0; c < clients_db.Count; c++)
                {
                    if (persons[i].Id == clients_db[c].Id)
                    {
                        clients.Add(new Client()
                        {
                            FirstName = persons[i].FirstName,
                            MiddleName = persons[i].MiddleName,
                            LastName = persons[i].LastName,
                            Phone = clients_db[c].Phone,
                            Email = clients_db[c].Email,
                            Id = persons[i].Id
                        });

                    }
                }
                for (int a = 0; a < agents_db.Count; a++)
                {
                    if (persons[i].Id == agents_db[a].Id)
                    {

                        agents.Add(new Agent()
                        {
                            FirstName = persons[i].FirstName,
                            MiddleName = persons[i].MiddleName,
                            LastName = persons[i].LastName,
                            DealShare = agents_db[a].DealShare,
                            Id = persons[i].Id

                        });
                    }
                }
            }
            for (int j = 0; j < restate_db.Count; j++)
            {
                restates.Add(new Object()
                {
                    City = restate_db[j].Address_City,
                    Street = restate_db[j].Address_Street,
                    House = restate_db[j].Address_House,
                    Number = restate_db[j].Address_Number,
                    Latitude = Convert.ToInt32(restate_db[j].Coordinate_latitude),
                    Longitude = Convert.ToInt32(restate_db[j].Coordinate_longitude),
                    Id = restate_db[j].Id
                });
            }
            for (int i = 0; i < restates.Count; i++)
            {
                for (int l = 0; l < land_db.Count; l++)
                    if (restates[i].Id == land_db[l].Id) restates[i].Type = "Земельный участок";
                for (int a = 0; a < apartment_db.Count; a++)
                {
                    if (restates[i].Id == apartment_db[a].Id) restates[i].Type = "Квартира";
                }
                for (int h = 0; h < house_db.Count; h++)
                {
                    if (restates[i].Id == house_db[h].Id) restates[i].Type = "Дом";
                }
            }

            for (int s = 0; s < sup_db.Count; s++)
            {
                for (int c = 0; c < clients.Count; c++)
                {
                    for (int a = 0; a < agents.Count; a++)
                    {
                        for (int r = 0; r < restates.Count; r++)
                        {
                            if (sup_db[s].ClientId == clients[c].Id && sup_db[s].AgentId == agents[a].Id && sup_db[s].RealEstateId == restates[r].Id)
                            {
                                supplies.Add(new Supply()
                                {
                                    Id = sup_db[s].Id,
                                    Type = restates[r].Type,
                                    City = restates[r].City,
                                    Street = restates[r].Street,
                                    House = restates[r].House,
                                    Number = restates[r].Number,
                                    Client = String.Format(clients[c].FirstName + ' ' + clients[c].MiddleName + ' ' + clients[c].LastName),
                                    Agent = String.Format(agents[a].FirstName + ' ' + agents[a].MiddleName + ' ' + agents[a].LastName),
                                    Price = sup_db[s].Price
                                });
                            }
                        }
                    }
                }
            }
            clientBox.ItemsSource = clients;
            _clientbox.ItemsSource = clients;
            agentBox.ItemsSource = agents;
            _agentbox.ItemsSource = agents;
            restateBox.ItemsSource = types;
            _typebox.ItemsSource = types;
            _addressbox.ItemsSource = restates;
            sup_datagrid.ItemsSource = supplies;

        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sup_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            int i = sup_datagrid.SelectedIndex;
            _price_textbox.Text = Convert.ToString(supplies[i].Price);
            for (int _c=0;_c<clients_db.Count;_c++)
            {
                if (clients_db[_c].Id == sup_db[i].ClientId )
                    _clientbox.SelectedIndex = _c;
            }
            for (int _a = 0; _a < agents_db.Count; _a++)
            {
                if (agents_db[_a].Id == sup_db[i].AgentId)
                    _agentbox.SelectedIndex = _a;
            }
            int _r_last = sup_db[i].RealEstateId;
            for (int _r = 0; _r < _r_last; _r++)
            {
                if (restates[_r].Id == _r_last)
                {
                    _addressbox.SelectedIndex = _r;
                }
            }

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
        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            sup_datagrid.ItemsSource = supplies;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var restates_db = App.Context.RealEstateSet.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            double price = 0;
            int c, a, r;
            int clientid = 0, agentid = 0, restateid = 0;
            int i = sup_datagrid.SelectedIndex;
            if (add_radio.IsChecked == true)
            {
                if (_price_textbox.Text != "") price = Convert.ToDouble(_price_textbox.Text);
                else MessageBox.Show("Вы не указали цену!");
                if (_clientbox.SelectedItem != null)
                {
                    c = _clientbox.SelectedIndex;
                    clientid = clients_db[c].Id;
                }
                else MessageBox.Show("Вы не выбрали клиента!");
                if (_agentbox.SelectedItem != null)
                {
                    a = _agentbox.SelectedIndex;
                    agentid = agents_db[a].Id;
                }
                else MessageBox.Show("Вы не выбрали риэлтора!");
                if (_addressbox.SelectedItem != null)
                {
                    r = _addressbox.SelectedIndex;
                    restateid = restates_db[r].Id;
                }
                else MessageBox.Show("Вы не выбрали адрес объекта недвижимости!");
                if (_addressbox.SelectedItem != null && _agentbox.SelectedItem != null && _clientbox.SelectedItem != null && _price_textbox.Text != "")
                {
                    string query = String.Format("INSERT INTO SupplySet VALUES('" + price + "', '" + agentid + "','" + clientid + "', '" + restateid + "');");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                    }
                    MessageBox.Show("Предложение успешно создано!");
                }
            }
                if (edit_radio.IsChecked == true)
                {
                    price = Convert.ToDouble(_price_textbox.Text);
                    c = _clientbox.SelectedIndex;
                    a = _agentbox.SelectedIndex;
                    r = _addressbox.SelectedIndex;
                    int sup_id = sup_db[i].Id;
                    int c_id = clients[c].Id;
                    int a_id = agents[a].Id;
                    int r_id = restates[r].Id;
                    string query = String.Format("UPDATE SupplySet SET Price = '" + price + "', AgentId ='" + a_id + "', ClientId = '" + c_id + "', RealEstateId ='" + r_id + "' where Id = '" + sup_id + "'; ");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                    }
                    MessageBox.Show("Изменения были успешно сохранены!");
                }
                    save_button.IsEnabled = false;
                sup_datagrid.IsEnabled = true;
                view_radio.IsChecked = true;
                Close();
                new SuppliesWindow().Show();
        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            var sup_db = App.Context.SupplySet.ToList();
            if (sup_datagrid.SelectedItem != null)
            {
                int i = sup_datagrid.SelectedIndex;
                int sup_id = sup_db[i].Id;
                string query = String.Format("DELETE from SupplySet where Id='" + sup_id + "';");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                MessageBox.Show("Предложение удалено!");
                Close();
                new SuppliesWindow().Show();
            }
            else MessageBox.Show("Выберите запись в таблице!");
        }
        private void edit_radio_Click(object sender, RoutedEventArgs e)
        {
            save_button.IsEnabled = true;
            _clientbox.IsEnabled = true;
            _agentbox.IsEnabled = true;
            _addressbox.IsEnabled = true;
            _price_textbox.IsEnabled = true;

        }
        private void add_radio_Click(object sender, RoutedEventArgs e)
        {
            _typebox.SelectedItem = null;
            sup_datagrid.IsEnabled = false;
            save_button.IsEnabled = true;
            _clientbox.IsEnabled = true;
            _clientbox.SelectedItem = null;
            _agentbox.IsEnabled = true;
            _agentbox.SelectedItem = null;
            _addressbox.IsEnabled = true;
            _addressbox.SelectedItem = null;
            _price_textbox.IsEnabled = true;
            _price_textbox.Text = "";
            _city_textbox.Text = "";
            _street_textbox.Text = "";
            _house_textbox.Text = "";
            _number_textbox.Text = "";
            latitude_textbox.Text = "";
            longitude_textbox.Text = "";
            totalarea_textbox.Text = "";
            room_textbox.Text = "";
            floor_textbox.Text = "";
        }
        private void view_radio_Click(object sender, RoutedEventArgs e)
        {
            sup_datagrid.IsEnabled = true;
            save_button.IsEnabled = false;
            _clientbox.IsEnabled = false;
            _agentbox.IsEnabled = false;
            _addressbox.IsEnabled = false;
            _price_textbox.IsEnabled = false;
        }
        private void _addressbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = _addressbox.SelectedIndex;
            var restate_db = App.Context.RealEstateSet.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            var land_db = App.Context.RealEstateSet_Land.ToList();
            var house_db = App.Context.RealEstateSet_House.ToList();
            var apartment_db = App.Context.RealEstateSet_Apartment.ToList();
            floor_textbox.Text = "";
            room_textbox.Text = "";
            if (_addressbox.SelectedItem != null)
            {
                _typebox.SelectedItem = restates[i].Type;
                _city_textbox.Text = restates[i].City;
                _street_textbox.Text = restates[i].Street;
                _house_textbox.Text = restates[i].House;
                _number_textbox.Text = restates[i].Number;
                int _r_last = restate_db[i].Id;
                for (int _r = 0; _r < _r_last; _r++)
                {
                    if (restates[i].Id == _r_last)
                    {
                        latitude_textbox.Text = Convert.ToString(restates[_r].Latitude);
                        longitude_textbox.Text = Convert.ToString(restates[_r].Longitude);
                    }
                }

                for (int l = 0; l < land_db.Count; l++)
                    if (restates[i].Id == land_db[l].Id) totalarea_textbox.Text = Convert.ToString(land_db[l].TotalArea);

                for (int h = 0; h < house_db.Count; h++)
                {
                    if (restates[i].Id == house_db[h].Id)
                    {
                        totalarea_textbox.Text = Convert.ToString(house_db[h].TotalArea);
                        floor_textbox.Text = Convert.ToString(house_db[h].TotalFloors);
                    }
                }

                for (int a = 0; a < apartment_db.Count; a++)
                {
                    if (restates[i].Id == apartment_db[a].Id)
                    {
                        totalarea_textbox.Text = Convert.ToString(apartment_db[a].TotalArea);
                        floor_textbox.Text = Convert.ToString(apartment_db[a].Floor);
                        room_textbox.Text = Convert.ToString(apartment_db[a].Rooms);
                    }
                }
            }
        }

        private void restateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = Convert.ToString(restateBox.SelectedItem);
            var filtered = supplies.Where(supply => supply.Type.StartsWith(type));
            sup_datagrid.ItemsSource = filtered;
        }

        private void clientBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string client = clients[clientBox.SelectedIndex].FirstName;
            var filtered = supplies.Where(supply => supply.Client.StartsWith(client));
            sup_datagrid.ItemsSource = filtered;
        }

        private void agentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string agent = agents[agentBox.SelectedIndex].FirstName;
            var filtered = supplies.Where(supply => supply.Agent.StartsWith(agent));
            sup_datagrid.ItemsSource = filtered;
        }

        private void city_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (city_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.City.StartsWith(city_textbox.Text));
                sup_datagrid.ItemsSource = filtered;
            }
        }

        private void street_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (street_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.Street.StartsWith(street_textbox.Text));
                sup_datagrid.ItemsSource = filtered;
            }
        }

        private void house_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (house_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.House ==(house_textbox.Text));
                sup_datagrid.ItemsSource = filtered;
            }
        }

        private void number_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (number_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.Number == (number_textbox.Text));
                sup_datagrid.ItemsSource = filtered;
            }
        }

        private void min_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (min_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.Price > (Convert.ToInt32(min_textbox.Text)));
                sup_datagrid.ItemsSource = filtered;
            }
        }

        private void max_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (max_textbox.Text != "")
            {
                var filtered = supplies.Where(supply => supply.Price < (Convert.ToInt32(max_textbox.Text)));
                sup_datagrid.ItemsSource = filtered;
            }
        }
    }
}
