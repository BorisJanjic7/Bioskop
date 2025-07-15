using System.Windows;

namespace Bioskop
{
    public partial class LoginWindow : Window // Ovo je ključna promena: public
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void BtnKorisnik_Click(object sender, RoutedEventArgs e)
        {
            ViewerWindow viewer = new ViewerWindow();
            viewer.Show();
            this.Close();
        }
    }
}