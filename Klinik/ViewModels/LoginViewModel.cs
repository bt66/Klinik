using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;
using System;

namespace Klinik.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private ObservableCollection<User> dataUser;
        private User modelUser;
        public LoginViewModel()
        {
            dataUser = new ObservableCollection<User>();
            modelUser = new User();
            ReadCommand = new Command(async () => await ReadDataAsync());
        }
        public ICommand ReadCommand { get; set; }
        public event Action OnCallBack;
        public ObservableCollection<User> DataUser
        {
            get => dataUser;
            set
            {
                SetProperty(ref dataUser, value);
            }
        }

        public User ModelUser
        {
            get => modelUser;
            set
            {
                SetProperty(ref modelUser, value);
            }
        }

        private bool check()
        {
            var chk = false;
            if (modelUser.username == null)
            {
                MessageBox.Show("ID can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelUser.password == null)
            {
                MessageBox.Show("Password can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else
            {
                chk = true;
            }
            return chk;
        }

        private async Task ReadDataAsync()
        {
            if (check())
            {
                OpenConnection();
                await Task.Delay(0);
                var query = $"SELECT * FROM User " +
                    $"WHERE username='{modelUser.username}' " +
                    $"AND password='{modelUser.password}'";
                var sqlcmd = new SQLiteCommand(query, Connection);

                var sqlresult = sqlcmd.ExecuteReader();

                if (sqlresult.HasRows)
                {
                    dataUser.Clear();
                    while (sqlresult.Read())
                    {
                        dataUser.Add(new User
                        {
                            id_user = sqlresult[0].ToString(),
                            username = sqlresult[1].ToString(),
                            role = sqlresult[3].ToString(),
                        });
                        App.SessionUser = sqlresult[1].ToString();
                        App.SessionRole = sqlresult[3].ToString();
                    }
                    App.View = new Views.Dashboard();
                    App.View.Show();
                    OnCallBack?.Invoke();
                }
                else
                {
                    MessageBox.Show("Login Failed Incorect Username or Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            CloseConnection();
        }
    }
}
