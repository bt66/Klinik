﻿using System.Windows.Controls;
using System.Windows;
using Klinik.Views;

namespace Klinik
{
    public partial class App : Application
    {
        public static Dashboard View { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            View = new Dashboard();
            View.Show();
        }

        public static void ViewRouting(bool flag, Control content = null)
        {
            if (!flag)
            {
                View.ContentGrid.Children.Clear();
            }
            else
            {
                View.ContentGrid.Children.Clear();
                View.ContentGrid.Children.Add(content);
            }
        }
    }
}