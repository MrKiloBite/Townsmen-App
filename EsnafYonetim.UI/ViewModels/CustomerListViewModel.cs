using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class CustomerListViewModel : ViewModelBase
    {
        private readonly CustomerManager _customerManager;

        public ObservableCollection<Musteri> Customers { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditCustomerCommand))]
        private Musteri? _selectedCustomer;

        // Bu event, MainWindowViewModel tarafından dinlenecek ve navigasyonu tetikleyecek.
        public event Action<Musteri?>? AddOrEditCustomerRequested;

        public CustomerListViewModel()
        {
            _customerManager = new CustomerManager();
            _ = LoadCustomersAsync();
        }

        [RelayCommand]
        public async Task LoadCustomersAsync()
        {
            Customers.Clear();
            var customersFromDb = await _customerManager.GetAllCustomersAsync();
            foreach (var customer in customersFromDb)
            {
                Customers.Add(customer);
            }
        }

        [RelayCommand]
        private void AddNewCustomer()
        {
            // Yeni müşteri ekleme isteği. Parametre null olduğu için yeni kayıt olduğu anlaşılacak.
            AddOrEditCustomerRequested?.Invoke(null);
        }

        [RelayCommand(CanExecute = nameof(CanEditCustomer))]
        private void EditCustomer()
        {
            // Seçili müşteriyi düzenleme isteği.
            AddOrEditCustomerRequested?.Invoke(SelectedCustomer);
        }

        private bool CanEditCustomer() => SelectedCustomer != null;
    }
}
