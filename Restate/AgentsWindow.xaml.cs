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
    /// Логика взаимодействия для AgentsWindow.xaml
    /// </summary>
    public partial class AgentsWindow : Window
    {
        List<Agent> agents = new List<Agent>();
        public AgentsWindow()
        {
            InitializeComponent();
            //var agents = App.Context.PersonSet_Agent.ToList();
            //agents_listbox.ItemsSource = agents;
            string connectionString = @"Data Source = DESKTOP-53PJC1G\SQLEXPRESS;Initial Catalog=restate;Integrated Security=True";
            string sqlExpression = 
                "select FirstName, MiddleName, LastName, DealShare from PersonSet, PersonSet_Agent where PersonSet.Id = PersonSet_Agent.Id; ";
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
                        agents.Add(new Agent() { FirstName = Convert.ToString(firstname),
                            MiddleName = Convert.ToString(middlename),
                            LastName = Convert.ToString(lastname),
                            DealShare = Convert.ToString(dealshare)
                        });
                    }
                }

                reader.Close();
            }
            agents_listbox.ItemsSource = agents;

        }


        private void agents_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = agents_listbox.SelectedIndex;
            firstname_textbox.Text = agents[i].FirstName;
            middlename_textbox.Text = agents[i].MiddleName;
            lastname_textbox.Text = agents[i].LastName;
            dealshare_textbox.Text = agents[i].DealShare;
        }
        public class Agent
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string DealShare { get; set; }

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
