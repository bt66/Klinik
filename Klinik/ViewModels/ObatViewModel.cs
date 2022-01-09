using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;
using System.Collections.Generic;

namespace Klinik.ViewModels
{
    public class ObatViewModel : BaseViewModel
    {
        public ObatViewModel()
        {
            collection = new ObservableCollection<Obat>();
            model = new Obat();
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

        public ObservableCollection<Obat> Collection
        {
            get => collection;
            set
            {
                SetProperty(ref collection, value);
            }
        }

        public Obat Model
        {
            get => model;
            set
            {
                SetProperty(ref model, value);
            }
        }

        private ObservableCollection<Obat> collection;
        private Obat model;
        
        private async Task<bool> check()
        {
            var chk = false;
            if (model.id_obat == null)
            {
                MessageBox.Show("ID can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.nama_obat == null)
            {
                MessageBox.Show("nama obat can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.harga_satuan == null)
            {
                MessageBox.Show("harga satuan can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
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
                var query = $"INSERT INTO Obat " +
                            $"VALUES('{model.id_obat}','{model.nama_obat}','{model.khasiat}','{model.jumlah}','{model.harga_satuan}')";

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
        private async Task<IEnumerable<Obat>> ReadAsync()
        {
            var query = "SELECT * FROM [Obat]";
            if (OpenConnection())
            {
                var command = new SQLiteCommand(query, Connection);
                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    collection.Clear();
                    while (result.Read())
                    {
                        collection.Add(new Obat
                        {
                            id_obat = result[0].ToString(),
                            nama_obat = result[1].ToString(),
                            khasiat = result[2].ToString(),
                            jumlah = result[3].ToString(),
                            harga_satuan = result[4].ToString(),
                        });
                    }
                }
            }

            CloseConnection();
            return await Task.FromResult(collection);
        }

        private async Task UpdateAsync()
        {
            try
            {
                if (await check())
                {
                    var query = $"UPDATE Obat SET " +
                                $"nama_obat = '{model.nama_obat}', " +
                                $"khasiat = '{model.khasiat}', " +
                                $"jumlah = '{model.jumlah}', " +
                                $"harga_satuan = '{model.harga_satuan}' " +
                                $"WHERE id_obat = '{model.id_obat}'";
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
            //return await Task.FromResult(true);
        }

        private async Task DeleteAsync()
        {
            try
            {
                if (MessageBox.Show($"yakin ingin menghapus '{model.id_obat}' ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    {
                        
                        if (OpenConnection())
                        {
                            var query = $"DELETE FROM Obat " +
                                        $"WHERE id_obat = '{model.id_obat}'";
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
            //return await Task.FromResult(true);
        }
    }
}
