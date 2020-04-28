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
            public string userName { get; set; }
            public double stars { get; set; }
            public string yeslpingSince { get; set; }
            public int totalTipLikes { get; set; }
        }

        public class Tip
        {
            public string Username { get; set; }
            public string Bussiness { get; set; }
            public string City { get; set; }
            public string Text { get; set; }
            public string Date { get; set; }
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

        private string DB = "milestone3";
        private string PW = "peanutbutter12";
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

        private string queryUserId()
        {
            return "SELECT userID FROM yelpUser WHERE uName = '" + User_Search.Text.ToString() + "'";
        }

        private string queryUser(string UserId)
        {
            return "SELECT * FROM yelpUser WHERE userID = '" + UserId + "'";
        }

        private string queryFriends(String Item)
        {
            return "SELECT * FROM Friends WHERE userID1 = '" + Item + "'";
        }

        private string queryTips(string userId)
        {
            return "SELECT * FROM Tips WHERE tuserID = '" + userId + "' ORDER BY tPosted_Datetime DESC";
        }

        private string queryBusiness(string BID)
        {
            return "SELECT bName, bCity FROM Business WHERE businessID = '" + BID + "'";
        }

        //TODO: fix category table attributes
        private string buildBusinessQuery()
        {
            GeoCoordinateWatcher currentLocation = new GeoCoordinateWatcher();
            currentLocation.Start();
            GeoCoordinate currentCoordinate = currentLocation.Position.Location;


            string sql = "SELECT DISTINCT businessID, bName, bStreet, bCity, bState, calculate_distance('" + currentCoordinate.Latitude + "', '" + currentCoordinate.Longitude + "', bLatitude, bLongitude, M), bNum_Stars, bNum_Tips, bCheckins " +
                      "FROM business, category " +
                      "WHERE business.bID = category.bID AND state = '" + State_Select.SelectedItem.ToString() + "' AND bCity = '" + City_List.SelectedItem.ToString() + "' AND bPostal_Code = '" + Zipcode_List.SelectedItem.ToString() +
                      "' AND NOT EXISTS { " +
                            "SELECT businessID, bName, bStreet, bCity, bState, calculate_distance('" + currentCoordinate.Latitude + "', '" + currentCoordinate.Longitude + "', bLatitude, bLongitude, M), bNum_Stars, bNum_Tips, bCheckins " +
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
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("userName");
            col1.Header = "Name";
            col1.Width = 125;
            FriendsBox.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("totalTipLikes");
            col2.Header = "Total Likes";
            col2.Width = 125;
            FriendsBox.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("stars");
            col3.Header = "Avg Stars";
            col3.Width = 100;
            FriendsBox.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("yeslpingSince");
            col4.Header = "YelpingSince";
            col4.Width = 145;
            FriendsBox.Columns.Add(col4);
        }

        private void init_FriendTips()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("Username");
            col1.Header = "User Name";
            col1.Width = 100;
            LatestTipsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("Bussiness");
            col2.Header = "Bussiness";
            col2.Width = 100;
            LatestTipsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("City");
            col3.Header = "City";
            col3.Width = 75;
            LatestTipsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("Text");
            col4.Header = "Text";
            col4.Width = 821;
            LatestTipsGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("Date");
            col5.Header = "Date";
            col5.Width = 75;
            LatestTipsGrid.Columns.Add(col5);
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
            UserIdBox.Items.Clear();
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryUserId();

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            UserIdBox.Items.Add(reader.GetString(0));//add to city list
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


        private void User_Selected(object sender, SelectionChangedEventArgs e)
        {
            int index = UserIdBox.SelectedIndex;
            string item = "";
            if (index > -1)
            {
               item = UserIdBox.SelectedItem.ToString();
            }
            
            populateUserInformation();
            populateFriendsTable(index, item);
            populateTipsTable(index, item);
        }

        private List<string> getFriends(string userID)
        {
            List<string> friends = new List<string>();
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryFriends(userID);

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            friends.Add(reader.GetString(1));
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
            return friends;
        }

        private Tip getLatestTip(string userID)
        {
            Tip latestTip = new Tip() 
            { 
                Username = "NA",
                Bussiness = "NA",
                City = "NA",
                Text = "No Tips posted.",
                Date = "NA"
            };
            int readFirst = 0;
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryTips(userID);

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if(readFirst == 0)
                            {
                                latestTip = new Tip()
                                {
                                    Username = reader.GetString(1),
                                    Bussiness = reader.GetString(0),
                                    City = "",
                                    Text = reader.GetString(4),
                                    Date = reader.GetTimeStamp(3).ToString()
                                };
                                readFirst++;
                            }
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
            latestTip = updateBusinessInformation(latestTip);
            latestTip =  updateUserInformation(latestTip);
            return latestTip;
        }

        private Tip updateBusinessInformation(Tip latestTip)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryBusiness(latestTip.Bussiness);

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            latestTip.Bussiness = reader.GetString(0);
                            latestTip.City = reader.GetString(1);
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
            return latestTip;
        }

        private Tip updateUserInformation(Tip latestTip)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryUser(latestTip.Username);

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            latestTip.Username = reader.GetString(1);
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
            return latestTip;
        }


        private void populateUserInformation()
        {
            if (UserIdBox.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryUser(UserIdBox.SelectedItem.ToString());

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                UserInformationName.Text = reader.GetString(1);
                                UserInformationFans.Text = reader.GetInt32(2).ToString();
                                UserInformationLat.Text = reader.GetDouble(9).ToString();
                                UserInformationLong.Text = reader.GetDouble(10).ToString();
                                UserInformationStars.Text = reader.GetDouble(8).ToString();
                                UserInformationTipCount.Text = reader.GetInt32(6).ToString();
                                //Problem with the database
                                //UserInformationTotalTipLikes.Text = reader.GetInt32(7).ToString();
                                UserInformationTotalTipLikes.Text = "DataBase Problem";
                                UserInformationYelpingCool.Text = reader.GetInt32(4).ToString();
                                UserInformationYelpingFunny.Text = reader.GetInt32(3).ToString();
                                UserInformationYelpingUseful.Text = reader.GetInt32(5).ToString();
                                UserInformationYelpingSince.Text = reader.GetDate(11).ToString();

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

        private void populateFriendsTable(int selectedIndex, string item)
        {
            FriendsBox.Items.Clear();
            List<string> friends = new List<string>();
            if (selectedIndex > -1)
            {
                friends = getFriends(item);
                foreach (string friend in friends)
                {
                    using (var connection = new NpgsqlConnection(buildConnectionString()))
                    {
                        connection.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = queryUser(friend);

                            try
                            {
                                var reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    FriendsBox.Items.Add(new User()
                                    {
                                        userName = reader.GetString(1),
                                        stars = reader.GetDouble(8),
                                        yeslpingSince = reader.GetDate(11).ToString(),
                                        //totalTipLikes = reader.GetInt32(7)
                                        totalTipLikes = 0
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
        }

        private void populateTipsTable(int selectedIndex, string userid)
        {
            LatestTipsGrid.Items.Clear();
            List<string> friends = new List<string>();
            if (selectedIndex > -1)
            {
                friends = getFriends(userid);
                foreach (string friend in friends)
                {
                    Tip currentTip = getLatestTip(friend);
                    if(currentTip.Text != "No Tips posted.")
                    {
                        LatestTipsGrid.Items.Add(currentTip);
                    }
                }
            }
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

        private void Category_List_Selection(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Selected_Category_Update(object sender, SelectionChangedEventArgs e)
        {
        //    selected_Category = new Category() { category_name = Category_List.SelectedItem.ToString() };
        }

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
