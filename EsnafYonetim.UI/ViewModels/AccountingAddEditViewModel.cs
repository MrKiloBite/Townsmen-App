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
    public partial class AccountingAddEditViewModel : ViewModelBase
    {
        private readonly AccountingManager _accountingManager;
        private readonly MainWindowViewModel _mainVm;

        [ObservableProperty]
        private Muhasebe _transaction;

        private readonly bool _isNewTransaction;

        public string Title => _isNewTransaction ? "Yeni İşlem Ekle" : "İşlem Bilgilerini Düzenle";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AccountingAddEditViewModel(MainWindowViewModel mainVm, Muhasebe? transactionToEdit)
        {
            _accountingManager = new AccountingManager();
            _mainVm = mainVm;

            if (transactionToEdit == null)
            {
                _transaction = new Muhasebe { IslemTarihi = DateTime.Now };
                _isNewTransaction = true;
            }
            else
            {
                _transaction = transactionToEdit;
                _isNewTransaction = false;
            }

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async Task SaveAsync()
        {
            if (_isNewTransaction)
            {
                await _accountingManager.AddTransactionAsync(Transaction);
            }
            else
            {
                await _accountingManager.UpdateTransactionAsync(Transaction);
            }

            _mainVm.Content = new AccountingView { DataContext = new AccountingViewModel(_mainVm) };
        }

        private void Cancel()
        {
            _mainVm.Content = new AccountingView { DataContext = new AccountingViewModel(_mainVm) };
        }
    }
}
