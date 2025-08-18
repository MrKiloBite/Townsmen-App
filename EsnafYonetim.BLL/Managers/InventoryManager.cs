using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Repositories;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.BLL.Managers
{
    public class InventoryManager
    {
        private readonly StokRepository _stokRepository;

        public InventoryManager()
        {
            var databaseService = new DatabaseService();
            // Veritabanının zaten başka bir yönetici tarafından başlatıldığını varsayıyoruz.
            _stokRepository = new StokRepository(databaseService);
        }

        public async Task<IEnumerable<Stok>> GetAllItemsAsync()
        {
            return await _stokRepository.GetAllAsync();
        }

        public async Task<int> AddItemAsync(Stok item)
        {
            // İş mantığı burada eklenebilir.
            return await _stokRepository.CreateAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Stok item)
        {
            return await _stokRepository.UpdateAsync(item);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            return await _stokRepository.DeleteAsync(id);
        }
    }
}
