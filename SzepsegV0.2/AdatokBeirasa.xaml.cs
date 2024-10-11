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
        public AdatokBeirasa()
        {
            InitializeComponent();
        }

        


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Kiolvassuk a szöveget a TextBox-ból
            string felhasznaloNev = txtBeirtNev.Text;

            // Ellenőrizzük, hogy nem üres-e a felhasználói név
            if (!string.IsNullOrEmpty(felhasznaloNev))
            {
                // Megnyitjuk a Bejelentkezés ablakot és átadjuk neki a nevet
                Bejelnetkzes bejelentkezesWindow = new Bejelnetkzes(felhasznaloNev);

                // Bezárjuk az AdatokBeirasa ablakot
                this.Close();

                // Megnyitjuk a Bejelentkezés ablakot
                bejelentkezesWindow.Show();
            }
            else
            {
                MessageBox.Show("Kérjük, adja meg a felhasználó nevét.");
            }
        }

        private void txtBeirtNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Csak betűk engedélyezése (A-Z, a-z)
            string input = txtBeirtNev.Text;
            StringBuilder filtered = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c) || char.IsWhiteSpace(c)) // Csak betűk és szóközök engedélyezettek
                {
                    filtered.Append(c);
                }
            }

            // Ha a beírt szöveg eltér a szűrt szövegtől, frissítjük a TextBox tartalmát
            if (txtBeirtNev.Text != filtered.ToString())
            {
                txtBeirtNev.Text = filtered.ToString();
                txtBeirtNev.CaretIndex = filtered.Length; // Helyezzük a kurzort a szöveg végére
            }
        }
    }
}
