using System.Windows;

namespace Klinik.Views
{

    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            customDesign();
        }
        private void customDesign()
        {
            subMenu.Visibility = Visibility.Collapsed;
            labelUsername.Content = App.SessionUser;
            labelRole.Content = App.SessionRole;
        }
        private void hideSubMenu()
        {
            if (subMenu.Visibility == Visibility.Visible)
            {
                subMenu.Visibility = Visibility.Collapsed;
            }
        }
        private void showSubMenu()
        {
            if (subMenu.Visibility == Visibility.Collapsed)
            {
                hideSubMenu();
                subMenu.Visibility = Visibility.Visible;
            }
            else if (subMenu.Visibility == Visibility.Visible)
            {
                hideSubMenu();
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            showSubMenu();
        }

        private void btnObat_Click(object sender, RoutedEventArgs e)
        {
            App.ViewRouting(true, new Views.ObatView());
        }

        private void btnPembeli_Click(object sender, RoutedEventArgs e)
        {
            App.ViewRouting(true, new Views.PembeliView());
        }

        private void btnTransaksi_Click(object sender, RoutedEventArgs e)
        {
            App.ViewRouting(true, new Views.TransaksiView());
        }
    }
}
