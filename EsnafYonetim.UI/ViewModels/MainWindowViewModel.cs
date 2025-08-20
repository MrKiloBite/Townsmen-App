using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.UI.Views;
using System.Windows.Input;

namespace EsnafYonetim.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object? _content;

        public object? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowCustomersCommand { get; }
        public ICommand ShowStockCommand { get; }
        public ICommand ShowAccountingCommand { get; }
        public ICommand ShowBildirimlerCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public MainWindowViewModel()
        {
            // Komutları ayarla
            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowCustomersCommand = new RelayCommand(ShowCustomers);
            ShowStockCommand = new RelayCommand(ShowStock);
            ShowAccountingCommand = new RelayCommand(ShowAccounting);
            ShowBildirimlerCommand = new RelayCommand(ShowBildirimler);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            // Başlangıç içeriğini ayarla
            Content = new DashboardView();
        }

        private void ShowDashboard()
        {
            Content = new DashboardView();
        }

        private void ShowCustomers()
        {
            Content = new CustomerListView { DataContext = new CustomerListViewModel(this) };
        }

        private void ShowStock()
        {
            Content = new StockListView { DataContext = new StockListViewModel(this) };
        }

        private void ShowAccounting()
        {
            Content = new AccountingView { DataContext = new AccountingViewModel(this) };
        }

        private void ShowBildirimler()
        {
            Content = new BildirimlerView { DataContext = new BildirimlerViewModel(this) };
        }

        private void ShowSettings()
        {
            Content = new SettingsView();
        }
    }
}
