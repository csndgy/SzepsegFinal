using System;
using System.Collections.ObjectModel;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SzepsegV0._2
{
    public partial class Bejelnetkzes : Window
    {
        private MainWindow bejelentkezesAblak;
        private static readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";

        static ObservableCollection<Booking> booking = new ObservableCollection<Booking>();
        public Bejelnetkzes()
        {
            InitializeComponent();
            dataGridBooking.ItemsSource = booking; // Bind DataGrid
            LoadDataGrid(); // Load data from the database
            this.DataContext = this;
        }

        public Bejelnetkzes(string felhasznaloNev) : this()
        {

            lbnFelhasznaloNev.Content = felhasznaloNev;
        }

        public void Bejelentkezes(string felhasznaloNev)
        {
            // Beállítjuk a Label szövegét
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridBooking.SelectedIndex < 0)
            {
                MessageBox.Show("Nincs kijelölt elem!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                for (int i = dataGridBooking.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Booking selectedBooking = (Booking)dataGridBooking.SelectedItems[i];

                    // Create DELETE command
                    string deleteQuery = "DELETE FROM `Foglalás` WHERE `foglalasID` = @foglalasID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@foglalasID", selectedBooking.FoglalasID);
                        command.ExecuteNonQuery();
                    }

                    // Remove the booking from the ObservableCollection
                    booking.Remove(selectedBooking);
                }
            }
        }
        public static void LoadDataGrid() // Public to allow external refresh
        {
            string query = "SELECT * FROM Foglalás"; // Use backticks if necessary

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear previous items
                    booking.Clear();

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

                        booking.Add(foglalas);
                    }

                    // Check if any records were added
                    if (booking.Count == 0)
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
