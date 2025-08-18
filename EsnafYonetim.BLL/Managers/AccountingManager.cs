using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Repositories;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsnafYonetim.BLL.Managers
{
    public class AccountingManager
    {
        private readonly AccountingRepository _accountingRepository;

        public AccountingManager()
        {
            var databaseService = new DatabaseService();
            _accountingRepository = new AccountingRepository(databaseService);
        }

        public async Task<IEnumerable<Muhasebe>> GetAllTransactionsAsync()
        {
            return await _accountingRepository.GetAllAsync();
        }

        public async Task<int> AddTransactionAsync(Muhasebe transaction)
        {
            return await _accountingRepository.CreateAsync(transaction);
        }

        public async Task<bool> UpdateTransactionAsync(Muhasebe transaction)
        {
            return await _accountingRepository.UpdateAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            return await _accountingRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            var transactions = await _accountingRepository.GetAllAsync();
            return transactions.Where(t => t.IslemTipi == "Gelir").Sum(t => t.BrutTutar);
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            var transactions = await _accountingRepository.GetAllAsync();
            return transactions.Where(t => t.IslemTipi == "Gider").Sum(t => t.BrutTutar);
        }
    }
}
