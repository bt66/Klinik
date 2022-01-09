using Klinik.ViewModels;
using System.Windows;


namespace Klinik.Views
{
    public partial class LoginDialog : Window
    {
        private readonly LoginViewModel vm;
        public LoginDialog()
        {
            InitializeComponent();
            vm = new LoginViewModel();
            vm.OnCallBack += Close;
            DataContext = vm;
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.Model.password = txtPass.Password;
        }
    }
}
