using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;
using EsnafYonetim.UI.Views;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class CustomerAddEditViewModel : ViewModelBase
    {
        private readonly CustomerManager _customerManager;
        private readonly MainWindowViewModel _mainVm;
        private readonly Musteri _customer;
        private bool _isNewCustomer;

        [ObservableProperty] private string? _adSoyad;
        [ObservableProperty] private string? _telefon;
        [ObservableProperty] private string? _eposta;
        [ObservableProperty] private string? _adres;
        [ObservableProperty] private string? _notlar;
        [ObservableProperty] private string _status;

        public string Title => _isNewCustomer ? "Yeni Müşteri Ekle" : "Müşteri Bilgilerini Düzenle";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CustomerAddEditViewModel(MainWindowViewModel mainVm, Musteri? customer)
        {
            _customerManager = new CustomerManager();
            _mainVm = mainVm;

            if (customer == null)
            {
                _customer = new Musteri();
                _isNewCustomer = true;
                _status = "active"; // Default status
            }
            else
            {
                _customer = customer;
                _isNewCustomer = false;
                _adSoyad = _customer.AdSoyad;
                _telefon = _customer.Telefon;
                _eposta = _customer.Eposta;
                _adres = _customer.Adres;
                _notlar = _customer.Notlar;
                _status = _customer.Status;
            }

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async Task SaveAsync()
        {
            _customer.AdSoyad = AdSoyad;
            _customer.Telefon = Telefon;
            _customer.Eposta = Eposta;
            _customer.Adres = Adres;
            _customer.Notlar = Notlar;
            _customer.Status = Status;

            if (_isNewCustomer)
            {
                await _customerManager.AddCustomerAsync(_customer);
            }
            else
            {
                await _customerManager.UpdateCustomerAsync(_customer);
            }

            _mainVm.Content = new CustomerListView { DataContext = new CustomerListViewModel(_mainVm) };
        }

        private void Cancel()
        {
            _mainVm.Content = new CustomerListView { DataContext = new CustomerListViewModel(_mainVm) };
        }
    }
}
