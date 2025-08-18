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

        public MainWindowViewModel()
        {
            _dashboardViewModel = new DashboardViewModel();
            _customerListViewModel = new CustomerListViewModel();
            _stockListViewModel = new StockListViewModel();
            _accountingViewModel = new AccountingViewModel();

            _customerListViewModel.AddOrEditCustomerRequested += OnAddOrEditCustomerRequested;

            _content = _dashboardViewModel;
        }

        private void OnAddOrEditCustomerRequested(Musteri? customer)
        {
            var addEditVm = new CustomerAddEditViewModel(customer);
            addEditVm.OnRequestClose += OnAddEditViewRequestClose;
            Content = addEditVm;
        }

        private void OnAddEditViewRequestClose()
        {
            Content = _customerListViewModel;
            _customerListViewModel.LoadCustomersCommand.Execute(null);
        }

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
    }
}
