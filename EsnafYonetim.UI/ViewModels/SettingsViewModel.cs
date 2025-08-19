using Avalonia;
using System;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Services;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        partial void OnIsDarkModeChanged(bool value)
        {
            Application.Current!.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
        }

        public SettingsViewModel()
        {
            _settingsService = new SettingsService();
            KdvRate = _settingsService.KDVRate;
            PosCommissionRate = _settingsService.POSCommissionRate;
            IsDarkMode = Application.Current!.RequestedThemeVariant == ThemeVariant.Dark;

            SaveSettingsCommand = new AsyncRelayCommand(SaveSettingsAsync);
        }

        private async Task SaveSettingsAsync()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "config.txt");

            var settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                settings = lines
                    .Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#") && line.Contains('='))
                    .Select(line => line.Split('=', 2))
                    .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim(), System.StringComparer.OrdinalIgnoreCase);
            }

            settings["kdv_orani"] = KdvRate.ToString(CultureInfo.InvariantCulture);
            settings["pos_komisyon_orani"] = PosCommissionRate.ToString(CultureInfo.InvariantCulture);

            var newLines = settings.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList();
            File.WriteAllLines(configPath, newLines);

            ConfirmationMessage = "Ayarlar başarıyla kaydedildi!";
            IsConfirmationVisible = true;
            await Task.Delay(3000);
            IsConfirmationVisible = false;
        }
    }
}
