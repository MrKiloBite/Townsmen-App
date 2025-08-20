using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;

        [ObservableProperty]
        private decimal _kdvRate;

        [ObservableProperty]
        private decimal _posCommissionRate;

        [ObservableProperty]
        private bool _isDarkMode;

        [ObservableProperty]
        private string? _confirmationMessage;

        [ObservableProperty]
        private bool _isConfirmationVisible;

        public ICommand SaveSettingsCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public SettingsViewModel()
        {
            _settingsService = new SettingsService();
            _kdvRate = _settingsService.KDVRate;
            _posCommissionRate = _settingsService.POSCommissionRate;

            // Set initial theme state from service
            _isDarkMode = _settingsService.Theme.Equals("Dark", System.StringComparison.OrdinalIgnoreCase);
            // Apply the theme immediately
            Application.Current!.RequestedThemeVariant = _isDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;

            SaveSettingsCommand = new AsyncRelayCommand(SaveSettingsAsync);
            ToggleThemeCommand = new RelayCommand(ToggleTheme);
        }

        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode; // This will trigger the UI change via the setter
            Application.Current!.RequestedThemeVariant = IsDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;

            // Update service and save
            _settingsService.Theme = IsDarkMode ? "Dark" : "Light";
            _settingsService.SaveSettings();
        }

        private async Task SaveSettingsAsync()
        {
            // Update service with values from UI
            _settingsService.KDVRate = KdvRate;
            _settingsService.POSCommissionRate = PosCommissionRate;

            // Save all settings
            _settingsService.SaveSettings();

            ConfirmationMessage = "Ayarlar başarıyla kaydedildi!";
            IsConfirmationVisible = true;
            await Task.Delay(3000);
            IsConfirmationVisible = false;
        }
    }
}
