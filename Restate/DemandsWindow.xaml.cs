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
    /// Логика взаимодействия для DemandsWindow.xaml
    /// </summary>
    public partial class DemandsWindow : Window
    {
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        List<Client> clients = new List<Client>();
        List<Agent> agents = new List<Agent>();
        List<String> types = new List<String>() { "Земельный участок", "Дом", "Квартира" };
        List<Demand> demands = new List<Demand>();
        List<Object> restates = new List<Object>();
        public DemandsWindow()
        {
            InitializeComponent();
            var persons = App.Context.PersonSet.ToList();
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            var land_db = App.Context.RealEstateFilterSet_LandFilter.ToList();
            var house_db = App.Context.RealEstateFilterSet_HouseFilter.ToList();
            var apartment_db = App.Context.RealEstateFilterSet_ApartmentFilter.ToList();
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
                            if(dem_db[d].Address_City == null) dem_db[d].Address_City = "";
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
            clientBox.ItemsSource = clients;
            _clientbox.ItemsSource = clients;
            agentBox.ItemsSource = agents;
            _agentbox.ItemsSource = agents;
            restateBox.ItemsSource = types;
            _typebox.ItemsSource = types;
            dem_datagrid.ItemsSource = demands;

        }

        private void dem_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            var land_db = App.Context.RealEstateFilterSet_LandFilter.ToList();
            var house_db = App.Context.RealEstateFilterSet_HouseFilter.ToList();
            var apartment_db = App.Context.RealEstateFilterSet_ApartmentFilter.ToList();
            int i = dem_datagrid.SelectedIndex;
            maxfloor_textbox.Text = "";
            minroom_textbox.Text = "";
            minfloor_textbox.Text = "";
            maxroom_textbox.Text = "";
            maxtotalarea_textbox.Text = "";
            mintotalarea_textbox.Text = "";
            if (dem_datagrid.SelectedItem != null && dem_datagrid.SelectedIndex < dem_datagrid.Items.Count)
            {
                for (int _c = 0; _c < clients_db.Count; _c++)
                {
                    if (clients_db[_c].Id == dem_db[i].ClientId)
                        _clientbox.SelectedIndex = _c;
                }
                for (int _a = 0; _a < agents_db.Count; _a++)
                {
                    if (agents_db[_a].Id == dem_db[i].AgentId)
                        _agentbox.SelectedIndex = _a;
                }

                    _maxprice_textbox.Text = Convert.ToString(demands[i].MaxPrice);
                    _minprice_textbox.Text = Convert.ToString(demands[i].MinPrice);
                    _typebox.SelectedItem = demands[i].Type;
                    _city_textbox.Text = demands[i].City;
                    _street_textbox.Text = demands[i].Street;
                    _house_textbox.Text = demands[i].House;
                    _number_textbox.Text = demands[i].Number;

                    for (int l = 0; l < land_db.Count; l++)
                        if (dem_db[i].RealEstateFilter_Id == land_db[l].Id)
                        {
                            maxtotalarea_textbox.Text = Convert.ToString(land_db[l].MaxArea);
                            mintotalarea_textbox.Text = Convert.ToString(land_db[l].MinArea);
                        }


                    for (int h = 0; h < house_db.Count; h++)
                    {
                        if (dem_db[i].RealEstateFilter_Id == house_db[h].Id)
                        {
                            maxtotalarea_textbox.Text = Convert.ToString(house_db[h].MaxArea);
                            mintotalarea_textbox.Text = Convert.ToString(house_db[h].MinArea);
                            maxfloor_textbox.Text = Convert.ToString(house_db[h].MaxFloors);
                            minfloor_textbox.Text = Convert.ToString(house_db[h].MinFloors);
                            maxroom_textbox.Text = Convert.ToString(house_db[h].MaxRooms);
                            minroom_textbox.Text = Convert.ToString(house_db[h].MinRooms);
                        }
                    }

                    for (int a = 0; a < apartment_db.Count; a++)
                    {
                        if (dem_db[i].RealEstateFilter_Id == apartment_db[a].Id)
                        {
                            maxtotalarea_textbox.Text = Convert.ToString(apartment_db[a].MaxArea);
                            mintotalarea_textbox.Text = Convert.ToString(apartment_db[a].MinArea);
                            maxfloor_textbox.Text = Convert.ToString(apartment_db[a].MaxFloor);
                            minfloor_textbox.Text = Convert.ToString(apartment_db[a].MinFloor);
                            maxroom_textbox.Text = Convert.ToString(apartment_db[a].MaxRooms);
                            minroom_textbox.Text = Convert.ToString(apartment_db[a].MinRooms);
                        }
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
            dem_datagrid.ItemsSource = demands;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            var clients_db = App.Context.PersonSet_Client.ToList();
            var agents_db = App.Context.PersonSet_Agent.ToList();
            var restates_db = App.Context.RealEstateSet.ToList();
            var dem_db = App.Context.DemandSet.ToList();
            double maxprice = 0;
            double minprice = 0;
            int c, a, r;
            int clientid = 0, agentid = 0, maxarea=0, minarea=0, maxfloor=0, minfloor=0, maxroom=0, minroom=0;
            string city, street, house, number, type="";
            if (_city_textbox.Text != "") city = _city_textbox.Text; else city = "";
            if (_street_textbox.Text != "") street = _street_textbox.Text; else street = "";
            if (_house_textbox.Text != "") house = _house_textbox.Text; else house = "";
            if (_number_textbox.Text != "") number = _number_textbox.Text; else number = "";
            if (maxtotalarea_textbox.Text != "") maxarea = Convert.ToInt32(maxtotalarea_textbox.Text);
            if (mintotalarea_textbox.Text != "") minarea = Convert.ToInt32(mintotalarea_textbox.Text); 
            if (maxfloor_textbox.Text != "") maxfloor = Convert.ToInt32(maxfloor_textbox.Text); 
            if (minfloor_textbox.Text != "") minfloor = Convert.ToInt32(minfloor_textbox.Text); 
            if (maxroom_textbox.Text != "") maxroom = Convert.ToInt32(maxroom_textbox.Text); 
            if (minroom_textbox.Text != "") minroom = Convert.ToInt32(minroom_textbox.Text); 
            string query1 = "",query2 = "", query3 = "", query4 = "", query5 = "";
            string querye1 = " ", querye2 = " ", querye3 = " ", querye4 = " ";
            int i = dem_datagrid.SelectedIndex;
            if (add_radio.IsChecked == true)
            {
                if (_maxprice_textbox.Text != "") maxprice = Convert.ToDouble(_maxprice_textbox.Text);
                else MessageBox.Show("Вы не указали максимальную цену!");
                if (_minprice_textbox.Text != "") minprice = Convert.ToDouble(_minprice_textbox.Text);
                else MessageBox.Show("Вы не указали минимальную цену!");
                if (_city_textbox.Text ==""&& _street_textbox.Text == "" && _house_textbox.Text == "" && _number_textbox.Text == "")
                    MessageBox.Show("Вы совсем не указали желаемый адрес!");
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
                if (_typebox.SelectedItem != null) type = Convert.ToString(_typebox.SelectedItem);
                else MessageBox.Show("Вы не указали тип объекта!");

                if (_typebox.SelectedItem != null && _agentbox.SelectedItem != null && _clientbox.SelectedItem != null && _maxprice_textbox.Text != "" && _minprice_textbox.Text != "" && (_city_textbox.Text != "" || _street_textbox.Text != "" || _house_textbox.Text != "" || _number_textbox.Text != ""))
                {
                    query1 = String.Format("INSERT INTO RealEstateFilterSet (i) values ('');");
                    if (type == "Земельный участок") query2 = String.Format("INSERT INTO RealEstateFilterSet_LandFilter VALUES(" + minarea + "," + maxarea + ",(select max(Id) from RealEstateFilterSet)); ");
                    if (type == "Дом") query3 = String.Format("INSERT INTO RealEstateFilterSet_HouseFilter VALUES(" + minfloor + "," + maxfloor + "," + minarea + "," + maxarea + "," + minroom + "," + maxroom + ",(select max(Id) from RealEstateFilterSet)); ");
                    if (type == "Квартира") query4 = String.Format("INSERT INTO RealEstateFilterSet_ApartmentFilter VALUES(" + minarea + "," + maxarea + "," + minroom + "," + maxroom + "," + minfloor + "," + maxfloor + ",(select max(Id) from RealEstateFilterSet)); ");
                    query5 = String.Format("INSERT INTO DemandSet (Address_City, Address_Street, Address_House, Address_Number, MinPrice, MaxPrice, AgentId, ClientId, RealEstateFilter_Id) values('" + city + "', '" + street + "', '" + house + "', '" + number + "', " + minprice + ", " + maxprice + ", " + agentid + ", " + clientid + ", (select max(Id) from RealEstateFilterSet)); ");
                    string query = String.Format(query1 + query2 + query3 + query4 + query5);
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        connection1.Open();
                        SqlCommand command = new SqlCommand(query, connection1);
                        SqlDataReader reader = command.ExecuteReader();
                        connection1.Close();
                    }
                    MessageBox.Show("Потребность успешно создана!");
                }
            }
            if (edit_radio.IsChecked == true)
            {
                maxprice = Convert.ToDouble(_maxprice_textbox.Text);
                minprice = Convert.ToDouble(_minprice_textbox.Text);
                c = _clientbox.SelectedIndex;
                a = _agentbox.SelectedIndex;
                type = Convert.ToString(_typebox.SelectedItem);
                int dem_id = dem_db[i].Id;
                int c_id = clients[c].Id;
                int a_id = agents[a].Id;
                int r_id = dem_db[i].RealEstateFilter_Id;
                if (type == "Земельный участок") querye1 = String.Format("UPDATE RealEstateFilterSet_LandFilter SET " + "MinArea =" + minarea + ", MaxArea= " + maxarea + "where Id =" + r_id + ";" );
                if (type == "Дом") querye2 = String.Format("UPDATE RealEstateFilterSet_HouseFilter SET " + "MinArea =" + minarea + ", MaxArea= " + maxarea + ", MinFloors =" + minfloor + ", MaxFloors =" + maxfloor + ", MinRooms =" + minroom + ", MaxRooms =" + maxroom + "where Id =" + r_id + ";" );
                if (type == "Квартира") querye3 = String.Format("UPDATE RealEstateFilterSet_ApartmentFilter SET " + "MinArea =" + minarea + ", MaxArea= " + maxarea + ", MinFloor =" + minfloor + ", MaxFloor =" + maxfloor + ", MinRooms =" + minroom + ", MaxRooms =" + maxroom + "where Id =" + r_id + ";" );
                querye4 = String.Format("UPDATE DemandSet SET MaxPrice = " + maxprice + ", MinPrice =" + minprice + ", Address_City ='" + city + "', Address_Street ='" + street + "', Address_House ='" + house + "', Address_Number ='" + number + "', AgentId =" + a_id + ", ClientId = " + c_id + " where Id = " + dem_id + "; " );
                string querye = String.Format(querye1 + querye2 + querye3 + querye4);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(querye, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                MessageBox.Show("Изменения были успешно сохранены!");
            }
            save_button.IsEnabled = false;
            dem_datagrid.IsEnabled = true;
            view_radio.IsChecked = true;
            Close();
            new DemandsWindow().Show();
        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            var dem_db = App.Context.DemandSet.ToList();
            if (dem_datagrid.SelectedItem != null)
            {
                int i = dem_datagrid.SelectedIndex;
                int dem_id = dem_db[i].Id;
                string query = String.Format("DELETE from DemandSet where Id='" + dem_id + "';");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                MessageBox.Show("Потребность удалена!");
                Close();
                new DemandsWindow().Show();
            }
            else MessageBox.Show("Выберите запись в таблице!");
        }
        private void edit_radio_Click(object sender, RoutedEventArgs e)
        {
            save_button.IsEnabled = true;
            _clientbox.IsEnabled = true;
            _agentbox.IsEnabled = true;
            _typebox.IsEnabled = true;
            _maxprice_textbox.IsEnabled = true;
            _minprice_textbox.IsEnabled = true;
            _city_textbox.IsEnabled = true;
            _street_textbox.IsEnabled = true;
            _house_textbox.IsEnabled = true;
            _number_textbox.IsEnabled = true;
            maxtotalarea_textbox.IsEnabled = true;
            mintotalarea_textbox.IsEnabled = true;
            maxfloor_textbox.IsEnabled = true;
            minfloor_textbox.IsEnabled = true;
            minroom_textbox.IsEnabled = true;
            maxroom_textbox.IsEnabled = true;

        }
        private void add_radio_Click(object sender, RoutedEventArgs e)
        {
            dem_datagrid.IsEnabled = false;
            save_button.IsEnabled = true;
            _clientbox.IsEnabled = true;
            _clientbox.SelectedItem = null;
            _agentbox.IsEnabled = true;
            _agentbox.SelectedItem = null;
            _typebox.IsEnabled = true;
            _typebox.SelectedItem = null;
            _maxprice_textbox.IsEnabled = true;
            _minprice_textbox.IsEnabled = true;
            _city_textbox.IsEnabled = true;
            _street_textbox.IsEnabled = true;
            _house_textbox.IsEnabled = true;
            _number_textbox.IsEnabled = true;
            maxtotalarea_textbox.IsEnabled = true;
            mintotalarea_textbox.IsEnabled = true;
            maxfloor_textbox.IsEnabled = true;
            minfloor_textbox.IsEnabled = true;
            minroom_textbox.IsEnabled = true;
            maxroom_textbox.IsEnabled = true;
            _maxprice_textbox.Text = "";
            _minprice_textbox.Text = "";
            _city_textbox.Text = "";
            _street_textbox.Text = "";
            _house_textbox.Text = "";
            _number_textbox.Text = "";
            maxtotalarea_textbox.Text = "";
            mintotalarea_textbox.Text = "";
            maxroom_textbox.Text = "";
            maxfloor_textbox.Text = "";
            minroom_textbox.Text = "";
            minfloor_textbox.Text = "";
        }
        private void view_radio_Click(object sender, RoutedEventArgs e)
        {
            dem_datagrid.IsEnabled = true;
            save_button.IsEnabled = false;
            _clientbox.IsEnabled = false;
            _agentbox.IsEnabled = false;
            _typebox.IsEnabled = false;
            _maxprice_textbox.IsEnabled = false;
            _minprice_textbox.IsEnabled = false;
            _city_textbox.IsEnabled = false;
            _street_textbox.IsEnabled = false;
            _house_textbox.IsEnabled = false;
            _number_textbox.IsEnabled = false;
            maxtotalarea_textbox.IsEnabled = false;
            mintotalarea_textbox.IsEnabled = false;
            maxfloor_textbox.IsEnabled = false;
            minfloor_textbox.IsEnabled = false;
            minroom_textbox.IsEnabled = false;
            maxroom_textbox.IsEnabled = false;
        }
        private void restateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = Convert.ToString(restateBox.SelectedItem);
            var filtered = demands.Where(demand => demand.Type.StartsWith(type));
            dem_datagrid.ItemsSource = filtered;
        }

        private void clientBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string client = clients[clientBox.SelectedIndex].FirstName;
            var filtered = demands.Where(demand => demand.Client.StartsWith(client));
            dem_datagrid.ItemsSource = filtered;
        }

        private void agentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string agent = agents[agentBox.SelectedIndex].FirstName;
            var filtered = demands.Where(demand => demand.Agent.StartsWith(agent));
            dem_datagrid.ItemsSource = filtered;
        }

        private void city_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (city_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.City.StartsWith(city_textbox.Text));
                dem_datagrid.ItemsSource = filtered;
            }
        }

        private void street_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (street_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.Street.StartsWith(street_textbox.Text));
                dem_datagrid.ItemsSource = filtered;
            }
        }

        private void house_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (house_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.House == (house_textbox.Text));
                dem_datagrid.ItemsSource = filtered;
            }
        }

        private void number_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (number_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.Number == (number_textbox.Text));
                dem_datagrid.ItemsSource = filtered;
            }
        }

        private void min_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (min_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.MinPrice == (Convert.ToInt32(min_textbox.Text)));
                dem_datagrid.ItemsSource = filtered;
            }
        }

        private void max_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (max_textbox.Text != "")
            {
                var filtered = demands.Where(demand => demand.MaxPrice == (Convert.ToInt32(max_textbox.Text)));
                dem_datagrid.ItemsSource = filtered;
            }
        }
    }
}
