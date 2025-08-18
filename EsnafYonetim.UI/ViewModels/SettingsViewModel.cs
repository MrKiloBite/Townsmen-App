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

        public SettingsViewModel()
        {
            _settingsService = new SettingsService();
            KdvRate = _settingsService.KDVRate;
            PosCommissionRate = _settingsService.POSCommissionRate;
            IsDarkMode = Application.Current!.RequestedThemeVariant == ThemeVariant.Dark;
        }

        [RelayCommand]
        private void SaveSettings()
        {
            // Update theme
            Application.Current!.RequestedThemeVariant = IsDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;

            // Update config file
            var configPath = Path.Combine(AppContext.BaseDirectory, "config.txt");

            // Read all lines, update the ones we care about, or add them if they don't exist
            var settings = new Dictionary<string, string>();
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

            // Re-write the file preserving comments etc. is hard.
            // For now, we just write the key-value pairs we manage.
            // A more robust solution would parse and reconstruct the file.
            var newLines = settings.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList();

            // This is a simplification and will lose comments.
            // TODO: Implement a more robust config file writer.
            File.WriteAllLines(configPath, newLines);

            // Optionally, notify the user that settings are saved.
        }
    }
}
