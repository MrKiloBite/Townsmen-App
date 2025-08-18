using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class AccountingViewModel : ViewModelBase
    {
        private readonly AccountingManager _accountingManager;

        public ObservableCollection<Muhasebe> Transactions { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditTransactionCommand))]
        private Muhasebe? _selectedTransaction;

        public event Action<Muhasebe?>? AddOrEditTransactionRequested;

        public AccountingViewModel()
        {
            _accountingManager = new AccountingManager();
            _ = LoadTransactionsAsync();
        }

        [RelayCommand]
        public async Task LoadTransactionsAsync()
        {
            Transactions.Clear();
            var transactionsFromDb = await _accountingManager.GetAllTransactionsAsync();
            foreach (var transaction in transactionsFromDb)
            {
                Transactions.Add(transaction);
            }
        }

        [RelayCommand]
        private void AddNewTransaction()
        {
            AddOrEditTransactionRequested?.Invoke(null);
        }

        [RelayCommand(CanExecute = nameof(CanEditTransaction))]
        private void EditTransaction()
        {
            AddOrEditTransactionRequested?.Invoke(SelectedTransaction);
        }

        private bool CanEditTransaction() => SelectedTransaction != null;
    }
}
