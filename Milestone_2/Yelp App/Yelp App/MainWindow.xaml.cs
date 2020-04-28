using System;
using System.Device.Location;
using System.Collections;
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
            public string BusinessID { get; set; }
            public string BusinessName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public float Distane { get; set; }
            //public float Longitude { get; set; }
            //public float Latitude { get; set; }
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

        //public class Category
        //{
        //    public string category_name { get; set; }
        //}



        public MainWindow()
        {
            InitializeComponent();
            init_UserList();
            init_UserFriends();
            init_FriendTips();
            init_BusinessList();
        }

        private string DB = "milestone2";
        private string PW = "Spartan-195";
        private Business cur_Business;
        private string cur_Category;
        private ArrayList Business_Categories = new ArrayList();
        private ArrayList Selected_Business_Categories = new ArrayList();

        //Database Connection and Queries
        private string buildConnectionString()
        {
            //TODO: change Database and Password to connect to DB
            return "Host = localhost; Username = postgres; Database = " + DB + "; Password = " + PW;
        }

        //TODO: check/fix SELECT statements in following queries to match database tables
        private string queryCity() {
            return "SELECT DISTINCT city FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' ORDER BY city";
        }

        private string queryZipcode()
        {
            return "SELECT DISTINCT zipcode FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' AND '" + City_List.SelectedItem.ToString() + "' ORDER BY city";
        }

        private string queryBusinessesDefault()
        {
            //BusinessName, Address, City, State, Stars, NumTips, NumCheckins
            //TODO: Adjust query to get latitude and longitude
            GeoCoordinateWatcher currentLocation = new GeoCoordinateWatcher();
            currentLocation.Start();
            GeoCoordinate currentCoordinate = currentLocation.Position.Location;

            return "SELECT businessID, bName, bStreet, bCity, bState, calculate_distance('" + currentCoordinate.Latitude + "', '" + currentCoordinate.Longitude + "', bLatitude, bLongitude, M), bNum_Stars, bNum_Tips, bCheckins FROM business WHERE state = '" + State_Select.SelectedItem.ToString() + "' AND '" + City_List.SelectedItem.ToString() + "' AND '" + Zipcode_List.SelectedItem.ToString() + "' ORDER BY city";
        }

        private string queryAllCategories()
        {
            return "SELECT DISTINCT Category FROM Categories";
        }

        private void FillCategories()
        {
            Business_Categories.Clear();
            if (Business_Categories.Count > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryAllCategories();

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Business_Categories.Add(reader.GetString(0));//add to city list
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

        //TODO: fix category table attributes
        private string buildBusinessQuery()
        {
            GeoCoordinateWatcher currentLocation = new GeoCoordinateWatcher();
            currentLocation.Start();
            GeoCoordinate currentCoordinate = currentLocation.Position.Location;


            string sql = "SELECT businessID, bName, bStreet, bCity, bState, calculate_distance('" + currentCoordinate.Latitude + "', '" + currentCoordinate.Longitude + "', bLatitude, bLongitude, M), bNum_Stars, bNum_Tips, bCheckins " +
                      "FROM business, category " +
                      "WHERE business.bID = category.bID AND state = '" + State_Select.SelectedItem.ToString() + "' AND bCity = '" + City_List.SelectedItem.ToString() + "' AND bPostal_Code = '" + Zipcode_List.SelectedItem.ToString() +
                      "' AND NOT EXISTS  { SELECT businessID, bName, bStreet, bCity, bState, calculate_distance('" + currentCoordinate.Latitude + "', '" + currentCoordinate.Longitude + "', bLatitude, bLongitude, M), bNum_Stars, bNum_Tips, bCheckins " +
                        "FROM business, category " +
                        "WHERE business.bID = category.bID AND state = '" + State_Select.SelectedItem.ToString() + "' AND bCity = '" + City_List.SelectedItem.ToString() + "' AND bPostal_Code = '" + Zipcode_List.SelectedItem.ToString() + "'";
            for (int i = 0; i < Selected_Business_Categories.Count; ++i)
            {
                sql = sql + " AND categories <> '" + Selected_Business_Categories[i].ToString() + "'";
            }

            sql += "} ORDER BY city";

            return sql;
        }

        private string queryHours()
        {
            return "SELECT hHours FROM Hours_Open WHERE hBusinessID = '" + cur_Business.BusinessID + "' AND hDay = '" + DateTime.Now.Day.ToString() + "'";
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

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Binding = new Binding("businessID");
            col9.Header = "";
            col9.Width = 0;
            Business_List.Columns.Add(col9);
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
                                Zipcode_List.Items.Add(reader.GetString(0));//add to zipcode list
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
                                    BusinessID = reader.GetString(0),
                                    BusinessName = reader.GetString(1),
                                    Address = reader.GetString(2),
                                    City = reader.GetString(3),
                                    State = reader.GetString(4),
                                    //TODO: calculate distance to business
                                    Distane = reader.GetFloat(5),
                                    Stars = reader.GetFloat(6),
                                    NumTips = reader.GetInt32(7),
                                    NumCheckins = reader.GetInt32(8)
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
            if (Business_List.SelectedIndex > -1)
            {
                cur_Business = Business_List.Items[Business_List.SelectedIndex] as Business;
                if (cur_Business.BusinessID != null && cur_Business.BusinessID.ToString().CompareTo("") != 0)
                {
                    Selected_Business_Name.Content = cur_Business.BusinessName;
                    Selected_Business_Address.Content = cur_Business.Address;
                    
                    using (var connection = new NpgsqlConnection(buildConnectionString()))
                    {
                        connection.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = queryHours();
                            try
                            {
                                var reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Selected_Business_Hours.Content = reader.GetString(0);
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
        }

        //private void Category_List_Selection(object sender, SelectionChangedEventArgs e)
        //{
            
        //}

        //private void Selected_Category_Update(object sender, SelectionChangedEventArgs e)
        //{
        //    selected_Category = new Category() { category_name = Category_List.SelectedItem.ToString() };
        //}

        private void Category_Add_Click(object sender, RoutedEventArgs e)
        {
            string selected_Cat_Text = Category_List.SelectedItem.ToString();
            int selected_Cat_Index = Category_List.SelectedIndex;

            Selected_Categories_List.Items.Add(selected_Cat_Text);
            if (Business_Categories != null)
            {
                Business_Categories.RemoveAt(selected_Cat_Index);
            }
            ApplyDataBinding();
        }

        private void Category_Remove_Click(object sender, RoutedEventArgs e)
        {
            string selected_Cat_Text = Category_List.SelectedItem.ToString();
            int selected_Cat_Index = Category_List.SelectedIndex;

            Category_List.Items.Add(selected_Cat_Text);
            if (Selected_Business_Categories != null)
            {
                Selected_Business_Categories.RemoveAt(selected_Cat_Index);
            }
            ApplyDataBinding();
        }

        private void ApplyDataBinding()
        {
            Category_List.ItemsSource = null;
            Category_List.ItemsSource = Business_Categories;

            Selected_Categories_List.ItemsSource = null;
            Selected_Categories_List.ItemsSource = Selected_Business_Categories;
        }

        private void Business_Category_Filter_Search(object sender, RoutedEventArgs e)
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
                        cmd.CommandText = buildBusinessQuery();

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                //BusinessName, Address, City, State, Distane, Stars, NumTips, NumCheckins 
                                Business_List.Items.Add(new Business()
                                {
                                    BusinessID = reader.GetString(0),
                                    BusinessName = reader.GetString(1),
                                    Address = reader.GetString(2),
                                    City = reader.GetString(3),
                                    State = reader.GetString(4),
                                    //TODO: calculate distance to business
                                    Distane = reader.GetFloat(5),
                                    Stars = reader.GetFloat(6),
                                    NumTips = reader.GetInt32(7),
                                    NumCheckins = reader.GetInt32(8)
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

        private void Update_Business_Checkin(object sender, RoutedEventArgs e)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO checkin(cBusinessID, cDate, cTime) VALUES ('" + cur_Business.BusinessID + "', '" + DateTime.Now.Day + "', '" + DateTime.Now.TimeOfDay + "')";
                    try
                    {
                        var writer = cmd.ExecuteNonQuery();
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
}
