using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;

namespace Klinik.ViewModels
{
    class TransaksiViewModel : BaseViewModel
    {
        private ObservableCollection<Transaksi> dataTransaksi;
        private Transaksi modelTransaksi;

        public TransaksiViewModel()
        {
            dataTransaksi = new ObservableCollection<Transaksi>();
            modelTransaksi = new Transaksi();
            InsertCommand = new Command(async () => await InsertDataAsync());
            UpdateCommand = new Command(async () => await UpdateDataAsync());
            DeleteCommand = new Command(async () => await DeleteDataAsync());
            ReadCommand = new Command(async () => await ReadDataAsync());
            ReadCommand.Execute(null);
        }
        public ICommand ReadCommand { get; set; }
        public ICommand InsertCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<Transaksi> DataTransaksi
        {
            get => dataTransaksi;
            set
            {
                SetProperty(ref dataTransaksi, value);
            }
        }

        public Transaksi ModelTransaksi
        {
            get => modelTransaksi;
            set
            {
                SetProperty(ref modelTransaksi, value);
            }
        }
        private bool check()
        {
            var chk = false;
            if (modelTransaksi.id_transaksi == null)
            {
                MessageBox.Show("ID transaksi can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelTransaksi.id_pembeli == null)
            {
                MessageBox.Show("id pembeli can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelTransaksi.id_obat == null)
            {
                MessageBox.Show("id obat can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelTransaksi.jumlah == null)
            {
                MessageBox.Show("jumlah barang can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
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
            OpenConnection();
            await Task.Delay(0);
            var query = "SELECT * FROM Transaksi";
            var sqlcmd = new SQLiteCommand(query, Connection);

            var sqlresult = sqlcmd.ExecuteReader();

            if (sqlresult.HasRows)
            {
                dataTransaksi.Clear();
                while (sqlresult.Read())
                {
                    dataTransaksi.Add(new Transaksi
                    {
                        id_transaksi = sqlresult[0].ToString(),
                        id_pembeli = sqlresult[1].ToString(),
                        id_obat = sqlresult[2].ToString(),
                        jumlah = sqlresult[3].ToString(),
                        total_harga = sqlresult[3].ToString()
                    });
                }
            }
            CloseConnection();
        }

        private string hargasatuanObat;
        private string jumlahStockObat;
        private int total;

        private async Task InsertDataAsync()
        {
            try
            {
                if (true)
                {
                    OpenConnection();
                    await Task.Delay(0);
                    var query1 = $"SELECT harga_satuan,jumlah FROM Obat WHERE id_obat='{modelTransaksi.id_obat}'";
                    var sqlcmd1 = new SQLiteCommand(query1, Connection);
                    var sqlresult1 = sqlcmd1.ExecuteReader();

                    if (sqlresult1.HasRows)
                    {
                        dataTransaksi.Clear();
                        while (sqlresult1.Read())
                        {
                            hargasatuanObat = sqlresult1[0].ToString();
                            jumlahStockObat = sqlresult1[1].ToString();
                        }
                    }
                    CloseConnection();
                    MessageBox.Show(hargasatuanObat, "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show(jumlahStockObat, "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    total = int.Parse(hargasatuanObat) * int.Parse(modelTransaksi.jumlah);
                    //MessageBox.Show(total, "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                    OpenConnection();
                    await Task.Delay(0);
                    var query = $"INSERT INTO Transaksi " +
                        $"VALUES('{modelTransaksi.id_transaksi}','{modelTransaksi.id_pembeli}','{modelTransaksi.id_obat}','{modelTransaksi.jumlah}','{total}')";
                    //$"VALUES('T0004','P0002','B0003','{modelTransaksi.jumlah}','{total}')";
                    var sqlcmd = new SQLiteCommand(query, Connection);

                    var sqlresult = sqlcmd.ExecuteNonQuery();
                    CloseConnection();


                    // kurangi stock
                    total = int.Parse(jumlahStockObat) - int.Parse(modelTransaksi.jumlah);

                    OpenConnection();
                    await Task.Delay(0);
                    query = $"UPDATE Obat SET " +
                        $"jumlah='{total}' " +
                        $"WHERE id_obat = '{modelTransaksi.id_obat}'";
                    sqlcmd = new SQLiteCommand(query, Connection);
                    sqlresult = sqlcmd.ExecuteNonQuery();

                    CloseConnection();

                    MessageBox.Show("Sucessfully Input", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    await ReadDataAsync();

                }
            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async Task UpdateDataAsync()
        {
            try
            {
                OpenConnection();
                await Task.Delay(0);
                var query = $"UPDATE Transaksi SET " +
                    $"id_pembeli = '{modelTransaksi.id_pembeli}', " +
                    $"id_obat = '{modelTransaksi.id_obat}', " +
                    $"jumlah = '{modelTransaksi.jumlah}', " +
                    $"total_harga = '{total}' " +
                    $"WHERE id_transaksi = '{modelTransaksi.id_transaksi}'";
                //$"VALUES('{modelTransaksi.id_obat}','{modelTransaksi.nama_obat}','{modelTransaksi.khasiat}','{modelTransaksi.jumlah}','{modelTransaksi.harga_satuan}')";
                var sqlcmd = new SQLiteCommand(query, Connection);

                var sqlresult = sqlcmd.ExecuteNonQuery();
                CloseConnection();
                await ReadDataAsync();
                MessageBox.Show("Sucessfully Update", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SQLiteException msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteDataAsync()
        {
            try
            {
                if (MessageBox.Show($"yakin ingin menghapus '{modelTransaksi.id_transaksi}' ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    OpenConnection();
                    await Task.Delay(0);
                    var query = $"DELETE FROM Transaksi " +
                        $"WHERE id_transaksi = '{modelTransaksi.id_transaksi}'";
                    var sqlcmd = new SQLiteCommand(query, Connection);

                    var sqlresult = sqlcmd.ExecuteNonQuery();
                    CloseConnection();
                    await ReadDataAsync();
                    MessageBox.Show("Sucessfully Deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }
    }
}
