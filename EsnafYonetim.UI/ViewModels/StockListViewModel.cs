using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class StockListViewModel : ViewModelBase
    {
        private readonly InventoryManager _inventoryManager;

        public ObservableCollection<Stok> Stocks { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditStockCommand))]
        private Stok? _selectedStock;

        public event Action<Stok?>? AddOrEditStockRequested;

        public StockListViewModel()
        {
            _inventoryManager = new InventoryManager();
            _ = LoadStocksAsync();
        }

        [RelayCommand]
        public async Task LoadStocksAsync()
        {
            Stocks.Clear();
            var stocksFromDb = await _inventoryManager.GetAllItemsAsync();
            foreach (var stock in stocksFromDb)
            {
                Stocks.Add(stock);
            }
        }

        [RelayCommand]
        private void AddNewStock()
        {
            AddOrEditStockRequested?.Invoke(null);
        }

        [RelayCommand(CanExecute = nameof(CanEditStock))]
        private void EditStock()
        {
            AddOrEditStockRequested?.Invoke(SelectedStock);
        }

        private bool CanEditStock() => SelectedStock != null;
    }
}
