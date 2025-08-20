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
        }

        // This method will be called from MainWindowViewModel after the view is created.
        public async Task InitializeAsync()
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            IsBusy = true;
            try
            {
                Customers.Clear();
                var customersFromDb = await _customerManager.GetAllCustomersAsync();
                foreach (var customer in customersFromDb)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                // TODO: Show a message to the user
                Console.WriteLine($"Failed to load customers: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
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
                IsBusy = true;
                try
                {
                    await _customerManager.DeleteCustomerAsync(SelectedCustomer.Id);
                    Customers.Remove(SelectedCustomer);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanEditOrDeleteCustomer() => SelectedCustomer != null;
    }
}
