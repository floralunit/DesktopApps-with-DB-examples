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
    /// Логика взаимодействия для RealEstate.xaml
    /// </summary>
    public partial class RealEstate : Window
    {
        List<Object> restates = new List<Object>();
        List<Land> lands = new List<Land>();
        List<Apartment> apartments = new List<Apartment>();
        List<House> houses = new List<House>();
        string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
        public RealEstate()
        {
            InitializeComponent();
            string sqlExpression =
    "select distinct (case when RealEstateSet.Id =af.Id  then 'Квартира' when RealEstateSet.Id = hf.Id  then 'Дом' when RealEstateSet.Id = lf.Id then 'Земля' End) as 'Тип', Address_City, Address_Street,Address_House,Address_Number, Coordinate_latitude,  Coordinate_longitude, RealEstateSet.Id from RealEstateSet_Apartment as af, RealEstateSet_House as hf, RealEstateSet_Land as lf, RealEstateSet where (case when RealEstateSet.Id = af.Id  then 'Apartment' when RealEstateSet.Id = hf.Id  then 'House' when RealEstateSet.Id = lf.Id then 'Land' End) is not null; ";
            string sql_land =
"select TotalArea, Id from RealEstateSet_Land;";
            string sql_house =
"select TotalArea,TotalFloors, Id from RealEstateSet_House;";
            string sql_apartment =
"select TotalArea, Rooms, Floor, Id from RealEstateSet_Apartment;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //запись всех объектов в список
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        object type = reader.GetValue(0);
                        object city = reader.GetValue(1);
                        object street = reader.GetValue(2);
                        object house = reader.GetValue(3);
                        object number = reader.GetValue(4);
                        object latitude = reader.GetValue(5);
                        object longitude = reader.GetValue(6);
                        object id = reader.GetValue(7);
                        restates.Add(new Object()
                        {
                            Type = Convert.ToString(type),
                            City = Convert.ToString(city),
                            Street = Convert.ToString(street),
                            House = Convert.ToString(house),
                            Number = Convert.ToString(number),
                            Latitude = Convert.ToInt32(latitude),
                            Longitude = Convert.ToInt32(longitude),
                            Id = Convert.ToInt32(id)
                        });
                    }
                }
               reader.Close();
                //запись земельных участков в список
                SqlCommand command_land = new SqlCommand(sql_land, connection);
                SqlDataReader reader_land = command_land.ExecuteReader();
                if (reader_land.HasRows) // если есть данные
                {

                    while (reader_land.Read()) // построчно считываем данные
                    {
                        object totalarea = reader_land.GetValue(0);
                        object id_land = reader_land.GetValue(1);
                        lands.Add(new Land()
                        {
                            TotalArea = Convert.ToDouble(totalarea),
                            Id = Convert.ToInt32(id_land)
                        });
                    }
                }
               reader_land.Close();
                //запись домов в список
                SqlCommand command_house = new SqlCommand(sql_house, connection);
                SqlDataReader reader_house = command_house.ExecuteReader();
                if (reader_house.HasRows) // если есть данные
                {

                    while (reader_house.Read()) // построчно считываем данные
                    {
                        object totalarea_house = reader_house.GetValue(0);
                        object totalfloors = reader_house.GetValue(1);
                        object id_house = reader_house.GetValue(2);
                        houses.Add(new House()
                        {
                            TotalArea = Convert.ToDouble(totalarea_house),
                            TotalFloors = Convert.ToInt32(totalfloors),
                            Id = Convert.ToInt32(id_house)
                        });
                    }
                }
                reader_house.Close();
                //запись квартир в список
                SqlCommand command_apartment = new SqlCommand(sql_apartment, connection);
                SqlDataReader reader_apartment = command_apartment.ExecuteReader();
                if (reader_apartment.HasRows) // если есть данные
                {

                    while (reader_apartment.Read()) // построчно считываем данные
                    {
                        object totalarea_apartment = reader_apartment.GetValue(0);
                        object rooms = reader_apartment.GetValue(1);
                        object floor = reader_apartment.GetValue(2);
                        object id_apartment = reader_apartment.GetValue(3);
                        apartments.Add(new Apartment()
                        {
                            TotalArea = Convert.ToDouble(totalarea_apartment),
                            Rooms = Convert.ToInt32(rooms),
                            Floor = Convert.ToInt32(floor),
                            Id = Convert.ToInt32(id_apartment)
                        });
                    }
                }
                reader_apartment.Close();
                connection.Close();
                restate_listbox.ItemsSource = restates;
            }
        }

        private void restate_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = restate_listbox.SelectedIndex;
            city_textbox.Text = null;
            street_textbox.Text = null;
            house_textbox.Text = null;
            number_textbox.Text = null;
            latitude_textbox.Text =null;
            longitude_textbox.Text = null;
            totalarea_textbox.Text = null;
            rooms_textbox.Text = null;
            floor_textbox.Text = null;
            city_textbox.Text = restates[i].City;
            street_textbox.Text = restates[i].Street;
            house_textbox.Text = restates[i].House;
            number_textbox.Text = restates[i].Number;
            latitude_textbox.Text = Convert.ToString(restates[i].Latitude);
            longitude_textbox.Text = Convert.ToString(restates[i].Longitude);
            for (int l=0; l < lands.Count; l++)
            {
                if (lands[l].Id == restates[i].Id)
                    totalarea_textbox.Text = Convert.ToString(lands[l].TotalArea);
            }
            for (int h = 0; h < houses.Count; h++)
            {
                if (houses[h].Id == restates[i].Id)
                {
                    totalarea_textbox.Text = Convert.ToString(houses[h].TotalArea);
                    floor_textbox.Text = Convert.ToString(houses[h].TotalFloors);
                }
            }
            for (int a = 0; a < apartments.Count; a++)
            {
                if (apartments[a].Id == restates[i].Id)
                {
                    totalarea_textbox.Text = Convert.ToString(apartments[a].TotalArea);
                    rooms_textbox.Text = Convert.ToString(apartments[a].Rooms);
                    floor_textbox.Text = Convert.ToString(apartments[a].Floor);
                }
            }

        }

        private void addland_button_Click(object sender, RoutedEventArgs e)
        {
            new AddLandWindow().Show();
        }
        private void addhouse_button_Click(object sender, RoutedEventArgs e)
        {
            new AddHouseWindow().Show();
        }
        private void addapartment_button_Click(object sender, RoutedEventArgs e)
        {
            new AddApartmentWindow().Show();
        }
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            int i = restate_listbox.SelectedIndex;
            int id_del =restates[i].Id;
            string query2 = "", query3 ="", query4="";
            string query1 = String.Format("DELETE from RealEstateSet where Id='" + id_del + "';");
            for (int l = 0; l < lands.Count; l++)
            {
                if (lands[l].Id == id_del)
                    query2 = String.Format("DELETE from RealEstateSet_Land where Id='" + id_del + "';");
            }
            for (int h = 0; h < houses.Count; h++)
            {
                if (houses[h].Id == id_del)
                    query3 = String.Format("DELETE from RealEstateSet_House where Id='" + id_del + "';");
            }
            for (int a = 0; a < apartments.Count; a++)
            {
                if (apartments[a].Id == id_del)
                    query4 = String.Format("DELETE from RealEstateSet_Apartment where Id='" + id_del + "';");
            }
            string query = String.Format(query1 + query2 + query3 + query4);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            MessageBox.Show("Объект недвижимости удален!");
        }

        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            city_textbox.IsEnabled = true;
            street_textbox.IsEnabled = true;
            house_textbox.IsEnabled = true;
            number_textbox.IsEnabled = true;
            latitude_textbox.IsEnabled = true;
            longitude_textbox.IsEnabled = true;
            totalarea_textbox.IsEnabled = true;
            floor_textbox.IsEnabled = true;
            rooms_textbox.IsEnabled = true;
            save_button.IsEnabled = true;
        }
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            int i = restate_listbox.SelectedIndex;
                string city_edit = city_textbox.Text;
                string street_edit = street_textbox.Text;
                string house_edit = house_textbox.Text;
                string number_edit = number_textbox.Text;
                int latitude_edit = Convert.ToInt32(latitude_textbox.Text);
                int longitude_edit = Convert.ToInt32(longitude_textbox.Text);
                double totalarea_edit = Convert.ToDouble(totalarea_textbox.Text);
                int floor_edit = Convert.ToInt32(floor_textbox.Text);
                int rooms_edit = Convert.ToInt32(rooms_textbox.Text);
                string id_edit = Convert.ToString(restates[i].Id);

                string sql = String.Format("Update RealEstateSet Set Address_City = '" + city_edit + "', Address_Street = '" + street_edit + "',Address_House = '" + house_edit + "',Address_Number = '" + number_edit + "',Coordinate_latitude = '" + latitude_edit + "',Coordinate_longitude = '" + longitude_edit + "' where Id = '" + id_edit + "';");
                string sql_land = String.Format("UPDATE RealEstateSet_Land SET TotalArea = " + totalarea_edit + " where Id = '" + id_edit + "'; ");
                string sql_house = String.Format("UPDATE RealEstateSet_House SET TotalArea = " + totalarea_edit + ",TotalFloors = " + floor_edit + " where Id = '" + id_edit + "'; ");
                string sql_apartment = String.Format("UPDATE RealEstateSet_Apartment SET TotalArea = " + totalarea_edit + ",Rooms = " + rooms_edit + ",Floor = " + floor_edit + " where Id = '" + id_edit + "'; ");
                string sqlExpression = String.Format(sql + sql_land + sql_house + sql_apartment);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                MessageBox.Show("Изменения были успешно сохранены!");
            city_textbox.IsEnabled = false;
            street_textbox.IsEnabled = false;
            house_textbox.IsEnabled = false;
            number_textbox.IsEnabled = false;
            latitude_textbox.IsEnabled = false;
            longitude_textbox.IsEnabled = false;
            totalarea_textbox.IsEnabled = false;
            floor_textbox.IsEnabled = false;
            rooms_textbox.IsEnabled = false;
            save_button.IsEnabled = false;
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new RealEstate().Show();
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
