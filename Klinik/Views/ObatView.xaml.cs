using Klinik.ViewModels;
using System.Windows.Controls;

namespace Klinik.Views
{
    public partial class ObatView : UserControl
    {
        private ObatViewModel vm;
        public ObatView()
        {
            InitializeComponent();
            vm = new ObatViewModel();
            DataContext = vm;
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

    }
}
