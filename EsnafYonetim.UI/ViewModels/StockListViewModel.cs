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
    public class StockListViewModel : ViewModelBase
    {
        private readonly InventoryManager _inventoryManager;
        private readonly MainWindowViewModel _mainVm;

        public ObservableCollection<Stok> Stocks { get; } = new();

        private Stok? _selectedStock;
        public Stok? SelectedStock
        {
            get => _selectedStock;
            set
            {
                if (SetProperty(ref _selectedStock, value))
                {
                    ((RelayCommand)EditStockCommand).NotifyCanExecuteChanged();
                    ((AsyncRelayCommand)DeleteStockCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand LoadStocksCommand { get; }
        public ICommand AddNewStockCommand { get; }
        public ICommand EditStockCommand { get; }
        public ICommand DeleteStockCommand { get; }

        public StockListViewModel(MainWindowViewModel mainVm)
        {
            _inventoryManager = new InventoryManager();
            _mainVm = mainVm;

            LoadStocksCommand = new AsyncRelayCommand(LoadStocksAsync);
            AddNewStockCommand = new RelayCommand(AddNewStock);
            EditStockCommand = new RelayCommand(EditStock, CanEditOrDeleteStock);
            DeleteStockCommand = new AsyncRelayCommand(DeleteStock, CanEditOrDeleteStock);
        }

        public async Task InitializeAsync()
        {
            await LoadStocksAsync();
        }

        private async Task LoadStocksAsync()
        {
            IsBusy = true;
            try
            {
                Stocks.Clear();
                var stocksFromDb = await _inventoryManager.GetAllItemsAsync();
                foreach (var stock in stocksFromDb)
                {
                    Stocks.Add(stock);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load stocks: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddNewStock()
        {
            _mainVm.Content = new StockAddEditView
            {
                DataContext = new StockAddEditViewModel(_mainVm, null)
            };
        }

        private void EditStock()
        {
            _mainVm.Content = new StockAddEditView
            {
                DataContext = new StockAddEditViewModel(_mainVm, SelectedStock)
            };
        }

        private async Task DeleteStock()
        {
            if (SelectedStock != null)
            {
                IsBusy = true;
                try
                {
                    await _inventoryManager.DeleteItemAsync(SelectedStock.Id);
                    Stocks.Remove(SelectedStock);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanEditOrDeleteStock() => SelectedStock != null;
    }
}
