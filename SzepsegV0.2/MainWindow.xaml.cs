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
using System.Diagnostics.Eventing.Reader;

namespace SzepsegV0._2
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";
        private string vezetekNev;
        private string keresztNev;

        public MainWindow(string VezNev,string KerNev)
        {
            InitializeComponent();
            SzolgaltatasokBetoltese();
            tbFelhasznalo.Text = KerNev + " " + VezNev;
            vezetekNev = VezNev;
            keresztNev = KerNev;
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

                    cbServices.ItemsSource = szolgaltatasok;
                    cbServices.SelectionChanged += SzolgaltatasKombobox_SelectionChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a szolgáltatások betöltése során: " + ex.Message);
                }
            }
            FillComboBoxWithTimeSlots();

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

                    cbWorker.ItemsSource = dolgozok;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a dolgozók betöltése során: " + ex.Message);
                }
            }
            FillComboBoxWithTimeSlots();
        }

        private void SzolgaltatasKombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbServices.SelectedItem != null)
            {
                string valasztottSzolgaltatas = cbServices.SelectedItem.ToString();
                DolgozokBetoltese(valasztottSzolgaltatas);
            }
        }

        private int GetSzolgaltatasIdotartam(string szolgaltatasKategoria)
        {
            string lekerdezes = "SELECT TIME_TO_SEC(TIMEDIFF(szolgaltatasIdotartam, '00:00:00')) / 60 FROM Szolgáltatás WHERE szolgaltatasKategoria = @szolgaltatasKategoria";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(lekerdezes, connection);
                command.Parameters.AddWithValue("@szolgaltatasKategoria", szolgaltatasKategoria);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void btnFoglalas_Click(object sender, RoutedEventArgs e)
            {
                if (cbServices.SelectedItem != null && cbWorker.SelectedItem != null && cbAppointment.SelectedItem != null && dpAppointment.SelectedDate != null)
                {
                    if (cbAppointment.SelectedItem != null)
                    {
                        string szolgaltatas = cbServices.SelectedItem.ToString();
                        string dolgozo = cbWorker.SelectedItem.ToString();
                        string idopont = cbAppointment.SelectedItem.ToString();
                        DateTime selectedDate = (DateTime)dpAppointment.SelectedDate;

                        DateTime foglalasStart = DateTime.Parse($"{selectedDate.ToString("yyyy-MM-dd")} {idopont}");

                        if (foglalasStart < DateTime.Now)
                        {
                            MessageBox.Show("Nem lehet időpontot foglalni a múltba!");
                            return;
                        }

                        int szolgaltatasIdotartam = GetSzolgaltatasIdotartam(szolgaltatas);
                        DateTime foglalasEnd = foglalasStart.AddMinutes(szolgaltatasIdotartam);
                        if (IsTimeSlotAvailable(foglalasStart, dolgozo, szolgaltatas))
                        {
                            int szolgaltatasID = GetSzolgaltatasID(szolgaltatas);
                            int dolgozoID = GetDolgozoID(dolgozo);
                            int ugyfelID = GetCustomerId();

                            string lekerdezes = @"INSERT INTO Foglalás (szolgaltatasID, dolgozoID, ugyfelID, foglalasStart, foglalasEnd) 
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
                                    Bejelnetkzes.LoadDataGrid(vezetekNev,keresztNev);
                            }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Hiba történt a foglalás során: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Az időpont foglalt. Válasszon másikat!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Időpont fog", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Kérjük, válassza ki az összes mezőt.");
                }
            }
        

            private int GetCustomerId()
        {
            if (Application.Current.Windows.OfType<Bejelnetkzes>().Any())
            {
                var bejelnetkzesWindow = Application.Current.Windows.OfType<Bejelnetkzes>().First();
                return bejelnetkzesWindow.CustomerId;
            }
            return 0;
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
            DateTime startTime = new DateTime(2024, 10, 4, 8, 0, 0); 
            DateTime endTime = new DateTime(2024, 10, 4, 16, 0, 0);  
            cbAppointment.Items.Clear(); 

            if (cbWorker.SelectedItem != null && cbServices.SelectedItem != null)
            {
                string dolgozo = cbWorker.SelectedItem.ToString();
                string szolgaltatas = cbServices.SelectedItem.ToString();
                int szolgaltatasIdotartam = GetSzolgaltatasIdotartam(szolgaltatas);

                while (startTime <= endTime)
                {
                    if (IsTimeSlotAvailable(startTime, dolgozo, szolgaltatas))
                    {
                        cbAppointment.Items.Add(startTime.ToString("HH:mm")); 
                    }
                    startTime = startTime.AddMinutes(30); 
                }
            }
            else
            {
                while (startTime <= endTime)
                {
                    cbAppointment.Items.Add(startTime.ToString("HH:mm")); 
                    startTime = startTime.AddMinutes(30); 
                }
            }
        }

        private void dpAppointment_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillComboBoxWithTimeSlots();

        }

        private void cbWorker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillComboBoxWithTimeSlots(); 
        }

        private bool IsTimeSlotAvailable(DateTime startTime, string dolgozo, string szolgaltatas)
        {
            int dolgozoID = GetDolgozoID(dolgozo);
            int szolgaltatasID = GetSzolgaltatasID(szolgaltatas);

            int szolgaltatasIdotartam = GetSzolgaltatasIdotartam(szolgaltatas);
            DateTime endTime = startTime.AddMinutes(szolgaltatasIdotartam);

            string query = @"SELECT COUNT(*) FROM Foglalás WHERE dolgozoID = @dolgozoID AND szolgaltatasID = @szolgaltatasID 
                     AND ((foglalasStart < @foglalasEnd) AND (foglalasEnd > @foglalasStart))";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@dolgozoID", dolgozoID);
                command.Parameters.AddWithValue("@szolgaltatasID", szolgaltatasID);
                command.Parameters.AddWithValue("@foglalasEnd", endTime); 
                command.Parameters.AddWithValue("@foglalasStart", startTime); 

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count == 0; 
            }
        }

        private void cbWorker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cbWorker.SelectedItem != null)
            {
                string valasztottSzolgaltatas = cbServices.SelectedItem.ToString();
                FillComboBoxWithTimeSlots();
            }
        }
        public void SetUserName(string userName)
        {
            // Feltételezve, hogy van egy TextBlock a XAML-ban, amelynek neve "tbFelhasznalo"
            tbFelhasznalo.Text = userName;
        }
    }
}
