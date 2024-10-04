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
    public partial class Bejelnetkzes : Window
    {
        private MainWindow bejelentkezesAblak;

        public Bejelnetkzes()
        {
            InitializeComponent();
        }

        private void btnFoglalas_Click(object sender, RoutedEventArgs e)
        {
            if (bejelentkezesAblak == null || !bejelentkezesAblak.IsVisible)
            {
                bejelentkezesAblak = new MainWindow();
                bejelentkezesAblak.Closed += (s, args) => bejelentkezesAblak = null; //??? chatG
                bejelentkezesAblak.Show();
            }
            else
            {
                bejelentkezesAblak.Activate();
            }
        }
    }
}
