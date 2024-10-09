using System;
using System.Collections.ObjectModel;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SzepsegV0._2
{
    public partial class Bejelnetkzes : Window
    {
        private MainWindow bejelentkezesAblak;
        private readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";

        public ObservableCollection<Booking> Booking { get; set; }

        public Bejelnetkzes()
        {
            InitializeComponent();
            Booking = new ObservableCollection<Booking>();
            dataGridBooking.ItemsSource = Booking; // Bind DataGrid
            LoadDataGrid(); // Load data from the database


        }

        private void LoadDataGrid()
        {
            string query = "SELECT * FROM `Foglalás`"; // Use backticks if necessary

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear previous items
                    Booking.Clear();

                    // Read the data and add to the ObservableCollection
                    while (reader.Read())
                    {
                        Booking foglalas = new Booking
                        {
                            FoglalasID = reader.GetInt32("foglalasID"),
                            SzolgaltatasID = reader.GetInt32("szolgaltatasID"),
                            DolgozoID = reader.GetInt32("dolgozoID"),
                            UgyfelID = reader.GetInt32("ugyfelID"),
                            FoglalasStart = reader.GetDateTime("foglalasStart"),
                            FoglalasEnd = reader.GetDateTime("foglalasEnd")
                        };

                        Booking.Add(foglalas);
                    }

                    // Check if any records were added
                    if (Booking.Count == 0)
                    {
                        MessageBox.Show("No records found in the database.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnFoglalas_Click(object sender, RoutedEventArgs e)
        {
            if (bejelentkezesAblak == null || !bejelentkezesAblak.IsVisible)
            {
                bejelentkezesAblak = new MainWindow();
                bejelentkezesAblak.Show();
            }
            else
            {
                bejelentkezesAblak.Activate();
            }
        }


    }

    public class Booking
    {
        public int FoglalasID { get; set; }
        public int SzolgaltatasID { get; set; }
        public int DolgozoID { get; set; }
        public int UgyfelID { get; set; }
        public DateTime FoglalasStart { get; set; }
        public DateTime FoglalasEnd { get; set; }
    }
}
