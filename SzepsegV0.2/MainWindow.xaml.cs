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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadWorkers();
            LoadServices();
        }

        private void LoadServices()
        {
            string connectionString = "server=localhost;database=szepseg;uid=root;";
            string query = "SELECT SzolgaltatasKategoria FROM szolgaltatas";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    List<string> services = new List<string>();

                    while (reader.Read())
                    {
                        services.Add(reader["SzolgaltatasKategoria"].ToString());
                    }

                    ServiceComboBox.ItemsSource = services;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a szolgáltatások betöltése során: " + ex.Message);
                }
            }
        }

        // Dolgozók tábla adatainak betöltése a ComboBox-ba
        private void LoadWorkers()
        {
            string connectionString = "server=localhost;database=szepseg;uid=root;";
            string query = "SELECT CONCAT(dolgozoFirstName, ' ', dolgozoLastName) AS DolgozoNev FROM dolgozok WHERE statusz = 1"; // Csak az aktív dolgozók

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    List<string> workers = new List<string>();

                    while (reader.Read())
                    {
                        workers.Add(reader["DolgozoNev"].ToString());
                    }

                    WorkerComboBox.ItemsSource = workers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a dolgozók betöltése során: " + ex.Message);
                }
            }
        }
    }
}