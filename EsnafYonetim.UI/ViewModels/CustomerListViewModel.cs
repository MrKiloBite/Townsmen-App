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
    public class CustomerListViewModel : ViewModelBase
    {
        private readonly CustomerManager _customerManager;
        private readonly MainWindowViewModel _mainVm;

        public ObservableCollection<Musteri> Customers { get; } = new();

        private Musteri? _selectedCustomer;
        public Musteri? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value))
                {
                    ((RelayCommand)EditCustomerCommand).NotifyCanExecuteChanged();
                    ((AsyncRelayCommand)DeleteCustomerCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand LoadCustomersCommand { get; }
        public ICommand AddNewCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }

        public CustomerListViewModel(MainWindowViewModel mainVm)
        {
            _customerManager = new CustomerManager();
            _mainVm = mainVm;

            LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
            AddNewCustomerCommand = new RelayCommand(AddNewCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, CanEditOrDeleteCustomer);
            DeleteCustomerCommand = new AsyncRelayCommand(DeleteCustomer, CanEditOrDeleteCustomer);

            _ = LoadCustomersAsync();
        }

        public async Task LoadCustomersAsync()
        {
            Customers.Clear();
            var customersFromDb = await _customerManager.GetAllCustomersAsync();
            foreach (var customer in customersFromDb)
            {
                Customers.Add(customer);
            }
        }

        private void AddNewCustomer()
        {
            _mainVm.Content = new CustomerAddEditView
            {
                DataContext = new CustomerAddEditViewModel(_mainVm, null)
            };
        }

        private void EditCustomer()
        {
            _mainVm.Content = new CustomerAddEditView
            {
                DataContext = new CustomerAddEditViewModel(_mainVm, SelectedCustomer)
            };
        }

        private async Task DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                await _customerManager.DeleteCustomerAsync(SelectedCustomer.Id);
                Customers.Remove(SelectedCustomer);
            }
        }

        private bool CanEditOrDeleteCustomer() => SelectedCustomer != null;
    }
}
