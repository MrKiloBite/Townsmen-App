using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EsnafYonetim.UI.Views;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class BildirimAddEditViewModel : ViewModelBase
    {
        private readonly BildirimManager _bildirimManager;
        private readonly MainWindowViewModel _mainVm;

        [ObservableProperty]
        private Bildirim _notification;

        private readonly bool _isNewNotification;

        public string Title => _isNewNotification ? "Yeni Bildirim Ekle" : "Bildirim Bilgilerini Düzenle";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BildirimAddEditViewModel(MainWindowViewModel mainVm, Bildirim? notificationToEdit)
        {
            _bildirimManager = new BildirimManager();
            _mainVm = mainVm;

            if (notificationToEdit == null)
            {
                // Corrected properties based on Bildirim.cs
                _notification = new Bildirim { TetiklenmeZamani = DateTime.Now, Aktif = true, Mesaj = "" };
                _isNewNotification = true;
            }
            else
            {
                _notification = notificationToEdit;
                _isNewNotification = false;
            }

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async Task SaveAsync()
        {
            if (_isNewNotification)
            {
                // Corrected method name
                await _bildirimManager.AddAsync(Notification);
            }
            else
            {
                // Corrected method name
                await _bildirimManager.UpdateAsync(Notification);
            }

            _mainVm.Content = new BildirimlerView { DataContext = new BildirimlerViewModel(_mainVm) };
        }

        private void Cancel()
        {
            _mainVm.Content = new BildirimlerView { DataContext = new BildirimlerViewModel(_mainVm) };
        }
    }
}
