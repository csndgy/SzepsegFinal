using System;
using System.Collections.ObjectModel;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SzepsegV0._2
{
    public partial class Bejelnetkzes : Window
    {
        private AdatokBeirasa adatokBeirasa;
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
        public static void LoadDataGrid()
        {
            string query = @"SELECT f.foglalasID, f.szolgaltatasID, f.dolgozoID, f.ugyfelID, f.foglalasStart, f.foglalasEnd,
                            s.szolgaltatasKategoria, d.dolgozoFirstName, d.dolgozoLastName, u.ugyfelFirstName, u.ugyfelLastName
                     FROM Foglalás f
                     JOIN Szolgáltatás s ON f.szolgaltatasID = s.szolgaltatasID
                     JOIN dolgozók d ON f.dolgozoID = d.dolgozoID
                     JOIN ügyfél u ON f.ugyfelID = u.ugyfelID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Előző elemek törlése
                    booking.Clear();

                    // Az új adatok betöltése
                    while (reader.Read())
                    {
                        Booking foglalas = new Booking
                        {
                            FoglalasID = reader.GetInt32("foglalasID"),
                            Szolgaltatas = reader.GetString("szolgaltatasKategoria"),
                            DolgozoNev = reader.GetString("dolgozoFirstName") + " " + reader.GetString("dolgozoLastName"),
                            UgyfelNev = reader.GetString("ugyfelFirstName") + " " + reader.GetString("ugyfelLastName"),
                            FoglalasStart = reader.GetDateTime("foglalasStart"),
                            FoglalasEnd = reader.GetDateTime("foglalasEnd")
                        };

                        booking.Add(foglalas);
                    }

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
            LoadDataGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (adatokBeirasa == null || !adatokBeirasa.IsVisible)
            {
                adatokBeirasa = new AdatokBeirasa();
                adatokBeirasa.Show();
                this.Close();
            }
            else
            {
                adatokBeirasa.Activate();
            }
        }
    }

    public class Booking
    {
        public int FoglalasID { get; set; }
        public string Szolgaltatas { get; set; }  // Szolgáltatás neve
        public string DolgozoNev { get; set; }    // Dolgozó teljes neve
        public string UgyfelNev { get; set; }     // Ügyfél teljes neve
        public DateTime FoglalasStart { get; set; }
        public DateTime FoglalasEnd { get; set; }
    }
}
