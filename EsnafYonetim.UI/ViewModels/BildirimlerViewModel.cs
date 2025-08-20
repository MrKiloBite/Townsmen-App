using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EsnafYonetim.UI.Views;
using System;

namespace EsnafYonetim.UI.ViewModels
{
    public class BildirimlerViewModel : ViewModelBase
    {
        private readonly BildirimManager _bildirimManager;
        private readonly MainWindowViewModel _mainVm;

        public ObservableCollection<Bildirim> Notifications { get; } = new();

        private Bildirim? _selectedNotification;
        public Bildirim? SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                if (SetProperty(ref _selectedNotification, value))
                {
                    ((RelayCommand)EditNotificationCommand).NotifyCanExecuteChanged();
                    ((AsyncRelayCommand)DeleteNotificationCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand LoadNotificationsCommand { get; }
        public ICommand AddNewNotificationCommand { get; }
        public ICommand EditNotificationCommand { get; }
        public ICommand DeleteNotificationCommand { get; }

        public BildirimlerViewModel(MainWindowViewModel mainVm)
        {
            _bildirimManager = new BildirimManager();
            _mainVm = mainVm;

            LoadNotificationsCommand = new AsyncRelayCommand(LoadNotificationsAsync);
            AddNewNotificationCommand = new RelayCommand(AddNewNotification);
            EditNotificationCommand = new RelayCommand(EditNotification, CanEditOrDeleteNotification);
            DeleteNotificationCommand = new AsyncRelayCommand(DeleteNotification, CanEditOrDeleteNotification);
        }

        public async Task InitializeAsync()
        {
            await LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            IsBusy = true;
            try
            {
                Notifications.Clear();
                var notificationsFromDb = await _bildirimManager.GetAllAsync();
                foreach (var notification in notificationsFromDb)
                {
                    Notifications.Add(notification);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load notifications: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddNewNotification()
        {
            _mainVm.Content = new BildirimAddEditView
            {
                DataContext = new BildirimAddEditViewModel(_mainVm, null)
            };
        }

        private void EditNotification()
        {
            _mainVm.Content = new BildirimAddEditView
            {
                DataContext = new BildirimAddEditViewModel(_mainVm, SelectedNotification)
            };
        }

        private async Task DeleteNotification()
        {
            if (SelectedNotification != null)
            {
                IsBusy = true;
                try
                {
                    await _bildirimManager.DeleteAsync(SelectedNotification.Id);
                    Notifications.Remove(SelectedNotification);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanEditOrDeleteNotification() => SelectedNotification != null;
    }
}
