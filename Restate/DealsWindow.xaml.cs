using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Логика взаимодействия для DealsWindow.xaml
    /// </summary>
    public partial class DealsWindow : Window
    {
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        List<Client> clients = new List<Client>();
        List<Agent> agents = new List<Agent>();
        List<String> types = new List<String>() { "Земельный участок", "Дом", "Квартира" };
        List<Demand> demands = new List<Demand>();
        List<Supply> supplies = new List<Supply>();
        List<Object> restates = new List<Object>();
        List<Deal> deals = new List<Deal>();
        public DealsWindow()
        {
            InitializeComponent();
            var persons = App.Context.PersonSet.ToList();
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            var land_db = App.Context.RealEstateFilterSet_LandFilter.ToList();
            var house_db = App.Context.RealEstateFilterSet_HouseFilter.ToList();
            var apartment_db = App.Context.RealEstateFilterSet_ApartmentFilter.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            var deal_db = App.Context.DealSet.ToList();
            var restate_db = App.Context.RealEstateSet.ToList();
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
            for (int d = 0; d < dem_db.Count; d++)
            {
                for (int c = 0; c < clients.Count; c++)
                {
                    for (int a = 0; a < agents.Count; a++)

                    {
                        if (dem_db[d].ClientId == clients[c].Id && dem_db[d].AgentId == agents[a].Id)
                        {
                            if (dem_db[d].Address_City == null) dem_db[d].Address_City = "";
                            if (dem_db[d].Address_Street == null) dem_db[d].Address_Street = "";
                            if (dem_db[d].Address_House == null) dem_db[d].Address_House = "";
                            if (dem_db[d].Address_Number == null) dem_db[d].Address_Number = "";
                            if (dem_db[d].MaxPrice == null) dem_db[d].MaxPrice = 0;
                            if (dem_db[d].MinPrice == null) dem_db[d].MinPrice = 0;
                            demands.Add(new Demand()
                            {
                                Id = dem_db[d].Id,
                                City = dem_db[d].Address_City,
                                Street = dem_db[d].Address_Street,
                                House = dem_db[d].Address_House,
                                Number = dem_db[d].Address_Number,
                                Client = String.Format(clients[c].FirstName + ' ' + clients[c].MiddleName + ' ' + clients[c].LastName),
                                Agent = String.Format(agents[a].FirstName + ' ' + agents[a].MiddleName + ' ' + agents[a].LastName),
                                MaxPrice = (long)dem_db[d].MaxPrice,
                                MinPrice = (long)dem_db[d].MinPrice
                            });

                        }
                    }
                }
            }
            for (int d = 0; d < dem_db.Count; d++)
            {
                for (int l = 0; l < land_db.Count; l++)
                    if (dem_db[d].RealEstateFilter_Id == land_db[l].Id) demands[d].Type = "Земельный участок";
                for (int ap = 0; ap < apartment_db.Count; ap++)
                {
                    if (dem_db[d].RealEstateFilter_Id == apartment_db[ap].Id) demands[d].Type = "Квартира";
                }
                for (int h = 0; h < house_db.Count; h++)
                {
                    if (dem_db[d].RealEstateFilter_Id == house_db[h].Id) demands[d].Type = "Дом";
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
            for (int dl = 0; dl < deal_db.Count; dl++)
            {
                for (int d = 0; d < demands.Count; d++)
                {
                    for (int s = 0; s < supplies.Count; s++)
                    {
                        if (deal_db[dl].Demand_Id == dem_db[d].Id && deal_db[dl].Supply_Id == sup_db[s].Id)
                        {
                            deals.Add(new Deal()
                            {
                                Id = deal_db[dl].Id,
                                //Customer = String.Format(clients[cc].FirstName + ' ' + clients[cc].MiddleName + ' ' + clients[cc].LastName),
                                //Seller = String.Format(clients[cs].FirstName + ' ' + clients[cs].MiddleName + ' ' + clients[cs].LastName),
                                DemandId = demands[d].Id,
                                SupplyId = supplies[s].Id
                            });
                            for (int c = 0; c < clients.Count; c++)
                            {
                                if (clients[c].Id == dem_db[d].ClientId) deals[dl].Customer = String.Format(clients[c].FirstName + ' ' + clients[c].MiddleName + ' ' + clients[c].LastName);
                                if (clients[c].Id == sup_db[s].ClientId) deals[dl].Seller = String.Format(clients[c].FirstName + ' ' + clients[c].MiddleName + ' ' + clients[c].LastName);
                            }

                        }

                    }
                }
            }
            dem_clientbox.ItemsSource = clients;
            sup_clientbox.ItemsSource = clients;
            dem_agentbox.ItemsSource = agents;
            sup_agentbox.ItemsSource = agents;
            dem_typebox.ItemsSource = types;
            dembox.ItemsSource = demands;
            supbox.ItemsSource = supplies;
            sup_restatebox.ItemsSource = restates;
            deals_listbox.ItemsSource = deals;
        }


        private void add_button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            var deal_db = App.Context.DealSet.ToList();
            if (deals_listbox.SelectedItem != null)
            {
                int i = deals_listbox.SelectedIndex;
                int deal_id = deal_db[i].Id;
                string query = String.Format("DELETE from DealSet where Id='" + deal_id + "';");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                MessageBox.Show("Сделка удалена!");
                Close();
                new DealsWindow().Show();
            }
            else MessageBox.Show("Выберите сделку для удаления!");
        }
        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            supbox.IsEnabled = true;
            dembox.IsEnabled = true;
            save_button.IsEnabled = true;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var restates_db = App.Context.RealEstateSet.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            var sup_db = App.Context.DemandSet.ToList();
            var deal_db = App.Context.DealSet.ToList();
            int demid = 0;
            int supid = 0;
            if (supbox.SelectedItem != null)
            {
                int s = supbox.SelectedIndex;
                supid = sup_db[s].Id;
            }
            else MessageBox.Show("Вы не выбрали предложение!");
            if (dembox.SelectedItem != null)
            {
                int d = dembox.SelectedIndex;
                demid = dem_db[d].Id;
            }
            else MessageBox.Show("Вы не выбрали потребность!");
            if (add_radio.IsChecked == true)
            {
                if (dembox.SelectedItem != null && supbox.SelectedItem != null)
                {
                    string query = String.Format("INSERT INTO DealSet (Demand_Id, Supply_Id) values (" + demid + "," + supid + ");");
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        connection1.Open();
                        SqlCommand command = new SqlCommand(query, connection1);
                        SqlDataReader reader = command.ExecuteReader();
                        connection1.Close();
                    }
                    MessageBox.Show("Сделка успешно создана!");
                    save_button.IsEnabled = false;
                    deals_listbox.IsEnabled = true;
                    view_radio.IsChecked = true;
                    var exe = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(exe);
                    Application.Current.Shutdown();
                }
            }
            if (deals_listbox.SelectedItem != null)
            {
                if (edit_radio.IsChecked == true)
                {
                    int i = deals_listbox.SelectedIndex;
                    int deal_id = deal_db[i].Id;
                    string query = String.Format("UPDATE DealSet SET Demand_Id = " + demid + ", Supply_Id =" + supid + " where Id = " + deal_id + "; ");
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        connection1.Open();
                        SqlCommand command = new SqlCommand(query, connection1);
                        SqlDataReader reader = command.ExecuteReader();
                        connection1.Close();
                    }
                    MessageBox.Show("Изменения были успешно сохранены!");
                    save_button.IsEnabled = false;
                    view_radio.IsChecked = true;
                    var exe = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(exe);
                    Application.Current.Shutdown();
                }
            }
            else MessageBox.Show("Выберите сделку для редактирования!");
        }

        private void deals_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sup_db = App.Context.SupplySet.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            int i = deals_listbox.SelectedIndex;
            for (int s = 0; s < sup_db.Count; s++)
            {
                if (sup_db[s].Id == deals[i].SupplyId)
                    supbox.SelectedIndex = s;
            }
            for (int d = 0; d < dem_db.Count; d++)
            {
                if (dem_db[d].Id == deals[i].DemandId)
                    dembox.SelectedIndex = d;
            }
            int r = sup_restatebox.SelectedIndex;
            int sup_a = sup_agentbox.SelectedIndex;
            int dem_a = dem_agentbox.SelectedIndex;
            double ds_sup = Convert.ToInt32(agents[sup_a].DealShare);
            double ds_dem = Convert.ToInt32(agents[dem_a].DealShare);
            double price = Convert.ToDouble(sup_price.Text);
            double _client_seller = -1, _client_customer = -1, _agent_seller = -1, _agent_customer = -1;
            if (restates[r].Type == "Земельный участок") _client_seller = price * 0.02 + 30000;
            if (restates[r].Type == "Дом") _client_seller = price * 0.01 + 30000;
            if (restates[r].Type == "Квартира") _client_seller = price * 0.01 + 36000;
            client_seller.Text = Convert.ToString(_client_seller);
            _client_customer = price * 0.03;
            client_customer.Text = Convert.ToString(_client_customer);
            _agent_seller = _client_seller * (ds_sup / 100);
            _agent_customer = _client_customer * (ds_dem / 100);
            agent_seller.Text = Convert.ToString(_agent_seller);
            agent_customer.Text = Convert.ToString(_agent_customer);
            company_gain.Text = Convert.ToString((_client_customer - _agent_customer) + (_client_seller - _agent_seller));
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

        private void dembox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            var land_db = App.Context.RealEstateFilterSet_LandFilter.ToList();
            var house_db = App.Context.RealEstateFilterSet_HouseFilter.ToList();
            var apartment_db = App.Context.RealEstateFilterSet_ApartmentFilter.ToList();
            int i = dembox.SelectedIndex;
            dem_maxfloor.Text = "";
            dem_minroom.Text = "";
            dem_minfloor.Text = "";
            dem_maxroom.Text = "";
            dem_maxarea.Text = "";
            dem_minarea.Text = "";
            if (dembox.SelectedItem != null && dembox.SelectedIndex < dembox.Items.Count)
            {
                for (int _c = 0; _c < clients_db.Count; _c++)
                {
                    if (clients_db[_c].Id == dem_db[i].ClientId)
                        dem_clientbox.SelectedIndex = _c;
                }
                for (int _a = 0; _a < agents_db.Count; _a++)
                {
                    if (agents_db[_a].Id == dem_db[i].AgentId)
                        dem_agentbox.SelectedIndex = _a;
                }

                dem_maxprice.Text = Convert.ToString(demands[i].MaxPrice);
                dem_minprice.Text = Convert.ToString(demands[i].MinPrice);
                dem_typebox.SelectedItem = demands[i].Type;
                dem_city.Text = demands[i].City;
                dem_street.Text = demands[i].Street;
                dem_house.Text = demands[i].House;
                dem_number.Text = demands[i].Number;

                for (int l = 0; l < land_db.Count; l++)
                    if (dem_db[i].RealEstateFilter_Id == land_db[l].Id)
                    {
                        dem_maxarea.Text = Convert.ToString(land_db[l].MaxArea);
                        dem_minarea.Text = Convert.ToString(land_db[l].MinArea);
                    }


                for (int h = 0; h < house_db.Count; h++)
                {
                    if (dem_db[i].RealEstateFilter_Id == house_db[h].Id)
                    {
                        dem_maxarea.Text = Convert.ToString(house_db[h].MaxArea);
                        dem_minarea.Text = Convert.ToString(house_db[h].MinArea);
                        dem_maxfloor.Text = Convert.ToString(house_db[h].MaxFloors);
                        dem_minfloor.Text = Convert.ToString(house_db[h].MinFloors);
                        dem_maxroom.Text = Convert.ToString(house_db[h].MaxRooms);
                        dem_minroom.Text = Convert.ToString(house_db[h].MinRooms);
                    }
                }

                for (int a = 0; a < apartment_db.Count; a++)
                {
                    if (dem_db[i].RealEstateFilter_Id == apartment_db[a].Id)
                    {
                        dem_maxarea.Text = Convert.ToString(apartment_db[a].MaxArea);
                        dem_minarea.Text = Convert.ToString(apartment_db[a].MinArea);
                        dem_maxfloor.Text = Convert.ToString(apartment_db[a].MaxFloor);
                        dem_minfloor.Text = Convert.ToString(apartment_db[a].MinFloor);
                        dem_maxroom.Text = Convert.ToString(apartment_db[a].MaxRooms);
                        dem_minroom.Text = Convert.ToString(apartment_db[a].MinRooms);
                    }
                }
            }
        }

        private void sup_restatebox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var restate_db = App.Context.RealEstateSet.ToList();
            var land_db = App.Context.RealEstateSet_Land.ToList();
            var house_db = App.Context.RealEstateSet_House.ToList();
            var apartment_db = App.Context.RealEstateSet_Apartment.ToList();

            int i = sup_restatebox.SelectedIndex;
            sup_floor.Text = "";
            sup_room.Text = "";
            if (sup_restatebox.SelectedItem != null)
            {
                sup_city.Text = restates[i].City;
                sup_street.Text = restates[i].Street;
                sup_house.Text = restates[i].House;
                sup_number.Text = restates[i].Number;
                int _r_last = restate_db[i].Id;
                for (int _r = 0; _r < _r_last; _r++)
                {
                    if (restates[i].Id == _r_last)
                    {
                        sup_latitude.Text = Convert.ToString(restates[_r].Latitude);
                        sup_longitude.Text = Convert.ToString(restates[_r].Longitude);
                    }
                }

                for (int l = 0; l < land_db.Count; l++)
                    if (restates[i].Id == land_db[l].Id) sup_totalarea.Text = Convert.ToString(land_db[l].TotalArea);

                for (int h = 0; h < house_db.Count; h++)
                {
                    if (restates[i].Id == house_db[h].Id)
                    {
                        sup_totalarea.Text = Convert.ToString(house_db[h].TotalArea);
                        sup_floor.Text = Convert.ToString(house_db[h].TotalFloors);
                    }
                }

                for (int a = 0; a < apartment_db.Count; a++)
                {
                    if (restates[i].Id == apartment_db[a].Id)
                    {
                        sup_totalarea.Text = Convert.ToString(apartment_db[a].TotalArea);
                        sup_floor.Text = Convert.ToString(apartment_db[a].Floor);
                        sup_room.Text = Convert.ToString(apartment_db[a].Rooms);
                    }
                }
            }
        }

        private void supbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var sup_db = App.Context.SupplySet.ToList();
            int i = supbox.SelectedIndex;
            sup_price.Text = Convert.ToString(supplies[i].Price);
            for (int _c = 0; _c < clients_db.Count; _c++)
            {
                if (clients_db[_c].Id == sup_db[i].ClientId)
                    sup_clientbox.SelectedIndex = _c;
            }
            for (int _a = 0; _a < agents_db.Count; _a++)
            {
                if (agents_db[_a].Id == sup_db[i].AgentId)
                    sup_agentbox.SelectedIndex = _a;
            }
            int _r_last = sup_db[i].RealEstateId;
            for (int _r = 0; _r < _r_last; _r++)
            {
                if (restates[_r].Id == _r_last)
                {
                    sup_restatebox.SelectedIndex = _r;
                }
            }
        }

        private void view_radio_Click(object sender, RoutedEventArgs e)
        {
            deals_listbox.IsEnabled = true;
            save_button.IsEnabled = false;
            supbox.IsEnabled = false;
            dembox.IsEnabled = false;
        }
        private void edit_radio_Click(object sender, RoutedEventArgs e)
        {
            deals_listbox.IsEnabled = false;
            save_button.IsEnabled = true;
            supbox.IsEnabled = true;
            dembox.IsEnabled = true;
        }
        private void add_radio_Click(object sender, RoutedEventArgs e)
        {
            deals_listbox.IsEnabled = false;
            save_button.IsEnabled = true;
            supbox.IsEnabled = true;
            dembox.IsEnabled = true;
            supbox.SelectedItem = null;
            dembox.SelectedItem = null;
        }
    }
}
