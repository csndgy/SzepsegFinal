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
        private readonly string connectionString = "server=localhost;database=szepseg;uid=root;";

        public MainWindow()
        {
            InitializeComponent();
            SzolgaltatasokBetoltese();
        }

        private void SzolgaltatasokBetoltese()
        {
            string lekerdezes = "SELECT SzolgaltatasKategoria FROM szolgaltatas";

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
    FROM dolgozok d 
    JOIN szolgaltatas s ON d.szolgáltatasa = s.szolgaltatasID
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
            Bejelnetkzes bejelnetkzes = new Bejelnetkzes();
            bejelnetkzes.Show();
        }
    }
}
