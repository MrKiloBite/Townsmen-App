using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.Core.Models;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _content;

        private readonly DashboardViewModel _dashboardViewModel;
        private readonly CustomerListViewModel _customerListViewModel;
        private readonly StockListViewModel _stockListViewModel;
        private readonly AccountingViewModel _accountingViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly BildirimlerViewModel _bildirimlerViewModel;

        public MainWindowViewModel()
        {
            _dashboardViewModel = new DashboardViewModel();
            _customerListViewModel = new CustomerListViewModel();
            _stockListViewModel = new StockListViewModel();
            _accountingViewModel = new AccountingViewModel();
            _settingsViewModel = new SettingsViewModel();
            _bildirimlerViewModel = new BildirimlerViewModel();

            // Navigasyon event'lerine abone ol
            _customerListViewModel.AddOrEditCustomerRequested += OnAddOrEditCustomerRequested;
            _stockListViewModel.AddOrEditStockRequested += OnAddOrEditStockRequested;
            _accountingViewModel.AddOrEditTransactionRequested += OnAddOrEditTransactionRequested;

            _content = _dashboardViewModel;
        }

        // --- Müşteri Navigasyonu ---
        private void OnAddOrEditCustomerRequested(Musteri? customer)
        {
            var vm = new CustomerAddEditViewModel(customer);
            vm.OnRequestClose += OnCustomerAddEditViewRequestClose;
            Content = vm;
        }

        private void OnCustomerAddEditViewRequestClose()
        {
            Content = _customerListViewModel;
            _customerListViewModel.LoadCustomersCommand.Execute(null);
        }

        // --- Stok Navigasyonu ---
        private void OnAddOrEditStockRequested(Stok? stock)
        {
            var vm = new StockAddEditViewModel(stock);
            vm.OnRequestClose += OnStockAddEditViewRequestClose;
            Content = vm;
        }

        private void OnStockAddEditViewRequestClose()
        {
            Content = _stockListViewModel;
            _stockListViewModel.LoadStocksCommand.Execute(null);
        }

        // --- Muhasebe Navigasyonu ---
        private void OnAddOrEditTransactionRequested(Muhasebe? transaction)
        {
            var vm = new AccountingAddEditViewModel(transaction);
            vm.OnRequestClose += OnAccountingAddEditViewRequestClose;
            Content = vm;
        }

        private void OnAccountingAddEditViewRequestClose()
        {
            Content = _accountingViewModel;
            _accountingViewModel.LoadTransactionsCommand.Execute(null);
        }

        // --- Ana Menü Komutları ---
        [RelayCommand]
        private void ShowDashboard()
        {
            Content = _dashboardViewModel;
        }

        [RelayCommand]
        private void ShowCustomers()
        {
            Content = _customerListViewModel;
        }

        [RelayCommand]
        private void ShowStock()
        {
            Content = _stockListViewModel;
        }

        [RelayCommand]
        private void ShowAccounting()
        {
            Content = _accountingViewModel;
        }

        [RelayCommand]
        private void ShowBildirimler()
        {
            Content = _bildirimlerViewModel;
        }

        [RelayCommand]
        private void ShowSettings()
        {
            Content = _settingsViewModel;
        }
    }
}
