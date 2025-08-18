using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class BildirimlerViewModel : ViewModelBase
    {
        private readonly BildirimManager _bildirimManager;

        public ObservableCollection<Bildirim> Notifications { get; } = new();

        public BildirimlerViewModel()
        {
            _bildirimManager = new BildirimManager();
            _ = LoadNotificationsAsync();
        }

        [RelayCommand]
        public async Task LoadNotificationsAsync()
        {
            Notifications.Clear();
            var notifications = await _bildirimManager.GetAllAsync();
            foreach (var notification in notifications)
            {
                Notifications.Add(notification);
            }
        }

        [RelayCommand]
        private void AddNewNotification()
        {
            // TODO: Navigate to an Add/Edit view for notifications.
        }
    }
}
