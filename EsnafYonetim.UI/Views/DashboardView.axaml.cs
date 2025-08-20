using Avalonia.Controls;
using EsnafYonetim.UI.ViewModels;

namespace EsnafYonetim.UI.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}
