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
        List<ClientNames> c_names = new List<ClientNames>();
        List<Agent> agents = new List<Agent>();
        List<AgentNames> a_names = new List<AgentNames>();
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
                        c_names.Add(new ClientNames()
                        {
                            Name = String.Format(persons[i].FirstName + persons[i].MiddleName + persons[i].LastName)
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
                        a_names.Add(new AgentNames()
                        {
                            Name = String.Format(persons[i].FirstName + persons[i].MiddleName + persons[i].LastName)
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
            }
            for (int i = 0; i < restates.Count; i++)
                for (int h = 0; h < house_db.Count; h++)
                {
                    if (restates[i].Id == house_db[h].Id) restates[i].Type = "Дом";
                }
            for (int i = 0; i < restates.Count; i++)
                for (int a = 0; a < apartment_db.Count; a++)
                {
                    if (restates[i].Id == apartment_db[a].Id) restates[i].Type = "Квартира";
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
                                    IdRestate = restates[r].Id,
                                    Type = restates[r].Type,
                                    City = restates[r].City,
                                    Street = restates[r].Street,
                                    House = restates[r].House,
                                    Number = restates[r].Number,
                                    Client = String.Format (clients[c].FirstName + ' ' + clients[c].MiddleName + ' ' + clients[c].LastName),
                                    Agent = String.Format(agents[a].FirstName + ' ' + agents[a].MiddleName + ' ' + agents[a].LastName),
                                    Price = sup_db[s].Price
                                });
                            }
                         }
                    }
                }
            }
            clientBox.ItemsSource = c_names;
            _clientbox.ItemsSource = c_names;
            agentBox.ItemsSource = a_names;
            _agentbox.ItemsSource = a_names;
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
            var land_db = App.Context.RealEstateSet_Land.ToList();
            var house_db = App.Context.RealEstateSet_House.ToList();
            var apartment_db = App.Context.RealEstateSet_Apartment.ToList();
            int i = sup_datagrid.SelectedIndex;
            _typebox.SelectedItem = supplies[i].Type;
            _clientbox.SelectedItem = supplies[i].Client;
            _agentbox.SelectedItem = supplies[i].Agent;
            _city_textbox.Text = supplies[i].City;
            _street_textbox.Text = supplies[i].Street;
            _house_textbox.Text = supplies[i].House;
            _number_textbox.Text = supplies[i].Number;
            for (int j=0;j<restates.Count;j++)
            {
                if (restates[j].Id == supplies[i].IdRestate)
                {
                    latitude_textbox.Text = Convert.ToString(restates[j].Latitude);
                    longitude_textbox.Text = Convert.ToString(restates[j].Longitude);
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
            Close();
            new SuppliesWindow().Show();
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
