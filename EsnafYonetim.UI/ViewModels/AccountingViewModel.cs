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
    public class AccountingViewModel : ViewModelBase
    {
        private readonly AccountingManager _accountingManager;
        private readonly MainWindowViewModel _mainVm;

        public ObservableCollection<Muhasebe> Transactions { get; } = new();

        private Muhasebe? _selectedTransaction;
        public Muhasebe? SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (SetProperty(ref _selectedTransaction, value))
                {
                    ((RelayCommand)EditTransactionCommand).NotifyCanExecuteChanged();
                    ((AsyncRelayCommand)DeleteTransactionCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand LoadTransactionsCommand { get; }
        public ICommand AddNewTransactionCommand { get; }
        public ICommand EditTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }

        public AccountingViewModel(MainWindowViewModel mainVm)
        {
            _accountingManager = new AccountingManager();
            _mainVm = mainVm;

            LoadTransactionsCommand = new AsyncRelayCommand(LoadTransactionsAsync);
            AddNewTransactionCommand = new RelayCommand(AddNewTransaction);
            EditTransactionCommand = new RelayCommand(EditTransaction, CanEditOrDeleteTransaction);
            DeleteTransactionCommand = new AsyncRelayCommand(DeleteTransaction, CanEditOrDeleteTransaction);
        }

        public async Task InitializeAsync()
        {
            await LoadTransactionsAsync();
        }

        private async Task LoadTransactionsAsync()
        {
            IsBusy = true;
            try
            {
                Transactions.Clear();
                var transactionsFromDb = await _accountingManager.GetAllTransactionsAsync();
                foreach (var transaction in transactionsFromDb)
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load transactions: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddNewTransaction()
        {
            _mainVm.Content = new AccountingAddEditView
            {
                DataContext = new AccountingAddEditViewModel(_mainVm, null)
            };
        }

        private void EditTransaction()
        {
            _mainVm.Content = new AccountingAddEditView
            {
                DataContext = new AccountingAddEditViewModel(_mainVm, SelectedTransaction)
            };
        }

        private async Task DeleteTransaction()
        {
            if (SelectedTransaction != null)
            {
                IsBusy = true;
                try
                {
                    await _accountingManager.DeleteTransactionAsync(SelectedTransaction.Id);
                    Transactions.Remove(SelectedTransaction);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanEditOrDeleteTransaction() => SelectedTransaction != null;
    }
}
