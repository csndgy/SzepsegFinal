using MySql.Data.MySqlClient;
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

namespace SzepsegV0._2
{
    /// <summary>
    /// Interaction logic for AdatokBeirasa.xaml
    /// </summary>
    public partial class AdatokBeirasa : Window
    {
        private static readonly string connectionString = "server=localhost;database=szepsegfinal;uid=root;";
        public AdatokBeirasa()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string teljesNev = txtBeirtNev.Text;
            if (!string.IsNullOrEmpty(teljesNev))
            {
                string[] nevek = teljesNev.Split(' ');

                if (nevek.Length < 2)
                {
                    MessageBox.Show("Kérjük, adja meg a keresztnevet és a vezetéknevet.(ebben a sorendben)");
                    return;
                }

                string vezetekNev = nevek[0]; 
                string keresztNev = nevek[1]; 

            
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                 
                    string query = "SELECT COUNT(*) FROM ügyfél WHERE ugyfelFirstName = @keresztNev AND ugyfelLastName = @vezetekNev";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vezetekNev", vezetekNev);
                        command.Parameters.AddWithValue("@keresztNev", keresztNev);
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            Bejelnetkzes bejelentkezesWindow = new Bejelnetkzes(teljesNev);
                            MainWindow nevAtadas = new MainWindow(vezetekNev, keresztNev);
                            this.Close();
                            bejelentkezesWindow.Show();
                        }
                        else
                        {
                            MessageBox.Show("Nincs ilyen felhasználó, regisztrálnia kell.");
                        }
                    }
                }
            }
        }

        private void txtBeirtNev_TextChanged(object sender, TextChangedEventArgs e)
        {

            string input = txtBeirtNev.Text;
            StringBuilder filtered = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c) || char.IsWhiteSpace(c))
                {
                    filtered.Append(c);
                }
            }

            if (txtBeirtNev.Text != filtered.ToString())
            {
                txtBeirtNev.Text = filtered.ToString();
                txtBeirtNev.CaretIndex = filtered.Length; 
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Foglalasok foglalasokWindow = new Foglalasok();

            foglalasokWindow.Show();
        }
    }
}
