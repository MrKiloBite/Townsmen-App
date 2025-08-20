using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.UI.Views;
using System.Threading.Tasks;
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
            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowCustomersCommand = new AsyncRelayCommand(ShowCustomersAsync);
            ShowStockCommand = new AsyncRelayCommand(ShowStockAsync);
            ShowAccountingCommand = new AsyncRelayCommand(ShowAccountingAsync);
            ShowBildirimlerCommand = new AsyncRelayCommand(ShowBildirimlerAsync);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            // Set initial content
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            Content = new DashboardView();
        }

        private async Task ShowCustomersAsync()
        {
            var vm = new CustomerListViewModel(this);
            Content = new CustomerListView { DataContext = vm };
            await vm.InitializeAsync();
        }

        private async Task ShowStockAsync()
        {
            var vm = new StockListViewModel(this);
            Content = new StockListView { DataContext = vm };
            await vm.InitializeAsync();
        }

        private async Task ShowAccountingAsync()
        {
            var vm = new AccountingViewModel(this);
            Content = new AccountingView { DataContext = vm };
            await vm.InitializeAsync();
        }

        private async Task ShowBildirimlerAsync()
        {
            var vm = new BildirimlerViewModel(this);
            Content = new BildirimlerView { DataContext = vm };
            await vm.InitializeAsync();
        }

        private void ShowSettings()
        {
            Content = new SettingsView();
        }
    }
}
