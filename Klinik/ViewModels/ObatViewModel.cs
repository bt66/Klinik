using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using Klinik.Models;

namespace Klinik.ViewModels
{
    public class ObatViewModel : BaseViewModel
    {
        private ObservableCollection<Obat> dataObat;
        private Obat modelObat;
        public ObatViewModel()
        {
            dataObat = new ObservableCollection<Obat>();
            modelObat = new Obat();
            InsertCommand = new Command(async () => await InsertDataAsync());
            UpdateCommand = new Command(async () => await UpdateDataAsync());
            DeleteCommand = new Command(async () => await DeleteDataAsync());
            ReadCommand = new Command(async () => await ReadDataAsync());
            ReadCommand.Execute(null);
        }

        public ICommand InsertCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReadCommand { get; set; }

        public ObservableCollection<Obat> DataObat
        {
            get => dataObat;
            set
            {
                SetProperty(ref dataObat, value);
            }
        }

        public Obat ModelObat
        {
            get => modelObat;
            set
            {
                SetProperty(ref modelObat, value);
            }
        }

        private async Task ReadDataAsync()
        {
            OpenConnection();
            await Task.Delay(0);
            var query = "SELECT * FROM [Obat]";
            var sqlcmd = new SQLiteCommand(query, Connection);

            var sqlresult = sqlcmd.ExecuteReader();

            if (sqlresult.HasRows)
            {
                dataObat.Clear();
                while (sqlresult.Read())
                {
                    dataObat.Add(new Obat
                    {
                        id_obat = sqlresult[0].ToString(),
                        nama_obat = sqlresult[1].ToString(),
                        khasiat = sqlresult[2].ToString(),
                        jumlah = sqlresult[3].ToString(),
                        harga_satuan = sqlresult[4].ToString(),
                    });
                }
            }
            CloseConnection();
        }
        private bool check()
        {
            var chk = false;
            if (modelObat.id_obat == null)
            {
                MessageBox.Show("ID can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelObat.nama_obat == null)
            {
                MessageBox.Show("nama obat can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else if (modelObat.harga_satuan == null)
            {
                MessageBox.Show("harga satuan can't null !", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                chk = false;
            }
            else
            {
                chk = true;
            }
            return chk;
        }

        private async Task InsertDataAsync()
        {
            try
            {
                if (check())
                {
                    OpenConnection();
                    await Task.Delay(0);
                    var query = $"INSERT INTO Obat " +
                        $"VALUES('{modelObat.id_obat}','{modelObat.nama_obat}','{modelObat.khasiat}','{modelObat.jumlah}','{modelObat.harga_satuan}')";
                    //$"VALUES('{modelObat.id_obat}','{modelObat.nama_obat}','{modelObat.khasiat}','{modelObat.jumlah}','{modelObat.harga_satuan}')";
                    var sqlcmd = new SQLiteCommand(query, Connection);

                    var sqlresult = sqlcmd.ExecuteNonQuery();
                    CloseConnection();
                    await ReadDataAsync();
                    MessageBox.Show("Sucessfully Input", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (check())
                {
                    OpenConnection();
                    await Task.Delay(0);
                    var query = $"UPDATE Obat SET " +
                        $"nama_obat = '{modelObat.nama_obat}', " +
                        $"khasiat = '{modelObat.khasiat}', " +
                        $"jumlah = '{modelObat.jumlah}', " +
                        $"harga_satuan = '{modelObat.harga_satuan}' " +
                        $"WHERE id_obat = '{modelObat.id_obat}'";
                    //$"VALUES('{modelObat.id_obat}','{modelObat.nama_obat}','{modelObat.khasiat}','{modelObat.jumlah}','{modelObat.harga_satuan}')";
                    var sqlcmd = new SQLiteCommand(query, Connection);

                    var sqlresult = sqlcmd.ExecuteNonQuery();
                    CloseConnection();
                    await ReadDataAsync();
                    MessageBox.Show("Sucessfully Update", "Data Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }

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
                if (MessageBox.Show($"yakin ingin menghapus '{modelObat.id_obat}' ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    {
                        OpenConnection();
                        await Task.Delay(0);
                        var query = $"DELETE FROM Obat " +
                            $"WHERE id_obat = '{modelObat.id_obat}'";
                        //$"VALUES('{modelObat.id_obat}','{modelObat.nama_obat}','{modelObat.khasiat}','{modelObat.jumlah}','{modelObat.harga_satuan}')";
                        var sqlcmd = new SQLiteCommand(query, Connection);

                        var sqlresult = sqlcmd.ExecuteNonQuery();
                        CloseConnection();
                        await ReadDataAsync();
                        MessageBox.Show("Sucessfully Deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
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

        }
    }
}
