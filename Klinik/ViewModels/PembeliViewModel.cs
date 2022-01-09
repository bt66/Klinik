using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;
using System.Collections.Generic;

namespace Klinik.ViewModels
{
    public class PembeliViewModel : BaseViewModel
    {
        public PembeliViewModel()
        {
            collection = new ObservableCollection<Pembeli>();
            model = new Pembeli();
            CreateCommand = new Command(async () => await CreateAsync());
            UpdateCommand = new Command(async () => await UpdateAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            ReadCommand = new Command(async () => await ReadAsync());
            ReadCommand.Execute(null);
        }

        public ICommand CreateCommand { get; set; }
        public ICommand ReadCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<Pembeli> Collection
        {
            get => collection;
            set
            {
                SetProperty(ref collection, value);
            }
        }

        public Pembeli Model
        {
            get => model;
            set
            {
                SetProperty(ref model, value);
            }
        }

        private ObservableCollection<Pembeli> collection;
        private Pembeli model;

        private async Task<bool> check()
        {
            var chk = false;
            if (model.id_pembeli == null)
            {
                MessageBox.Show("ID pembeli can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.nama_pembeli == null)
            {
                MessageBox.Show("nama pembeli can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.alamat == null)
            {
                MessageBox.Show("Alamat can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.telepon == null)
            {
                MessageBox.Show("Nomor telepon can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                chk = true;
            }
            return await Task.FromResult(chk);
        }
        private async Task<bool> CreateAsync()
        {
            try
            {
                if (await check())
                {
                    var query = $"INSERT INTO Pembeli " +
                                $"VALUES('{model.id_pembeli}','{model.nama_pembeli}','{model.alamat}','{model.telepon}')";

                    if (OpenConnection())
                    {
                        var command = new SQLiteCommand(query, Connection);
                        command.ExecuteNonQuery();
                    }
                    CloseConnection();
                    MessageBox.Show("Sucessfully Input", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await ReadAsync();
            return await Task.FromResult(true);
        }
        private async Task<IEnumerable<Pembeli>> ReadAsync()
        {
            var query = "SELECT * FROM [Pembeli]";
            if (OpenConnection())
            {
                var command = new SQLiteCommand(query, Connection);
                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    collection.Clear();
                    while (result.Read())
                    {
                        collection.Add(new Pembeli
                        {
                            id_pembeli = result[0].ToString(),
                            nama_pembeli = result[1].ToString(),
                            alamat = result[2].ToString(),
                            telepon = result[3].ToString(),
                        });
                    }
                }
            }

            CloseConnection();
            return await Task.FromResult(collection);
        }

        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (await check())
                {
                    var query = $"UPDATE Obat SET " +
                                $"nama_pembeli = '{model.nama_pembeli}', " +
                                $"alamat = '{model.alamat}', " +
                                $"Telepon = '{model.telepon}' " +
                                $"WHERE id_pembeli = '{model.id_pembeli}'";
                    if (OpenConnection())
                    {
                        var command = new SQLiteCommand(query, Connection);
                        command.ExecuteNonQuery();
                    }
                    CloseConnection();
                    MessageBox.Show("Sucessfully Update", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    await ReadAsync();
                }

            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return await Task.FromResult(true);
        }

        private async Task<bool> DeleteAsync()
        {
            try
            {
                if (MessageBox.Show($"yakin ingin menghapus '{model.id_pembeli}' ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    {

                        if (OpenConnection())
                        {
                            var query = $"DELETE FROM Pembeli " +
                                        $"WHERE id_pembeli = '{model.id_pembeli}'";
                            var command = new SQLiteCommand(query, Connection);
                            command.ExecuteNonQuery();
                        }
                        CloseConnection();
                        MessageBox.Show("Sucessfully Deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        await ReadAsync();
                    }
                }
                else
                {
                    MessageBox.Show("Hapus di batalkan", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return await Task.FromResult(true);
        }
    }
}
