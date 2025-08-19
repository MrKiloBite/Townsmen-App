using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class BildirimlerViewModel : ViewModelBase
    {
        private readonly BildirimManager _bildirimManager;

        public ObservableCollection<Bildirim> Notifications { get; } = new();

        [ObservableProperty]
        private Bildirim? _selectedNotification;

        public event Action<Bildirim?>? AddOrEditNotificationRequested;

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
            AddOrEditNotificationRequested?.Invoke(null);
        }

        // TODO: Add Edit and Delete commands
    }
}
