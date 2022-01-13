using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Data.SQLite;
using System.IO;

namespace Klinik.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SQLiteConnection Connection;
        public string StrConnection;

        public bool OpenConnection()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var path = projectDirectory + @"\klinik_fp.db";
            StrConnection = $"Data Source={path};Version=3";

            Connection = new SQLiteConnection(StrConnection);
            Connection.Open();

            return true;
        }

        public bool CloseConnection()
        {
            Connection.Close();
            return true;
        }

        public class Command : ICommand
        {
            public Command(Action cmdactionnone)
            {
                this.cmdactionnone = cmdactionnone;
            }

            public Command(Action<object> cmdaction)
            {
                this.cmdaction = cmdaction;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter != null)
                {
                    cmdaction(parameter);
                }
                else
                {
                    cmdactionnone();
                }
            }

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }

                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }

            private readonly Action cmdactionnone;
            private readonly Action<object> cmdaction;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null) return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
