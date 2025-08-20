using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EsnafYonetim.UI.Views;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class StockAddEditViewModel : ViewModelBase
    {
        private readonly InventoryManager _inventoryManager;
        private readonly MainWindowViewModel _mainVm;

        [ObservableProperty]
        private Stok _stock;

        private readonly bool _isNewStock;

        public string Title => _isNewStock ? "Yeni Stok Ekle" : "Stok Bilgilerini Düzenle";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public StockAddEditViewModel(MainWindowViewModel mainVm, Stok? stockToEdit)
        {
            _inventoryManager = new InventoryManager();
            _mainVm = mainVm;

            if (stockToEdit == null)
            {
                _stock = new Stok { EklenmeTarihi = DateTime.Now };
                _isNewStock = true;
            }
            else
            {
                _stock = stockToEdit;
                _isNewStock = false;
            }

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async Task SaveAsync()
        {
            if (_isNewStock)
            {
                await _inventoryManager.AddItemAsync(Stock);
            }
            else
            {
                await _inventoryManager.UpdateItemAsync(Stock);
            }

            _mainVm.Content = new StockListView { DataContext = new StockListViewModel(_mainVm) };
        }

        private void Cancel()
        {
            _mainVm.Content = new StockListView { DataContext = new StockListViewModel(_mainVm) };
        }
    }
}
