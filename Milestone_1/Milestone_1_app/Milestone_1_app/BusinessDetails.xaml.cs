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
using System.Windows.Shapes;
using Npgsql;

namespace Milestone_1_app
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        private string bid = "";

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1db; Password = ";
        }

        public BusinessDetails(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);

            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name, state, city FROM business WHERE business_id = '" + bid + "';";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        b_name.Text = reader.GetString(0);
                        state.Text = reader.GetString(1);
                        city.Text = reader.GetString(2);

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

                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    int biz_count = 0;
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name FROM business WHERE state = '" + state.Text + "' ;";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            biz_count++;

                        ns.Content = biz_count.ToString();
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

                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    int biz_count = 0;
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name FROM business WHERE city = '" + city.Text + "' ;";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            biz_count++;

                        nc.Content = biz_count.ToString();
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
