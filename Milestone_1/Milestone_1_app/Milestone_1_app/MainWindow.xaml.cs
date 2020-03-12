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

namespace Milestone_1_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Bussiness
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumnsToGrid();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1; Password = Spartan-195";
        }

        private void addState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state;";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            state_list.Items.Add(reader.GetString(0));
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

        private void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "Bussiness Name";
            col1.Width = 255;
            bussiness_grid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            bussiness_grid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            bussiness_grid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("bid");
            col4.Header = "";
            col4.Width = 0;
            bussiness_grid.Columns.Add(col4);

        }

        private void state_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            city_list.Items.Clear();
            if (state_list.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + state_list.SelectedItem.ToString() + "' ORDER BY city;";

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                                city_list.Items.Add(reader.GetString(0));
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

        private void city_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bussiness_grid.Items.Clear();
            if (city_list.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT name, state, city, business_id FROM business WHERE state = '" + state_list.SelectedItem.ToString() + "' AND city = '" +city_list.SelectedItem.ToString() + "' ORDER BY name;";

                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                                bussiness_grid.Items.Add(new Bussiness() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2) , bid = reader.GetString(3)});
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

        private void bussiness_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bussiness_grid.SelectedIndex > -1)
            {
                Bussiness B = bussiness_grid.Items[bussiness_grid.SelectedIndex] as Bussiness;
                if (B.bid != null && B.bid.ToString().CompareTo("") != 0)
                {
                    BusinessDetails businessWindow = new BusinessDetails(B.bid.ToString());
                    businessWindow.Show();
                }
            }
        }
    }
}
