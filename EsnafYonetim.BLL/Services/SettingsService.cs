using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EsnafYonetim.BLL.Services
{
    public class SettingsService
    {
        private readonly string _configPath;

        public string Username { get; set; } = "default_user";
        public decimal KDVRate { get; set; } = 0.20m;
        public decimal POSCommissionRate { get; set; } = 0.015m;
        public string Theme { get; set; } = "Light"; // Default theme

        public SettingsService()
        {
            _configPath = Path.Combine(AppContext.BaseDirectory, "config.txt");
            LoadSettings();
        }

        public void LoadSettings()
        {
            if (!File.Exists(_configPath))
            {
                // If file doesn't exist, save default settings and continue
                SaveSettings();
                return;
            }

            var settings = File.ReadAllLines(_configPath)
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"))
                .Select(line => line.Split(new[] { '=' }, 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim(), StringComparer.OrdinalIgnoreCase);

            if (settings.TryGetValue("username", out var usernameValue))
            {
                Username = usernameValue;
            }
            if (settings.TryGetValue("kdv_orani", out var kdvValue) &&
                decimal.TryParse(kdvValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var kdv))
            {
                KDVRate = kdv;
            }
            if (settings.TryGetValue("pos_komisyon_orani", out var posValue) &&
                decimal.TryParse(posValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var pos))
            {
                POSCommissionRate = pos;
            }
            if (settings.TryGetValue("theme", out var themeValue))
            {
                Theme = themeValue;
            }
        }

        public void SaveSettings()
        {
            var settings = new Dictionary<string, string>
            {
                { "username", Username },
                { "kdv_orani", KDVRate.ToString(CultureInfo.InvariantCulture) },
                { "pos_komisyon_orani", POSCommissionRate.ToString(CultureInfo.InvariantCulture) },
                { "theme", Theme }
            };

            var lines = new List<string>
            {
                "# Esnaf Yönetim Uygulaması Ayar Dosyası",
                "# Bu dosyayı manuel olarak düzenleyebilirsiniz.",
                ""
            };

            foreach (var setting in settings)
            {
                lines.Add($"{setting.Key} = {setting.Value}");
            }

            File.WriteAllLines(_configPath, lines);
        }
    }
}
