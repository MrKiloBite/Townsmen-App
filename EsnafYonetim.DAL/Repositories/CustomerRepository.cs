using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.DAL.Repositories
{
    public class CustomerRepository
    {
        private readonly DatabaseService _databaseService;

        public CustomerRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Musteri>> GetAllAsync()
        {
            using var connection = _databaseService.GetConnection();
            return await connection.QueryAsync<Musteri>("SELECT * FROM MUSTERI ORDER BY AdSoyad");
        }

        public async Task<Musteri?> GetByIdAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            return await connection.QuerySingleOrDefaultAsync<Musteri>("SELECT * FROM MUSTERI WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Musteri musteri)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                INSERT INTO MUSTERI (AdSoyad, Telefon, Adres, Eposta, Notlar, OlusturmaTarihi, MusteriFotograf, FisFotograf, Status)
                VALUES (@AdSoyad, @Telefon, @Adres, @Eposta, @Notlar, @OlusturmaTarihi, @MusteriFotograf, @FisFotograf, @Status);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, musteri);
        }

        public async Task<bool> UpdateAsync(Musteri musteri)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                UPDATE MUSTERI SET
                    AdSoyad = @AdSoyad,
                    Telefon = @Telefon,
                    Adres = @Adres,
                    Eposta = @Eposta,
                    Notlar = @Notlar,
                    MusteriFotograf = @MusteriFotograf,
                    FisFotograf = @FisFotograf,
                    Status = @Status
                WHERE Id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, musteri);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            var affectedRows = await connection.ExecuteAsync("DELETE FROM MUSTERI WHERE Id = @Id", new { Id = id });
            return affectedRows > 0;
        }
    }
}
