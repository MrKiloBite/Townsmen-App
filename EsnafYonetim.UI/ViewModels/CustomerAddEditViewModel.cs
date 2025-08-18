using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using System;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class CustomerAddEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Musteri _customer;

        private readonly CustomerManager _customerManager;
        private readonly bool _isNewCustomer;

        // This event can be used to signal that the view should be closed.
        public event Action? OnRequestClose;

        public CustomerAddEditViewModel(Musteri? customerToEdit)
        {
            _customerManager = new CustomerManager();
            if (customerToEdit == null)
            {
                // Yeni bir müşteri oluşturuluyor.
                _customer = new Musteri { OlusturmaTarihi = DateTime.Now, Status = "active" };
                _isNewCustomer = true;
            }
            else
            {
                // Mevcut bir müşteri düzenleniyor.
                _customer = customerToEdit;
                _isNewCustomer = false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // TODO: Add validation logic here.

            if (_isNewCustomer)
            {
                await _customerManager.AddCustomerAsync(Customer);
            }
            else
            {
                await _customerManager.UpdateCustomerAsync(Customer);
            }

            // İşlem tamamlandıktan sonra görünümü kapatmak için bir event tetikle.
            OnRequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            OnRequestClose?.Invoke();
        }
    }
}
