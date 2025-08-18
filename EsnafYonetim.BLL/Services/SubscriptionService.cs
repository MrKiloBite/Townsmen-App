using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace EsnafYonetim.BLL.Services
{
    public class SubscriptionValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class SubscriptionService
    {
        // TODO: Bu URL, kullanıcının sağlayacağı gerçek genel GitHub raw URL'si ile değiştirilmelidir.
        private const string SubscriptionListUrl = "https://raw.githubusercontent.com/USER/REPO/main/subscription_list.txt";
        private const string ConfigFileName = "config.txt";

        public async Task<SubscriptionValidationResult> ValidateSubscriptionAsync()
        {
            if (!IsInternetAvailable())
            {
                return new SubscriptionValidationResult { IsValid = false, Message = "İnternet bağlantısı bulunamadı. Lütfen bağlantınızı kontrol edin." };
            }

            var username = GetUsernameFromConfig();
            if (string.IsNullOrWhiteSpace(username))
            {
                return new SubscriptionValidationResult { IsValid = false, Message = $"`{ConfigFileName}` dosyası bulunamadı veya 'username' alanı boş." };
            }

            try
            {
                using HttpClient client = new HttpClient();
                var subscriptionListContent = await client.GetStringAsync(SubscriptionListUrl);

                var lines = subscriptionListContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var userLine = lines.FirstOrDefault(line => line.Trim().StartsWith(username, StringComparison.OrdinalIgnoreCase));

                if (userLine == null)
                {
                    return new SubscriptionValidationResult { IsValid = false, Message = "Kullanıcı abonelik listesinde bulunamadı." };
                }

                var parts = userLine.Split(' ');
                if (parts.Length == 2 && parts[1] == "1")
                {
                    return new SubscriptionValidationResult { IsValid = true, Message = "Abonelik geçerli." };
                }

                return new SubscriptionValidationResult { IsValid = false, Message = "Ödemeniz gerçekleşmediği için kullanamazsınız, lütfen ödemenizi yapıp bizi bilgilendirin ve tekrar deneyin." };
            }
            catch (HttpRequestException ex)
            {
                return new SubscriptionValidationResult { IsValid = false, Message = $"Abonelik listesine erişilemedi: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new SubscriptionValidationResult { IsValid = false, Message = $"Beklenmedik bir hata oluştu: {ex.Message}" };
            }
        }

        private string? GetUsernameFromConfig()
        {
            if (!File.Exists(ConfigFileName)) return null;

            var lines = File.ReadAllLines(ConfigFileName);
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("username=", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Split('=')[1].Trim();
                }
            }
            return null;
        }

        private bool IsInternetAvailable()
        {
            try
            {
                // Google'ın DNS sunucusuna ping atmayı dene
                using var ping = new Ping();
                var reply = ping.Send("8.8.8.8", 2000); // 2 saniye timeout
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
