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
