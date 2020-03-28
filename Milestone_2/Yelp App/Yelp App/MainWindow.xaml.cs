using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Yelp_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class User
        {

        }

        public class Business
        {
            public string BusinessName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public float Distane { get; set; }
            public float Stars { get; set; }
            public int NumTips { get; set; }
            public int NumCheckins { get; set; }
        }

        public class State
        {
            public string state_name { get; set; }
        }

        public class City
        {
            public string city_name { get; set; }
        }

        public class Zipcode
        {

        }

        public class Category
        {
            public string category_name { get; set; }
        }



        public MainWindow()
        {
            InitializeComponent();
            init_UserList();
            init_UserFriends();
            init_FriendTips();
            init_BusinessList();
        }

        //Database Connection and Queries
        private string buildConnectionString()
        {
            //TODO: change Database and Password to connect to DB
            return "Host = localhost; Username = postgres; Database = milestone2; Password = Spartan-195";
        }

        //TODO: check/fix SELECT statements in following queries to match database
        private string queryCity() {
            return "SELECT DISTINCT city FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' ORDER BY city";
        }

        private string queryZipcode()
        {
            return "SELECT DISTINCT zipcode FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' AND '" + City_List.SelectedItem.ToString() + "' ORDER BY city";
        }

        private string queryBusinessesDefault()
        {
            
            return "SELECT  FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' AND '" + City_List.SelectedItem.ToString() + "' AND '" + Zipcode_List.SelectedItem.ToString() + "' ORDER BY city";
        }



        //User Tab Init
        private void init_UserList()
        {
             
        }

        private void init_UserFriends()
        {

        }

        private void init_FriendTips()
        {

        }

        //Business Tab Init
        private void init_BusinessList()
        {
            //width = 1360
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("businessName");
            col1.Header = "Business Name";
            col1.Width = 300;
            Business_List.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("address");
            col2.Header = "Address";
            col2.Width = 250;
            Business_List.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 100;
            Business_List.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("state");
            col4.Header = "State";
            col4.Width = 100;
            Business_List.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("distance");
            col5.Header = "Distance";
            col5.Width = 200;
            Business_List.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("starts");
            col6.Header = "Stars";
            col6.Width = 200;
            Business_List.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("tips");
            col7.Header = "# of Tips";
            col7.Width = 100;
            Business_List.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("checkins");
            col8.Header = "Total Checkins";
            col8.Width = 100;
            Business_List.Columns.Add(col8);
        }

        

        private void addStates()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            State_Select.Items.Add(reader.GetString(0));
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Errpr - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        



        //User Tab
        private void User_Search_Updated(object sender, TextChangedEventArgs e)
        {

        }



        //Business Tab
        private void State_Selected(object sender, SelectionChangedEventArgs e)
        {
            City_List.Items.Clear();
            if (State_Select.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryCity();

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                City_List.Items.Add(reader.GetString(0));//add to city list
                            }
                        }
                        catch (NpgsqlException ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        private void City_Selected(object sender, SelectionChangedEventArgs e)
        {
            Zipcode_List.Items.Clear();
            if (City_List.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryZipcode();

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Zipcode_List.Items.Add(reader.GetString(0));//add to city list
                            }
                        }
                        catch (NpgsqlException ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        private void Zipcode_Selected(object sender, SelectionChangedEventArgs e)
        {
            Business_List.Items.Clear();
            if (Zipcode_List.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryBusinessesDefault();

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                //BusinessName, Address, City, State, Distane, Stars, NumTips, NumCheckins 
                                Business_List.Items.Add(new Business()
                                {
                                    BusinessName = reader.GetString(0),
                                    Address = reader.GetString(1),
                                    City = reader.GetString(2),
                                    State = reader.GetString(3),
                                    Distane = reader.GetFloat(4),
                                    Stars = reader.GetFloat(5),
                                    NumTips = reader.GetInt32(6),
                                    NumCheckins = reader.GetInt32(7)
                                });
                            }
                        }
                        catch (NpgsqlException ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        private void Business_Selected(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
