using Klinik.ViewModels;
using System.Windows.Controls;

namespace Klinik.Views
{
    public partial class TransaksiView : UserControl
    {
        private TransaksiViewModel vm;
        public TransaksiView()
        {
            InitializeComponent();
            vm = new TransaksiViewModel();
            DataContext = vm;
        }
    }
}
