using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Repositories;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.BLL.Managers
{
    public class CustomerManager
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerManager()
        {
            // Şimdilik basitlik adına bağımlılıkları doğrudan oluşturuyoruz.
            // Daha büyük bir uygulamada uygun bir DI (Dependency Injection) container'ı kullanılır.
            var databaseService = new DatabaseService();

            // Veritabanının ve tabloların oluşturulduğundan emin ol.
            databaseService.InitializeDatabase();

            _customerRepository = new CustomerRepository(databaseService);
        }

        public async Task<IEnumerable<Musteri>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Musteri?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<int> AddCustomerAsync(Musteri customer)
        {
            // Burada iş mantığı olabilir, örn. doğrulama (validation).
            return await _customerRepository.CreateAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(Musteri customer)
        {
            // Burada iş mantığı olabilir.
            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            return await _customerRepository.DeleteAsync(id);
        }
    }
}
