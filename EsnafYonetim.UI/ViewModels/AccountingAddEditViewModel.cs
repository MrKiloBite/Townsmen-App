using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.BLL.Services;
using EsnafYonetim.Core.Models;
using System;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class AccountingAddEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Muhasebe _transaction;

        private readonly AccountingManager _accountingManager;
        private readonly SettingsService _settingsService;
        private readonly bool _isNew;

        public event Action? OnRequestClose;

        public AccountingAddEditViewModel(Muhasebe? transactionToEdit)
        {
            _accountingManager = new AccountingManager();
            _settingsService = new SettingsService();
            _isNew = (transactionToEdit == null);

            if (_isNew)
            {
                // Yeni bir işlem oluştur ve varsayılan oranlarla doldur.
                _transaction = new Muhasebe
                {
                    IslemTarihi = DateTime.Now,
                    OdemeDurumu = "Ödenmedi",
                    OdemeTipi = "Nakit",
                    UygulananKDVOrani = _settingsService.KDVRate,
                    UygulananKomisyonOrani = _settingsService.POSCommissionRate
                };
            }
            else
            {
                // Mevcut bir işlemi düzenle.
                _transaction = transactionToEdit!;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // TODO: Doğrulama mantığı ekle.

            if (_isNew)
            {
                await _accountingManager.AddTransactionAsync(Transaction);
            }
            else
            {
                await _accountingManager.UpdateTransactionAsync(Transaction);
            }

            OnRequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            OnRequestClose?.Invoke();
        }
    }
}
