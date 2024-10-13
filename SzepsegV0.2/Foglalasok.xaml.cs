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

namespace SzepsegV0._2
{
    public partial class Foglalasok : Window
    {
        public Foglalasok()
        {
            InitializeComponent();
        }

        private void btnBeReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegKeresztNev.Text) ||
                string.IsNullOrWhiteSpace(txtRegVezeNev.Text) ||
                string.IsNullOrWhiteSpace(txtRegTelSzam.Text) ||
                string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                MessageBox.Show("Kérjük, töltse ki az összes mezőt!", "Hiányzó adatok", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string connectionString = "server=localhost;database=szepsegfinal;uid=root;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO ügyfél (ugyfelFirstName, ugyfelLastName, ugyfelTel, ugyfelEmail, ugyfelPontok) " +
                                   "VALUES (@FirstName, @LastName, @Tel, @Email, 0)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@FirstName", txtRegKeresztNev.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtRegVezeNev.Text);
                        cmd.Parameters.AddWithValue("@Tel", txtRegTelSzam.Text);
                        cmd.Parameters.AddWithValue("@Email", txtRegEmail.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Sikeres regisztráció!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void txtRegKeresztNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txtRegKeresztNev.Text;
            StringBuilder filtered = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c) || char.IsWhiteSpace(c))
                {
                    filtered.Append(c);
                }
            }


            if (txtRegKeresztNev.Text != filtered.ToString())
            {
                txtRegKeresztNev.Text = filtered.ToString();
                txtRegKeresztNev.CaretIndex = filtered.Length; 
            }
        }

        private void txtRegVezeNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txtRegVezeNev.Text;
            StringBuilder filtered = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c) || char.IsWhiteSpace(c))
                {
                    filtered.Append(c);
                }
            }


            if (txtRegVezeNev.Text != filtered.ToString())
            {
                txtRegVezeNev.Text = filtered.ToString();
                txtRegVezeNev.CaretIndex = filtered.Length; 
            }
        }

        private void txtRegEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txtRegEmail.Text;
            StringBuilder filtered = new StringBuilder();
            bool atSymbolExists = false;
            bool dotExists = false;

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) || c == '@' || c == '.')
                {
                    if (c == '@')
                    {
                        if (atSymbolExists)
                            continue; 
                        atSymbolExists = true;
                    }
                    if (c == '.')
                    {
                        if (dotExists)
                            continue; 
                        dotExists = true;
                    }

                    filtered.Append(c); 
                }
            }

            if (txtRegEmail.Text != filtered.ToString())
            {
                txtRegEmail.Text = filtered.ToString();
                txtRegEmail.CaretIndex = filtered.Length;
            }
        }

        private void txtRegTelSzam_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txtRegTelSzam.Text;
            StringBuilder filtered = new StringBuilder();

            // Ellenőrizzük, hogy az első karakter '+'-e
            if (input.Length > 0 && input[0] == '+')
            {
                filtered.Append('+'); // Hozzáadjuk a '+' jelet
            }

            // Az összes további karaktert ellenőrizzük
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsNumber(input[i]) && filtered.Length < 12)
                {
                    filtered.Append(input[i]); // Csak számok, és max 12 karakter
                }
            }

            // Frissítjük a textboxot, ha szükséges
            if (txtRegTelSzam.Text != filtered.ToString())
            {
                txtRegTelSzam.Text = filtered.ToString();
                txtRegTelSzam.CaretIndex = filtered.Length; // Áthelyezi a kurzort a szöveg végére
            }
        }
    }
}
