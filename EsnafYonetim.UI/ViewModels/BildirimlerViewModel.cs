using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EsnafYonetim.UI.Views;

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

            _ = LoadNotificationsAsync();
        }

        public async Task LoadNotificationsAsync()
        {
            Notifications.Clear();
            var notificationsFromDb = await _bildirimManager.GetAllAsync();
            foreach (var notification in notificationsFromDb)
            {
                Notifications.Add(notification);
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
                await _bildirimManager.DeleteAsync(SelectedNotification.Id);
                Notifications.Remove(SelectedNotification);
            }
        }

        private bool CanEditOrDeleteNotification() => SelectedNotification != null;
    }
}
