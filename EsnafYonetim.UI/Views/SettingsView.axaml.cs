using Avalonia.Controls;
using EsnafYonetim.UI.ViewModels;

namespace EsnafYonetim.UI.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
