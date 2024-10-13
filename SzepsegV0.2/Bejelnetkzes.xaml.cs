using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace SzepsegV0._2
{
    public partial class Bejelnetkzes : Window
    {
        private AdatokBeirasa adatokBeirasa;
        private MainWindow bejelentkezesAblak;
        private static readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";
        public int CustomerId { get; private set; }
        static ObservableCollection<Booking> booking = new ObservableCollection<Booking>();
        public Bejelnetkzes()
        {
            InitializeComponent();
            dataGridBooking.ItemsSource = booking; 
            this.DataContext = this;
        }
        public string VezetekNev { get; set; }
        public string KeresztNev { get; set; }
        public Bejelnetkzes(string felhasznaloNev) : this()
        {
            dataGridBooking.ItemsSource = booking;
            lbnFelhasznaloNev.Content = felhasznaloNev;
            this.DataContext = this;

            var names = felhasznaloNev.Split(' ');
            if (names.Length == 2)
            {
                VezetekNev = names[0].Trim();
                KeresztNev = names[1].Trim();

                CustomerId = LoadDataGrid(KeresztNev, VezetekNev);
            }
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
   
                    string deleteQuery = "DELETE FROM `Foglalás` WHERE `foglalasID` = @foglalasID";
                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@foglalasID", selectedBooking.FoglalasID);
                        command.ExecuteNonQuery();
                    }
                    
                    booking.Remove(selectedBooking);
                }
            }
        }
        public static int LoadDataGrid(string firstName, string lastName)
        {
            string customerIdQuery = "SELECT ugyfelID FROM ügyfél WHERE ugyfelFirstName = @firstName AND ugyfelLastName = @lastName";
            int customerId = 0; 

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand idCommand = new MySqlCommand(customerIdQuery, connection))
                    {
                        idCommand.Parameters.AddWithValue("@firstName", firstName);
                        idCommand.Parameters.AddWithValue("@lastName", lastName);

                        var result = idCommand.ExecuteScalar();

                        if (result != null)
                        {
                            customerId = Convert.ToInt32(result);
                        }
                        else
                        {
                            return 0; 
                        }
                    }

                    string query = @"SELECT f.foglalasID, f.szolgaltatasID, f.dolgozoID, f.ugyfelID, f.foglalasStart, f.foglalasEnd,
                             s.szolgaltatasKategoria, d.dolgozoFirstName, d.dolgozoLastName, u.ugyfelFirstName, u.ugyfelLastName
                             FROM Foglalás f
                             JOIN Szolgáltatás s ON f.szolgaltatasID = s.szolgaltatasID
                             JOIN dolgozók d ON f.dolgozoID = d.dolgozoID
                             JOIN ügyfél u ON f.ugyfelID = u.ugyfelID
                             WHERE f.ugyfelID = @ugyfelID";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ugyfelID", customerId); 
                        MySqlDataReader reader = command.ExecuteReader();

                        booking.Clear();


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
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Adatbázis hiba: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt: " + ex.Message);
                }
            }

            return customerId; 
        }

        public void SetNames(string vezetekNev, string keresztNev)
        {
            this.VezetekNev = vezetekNev;
            this.KeresztNev = keresztNev;
        }


        private void btnFoglalas_Click(object sender, RoutedEventArgs e)
        {
            if (bejelentkezesAblak == null || !bejelentkezesAblak.IsVisible)
            {
                bejelentkezesAblak = new MainWindow(KeresztNev,VezetekNev);
                bejelentkezesAblak.Show();  
            }
            else
            {
                bejelentkezesAblak.Activate();
            }
            LoadDataGrid(VezetekNev, KeresztNev);
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
        public string Szolgaltatas { get; set; }  
        public string DolgozoNev { get; set; }    
        public string UgyfelNev { get; set; }     
        public DateTime FoglalasStart { get; set; }
        public DateTime FoglalasEnd { get; set; }
    }
}
