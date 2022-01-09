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
        private ObservableCollection<User> collection;
        private User model;
        public LoginViewModel()
        {
            collection = new ObservableCollection<User>();
            model = new User();
            ReadCommand = new Command(async () => await ReadAsync());
        }
        public ICommand ReadCommand { get; set; }
        public event Action OnCallBack;
        public ObservableCollection<User> Collection
        {
            get => collection;
            set
            {
                SetProperty(ref collection, value);
            }
        }

        public User Model
        {
            get => model;
            set
            {
                SetProperty(ref model, value);
            }
        }

        private bool check()
        {
            var chk = false;
            if (model.username == null)
            {
                MessageBox.Show("ID can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.password == null)
            {
                MessageBox.Show("Password can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                chk = true;
            }
            return chk;
        }

        private async Task ReadAsync()
        {
            if (check())
            {
                OpenConnection();
                await Task.Delay(0);
                var query = $"SELECT * FROM User " +
                    $"WHERE username='{model.username}' " +
                    $"AND password='{model.password}'";
                var command = new SQLiteCommand(query, Connection);

                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    collection.Clear();
                    while (result.Read())
                    {
                        collection.Add(new User
                        {
                            id_user = result[0].ToString(),
                            username = result[1].ToString(),
                            role = result[3].ToString(),
                        });
                        App.SessionUser = result[1].ToString();
                        App.SessionRole = result[3].ToString();
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
