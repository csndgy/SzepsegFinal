using MySql.Data.MySqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace SzepsegV0._2
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";

        public MainWindow()
        {
            InitializeComponent();
            SzolgaltatasokBetoltese();
            FillComboBoxWithTimeSlots();
        }

        private void SzolgaltatasokBetoltese()
        {
            string lekerdezes = "SELECT SzolgaltatasKategoria FROM szolgáltatás";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(lekerdezes, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    List<string> szolgaltatasok = new List<string>();

                    while (reader.Read())
                    {
                        szolgaltatasok.Add(reader["SzolgaltatasKategoria"].ToString());
                    }

                    ServiceComboBox.ItemsSource = szolgaltatasok;
                    ServiceComboBox.SelectionChanged += SzolgaltatasKombobox_SelectionChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a szolgáltatások betöltése során: " + ex.Message);
                }
            }
        }

        private void DolgozokBetoltese(string valasztottSzolgaltatasKategoria)
        {
            string lekerdezes = @"
SELECT CONCAT(d.dolgozoFirstName, ' ', d.dolgozoLastName) AS DolgozoNev 
FROM Dolgozók d 
JOIN Szolgáltatás s ON d.szolgáltatasa = s.szolgaltatasID
WHERE s.szolgaltatasKategoria = @valasztottSzolgaltatasKategoria 
AND d.statusz = 1";


            using (MySqlConnection kapcsolat = new MySqlConnection(connectionString))
            {
                try
                {
                    kapcsolat.Open();
                    MySqlCommand parancs = new MySqlCommand(lekerdezes, kapcsolat);
                    parancs.Parameters.AddWithValue("@valasztottSzolgaltatasKategoria", valasztottSzolgaltatasKategoria);
                    MySqlDataReader olvaso = parancs.ExecuteReader();

                    List<string> dolgozok = new List<string>();

                    while (olvaso.Read())
                    {
                        dolgozok.Add(olvaso["DolgozoNev"].ToString());
                    }

                    WorkerComboBox.ItemsSource = dolgozok;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a dolgozók betöltése során: " + ex.Message);
                }
            }
        }

        private void SzolgaltatasKombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServiceComboBox.SelectedItem != null)
            {
                string valasztottSzolgaltatas = ServiceComboBox.SelectedItem.ToString();
                DolgozokBetoltese(valasztottSzolgaltatas);
            }
        }

        private void btnFoglalas_Click(object sender, RoutedEventArgs e)
        {

            if (ServiceComboBox.SelectedItem != null && WorkerComboBox.SelectedItem != null && appointmentComboBox.SelectedItem != null)
            {
                string szolgaltatas = ServiceComboBox.SelectedItem.ToString();
                string dolgozo = WorkerComboBox.SelectedItem.ToString();
                string idopont = appointmentComboBox.SelectedItem.ToString();

                DateTime foglalasStart = DateTime.Parse(idopont); 
               // DateTime foglalasEnd = foglalasStart.Add(foglalasStart.Minute; 

                int szolgaltatasID = GetSzolgaltatasID(szolgaltatas);
                int dolgozoID = GetDolgozoID(dolgozo);
                int ugyfelID = 1;

                string lekerdezes = @"
            INSERT INTO Foglalás (szolgaltatasID, dolgozoID, ugyfelID, foglalasStart, foglalasEnd)
            VALUES (@szolgaltatasID, @dolgozoID, @ugyfelID, @foglalasStart, @foglalasEnd)";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(lekerdezes, connection);
                        command.Parameters.AddWithValue("@szolgaltatasID", szolgaltatasID);
                        command.Parameters.AddWithValue("@dolgozoID", dolgozoID);
                        command.Parameters.AddWithValue("@ugyfelID", ugyfelID);
                        command.Parameters.AddWithValue("@foglalasStart", foglalasStart);
                        command.Parameters.AddWithValue("@foglalasEnd", foglalasEnd);

                        command.ExecuteNonQuery();
                        MessageBox.Show("A foglalás sikeresen létrejött!");
                        this.Close();
                        Bejelnetkzes.LoadDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hiba történt a foglalás során: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérjük, válassza ki az összes mezőt.");
            }

        }

        private int GetSzolgaltatasID(string szolgaltatasKategoria)
        {
            string lekerdezes = "SELECT szolgaltatasID FROM Szolgáltatás WHERE szolgaltatasKategoria = @szolgaltatasKategoria";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(lekerdezes, connection);
                command.Parameters.AddWithValue("@szolgaltatasKategoria", szolgaltatasKategoria);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private int GetDolgozoID(string dolgozoNev)
        {
            string lekerdezes = "SELECT dolgozoID FROM Dolgozók WHERE CONCAT(dolgozoFirstName, ' ', dolgozoLastName) = @dolgozoNev";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(lekerdezes, connection);
                command.Parameters.AddWithValue("@dolgozoNev", dolgozoNev);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }


        private void FillComboBoxWithTimeSlots()
        {
            DateTime startTime = new DateTime(2024, 10, 4, 8, 0, 0); // Kezdő időpont 8:00
            DateTime endTime = new DateTime(2024, 10, 4, 16, 0, 0);  // Végső időpont 16:00

            while (startTime <= endTime)
            {
                appointmentComboBox.Items.Add(startTime.ToString("HH:mm")); // Formázás: Óra és Perc
                startTime = startTime.AddMinutes(30); // Félórás lépés
            }
        }

        //private void btnFoglalas_Click(object sender, RoutedEventArgs e)
        //{
        //    Bejelnetkzes bejelnetkzes = new Bejelnetkzes();
        //    bejelnetkzes.Show();
        //}
    }
}
