using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EsnafYonetim.BLL.Services
{
    public class SettingsService
    {
        private const string ConfigFileName = "config.txt";

        public string Username { get; private set; } = "default_user";
        public decimal KDVRate { get; private set; } = 0.20m;
        public decimal POSCommissionRate { get; private set; } = 0.015m;

        public SettingsService()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "config.txt");
            if (!File.Exists(configPath))
            {
                // Dosya yoksa varsayılan değerler zaten ayarlı, bir şey yapma.
                return;
            }

            var lines = File.ReadAllLines(configPath);
            var settings = lines
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"))
                .Select(line => line.Split('=', 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

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
        }
    }
}
