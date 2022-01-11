using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;
using System.Collections.Generic;

namespace Klinik.ViewModels
{
    public class TransaksiViewModel : BaseViewModel
    {
        public TransaksiViewModel()
        {
            collection = new ObservableCollection<Transaksi>();
            model = new Transaksi();
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

        public ObservableCollection<Transaksi> Collection
        {
            get => collection;
            set
            {
                SetProperty(ref collection, value);
            }
        }

        public Transaksi Model
        {
            get => model;
            set
            {
                SetProperty(ref model, value);
            }
        }

        private ObservableCollection<Transaksi> collection;
        private Transaksi model;

        private async Task<bool> check()
        {
            await Task.Delay(0);
            var chk = false;
            if (model.id_transaksi == null)
            {
                MessageBox.Show("ID transaksi can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.id_pembeli == null)
            {
                MessageBox.Show("id pembeli can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.id_obat == null)
            {
                MessageBox.Show("id obat can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (model.jumlah == null)
            {
                MessageBox.Show("jumlah barang can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                chk = true;
            }
            return await Task.FromResult(chk);
        }

        private string hargasatuanObat;
        private string jumlahStockObat;
        private int total;

        private async Task<bool> CreateAsync()
        {
            try
            {
                if (await check())
                {
                    var query = $"SELECT harga_satuan,jumlah FROM Obat WHERE id_obat='{model.id_obat}'";

                    if (OpenConnection())
                    {
                        var command = new SQLiteCommand(query, Connection);
                        var result = command.ExecuteReader();

                        if (result.HasRows)
                        {
                            collection.Clear();
                            while (result.Read())
                            {
                                hargasatuanObat = result[0].ToString();
                                jumlahStockObat = result[1].ToString();
                            }
                        }
                        if (int.Parse(jumlahStockObat) <= 0)
                        {
                            MessageBox.Show("Stok Obat Habis", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            CloseConnection();
                            await ReadAsync();
                            return await Task.FromResult(true);
                        }
                        total = int.Parse(hargasatuanObat) * int.Parse(model.jumlah);

                        await Task.Delay(0);

                        query = $"INSERT INTO Transaksi " +
                                $"VALUES('{model.id_transaksi}','{model.id_pembeli}','{model.id_obat}','{model.jumlah}','{total}')";
                        command = new SQLiteCommand(query, Connection);
                        command.ExecuteNonQuery();

                        // kurangi stock
                        total = int.Parse(jumlahStockObat) - int.Parse(model.jumlah);

                        await Task.Delay(0);
                        query = $"UPDATE Obat SET " +
                                $"jumlah='{total}' " +
                                $"WHERE id_obat = '{model.id_obat}'";
                        command = new SQLiteCommand(query, Connection);
                        command.ExecuteNonQuery();
                    }
                    CloseConnection();
                    MessageBox.Show("Sucessfully Input", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    await ReadAsync();
                }

            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return await Task.FromResult(true);
        }
        private async Task<IEnumerable<Transaksi>> ReadAsync()
        {
            var query = "SELECT * FROM [Transaksi]";
            if (OpenConnection())
            {
                var command = new SQLiteCommand(query, Connection);
                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    collection.Clear();
                    while (result.Read())
                    {
                        collection.Add(new Transaksi
                        {
                            id_transaksi = result[0].ToString(),
                            id_pembeli = result[1].ToString(),
                            id_obat = result[2].ToString(),
                            jumlah = result[3].ToString(),
                            total_harga = result[4].ToString()
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
                    var query = $"UPDATE Transaksi SET " +
                                $"id_pembeli = '{model.id_pembeli}', " +
                                $"id_obat = '{model.id_obat}', " +
                                $"jumlah = '{model.jumlah}', " +
                                $"total_harga = '{total}' " +
                                $"WHERE id_transaksi = '{model.id_transaksi}'";
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
                            var query = $"DELETE FROM Transaksi " +
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
