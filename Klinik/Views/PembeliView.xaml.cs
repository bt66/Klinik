using Klinik.ViewModels;
using System.Windows.Controls;

namespace Klinik.Views
{
    public partial class PembeliView : UserControl
    {
        private PembeliViewModel vm;
        public PembeliView()
        {
            InitializeComponent();
            vm = new PembeliViewModel();
            DataContext = vm;
        }
    }
}
