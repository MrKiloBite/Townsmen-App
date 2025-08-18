using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Repositories;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.BLL.Managers
{
    public class BildirimManager
    {
        private readonly BildirimRepository _bildirimRepository;

        public BildirimManager()
        {
            var databaseService = new DatabaseService();
            _bildirimRepository = new BildirimRepository(databaseService);
        }

        public async Task<IEnumerable<Bildirim>> GetAllAsync()
        {
            return await _bildirimRepository.GetAllAsync();
        }

        public async Task<int> AddAsync(Bildirim bildirim)
        {
            return await _bildirimRepository.CreateAsync(bildirim);
        }

        public async Task<bool> UpdateAsync(Bildirim bildirim)
        {
            return await _bildirimRepository.UpdateAsync(bildirim);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bildirimRepository.DeleteAsync(id);
        }
    }
}
