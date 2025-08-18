using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using System;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class StockAddEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Stok _stock;

        private readonly InventoryManager _inventoryManager;
        private readonly bool _isNewStock;

        public event Action? OnRequestClose;

        public StockAddEditViewModel(Stok? stockToEdit)
        {
            _inventoryManager = new InventoryManager();
            if (stockToEdit == null)
            {
                // Yeni stok ürünü
                _stock = new Stok { EklenmeTarihi = DateTime.Now };
                _isNewStock = true;
            }
            else
            {
                // Mevcut stok ürünü düzenleniyor
                _stock = stockToEdit;
                _isNewStock = false;
            }
        }

        [RelayCommand]
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

            OnRequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            OnRequestClose?.Invoke();
        }
    }
}
