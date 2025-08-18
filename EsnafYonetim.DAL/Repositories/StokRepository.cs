using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.DAL.Repositories
{
    public class StokRepository
    {
        private readonly DatabaseService _databaseService;

        public StokRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Stok>> GetAllAsync()
        {
            using var connection = _databaseService.GetConnection();
            return await connection.QueryAsync<Stok>("SELECT * FROM STOKLAR ORDER BY UrunAdi");
        }

        public async Task<Stok?> GetByIdAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            return await connection.QuerySingleOrDefaultAsync<Stok>("SELECT * FROM STOKLAR WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Stok stok)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                INSERT INTO STOKLAR (UrunKodu, UrunAdi, Miktar, Birim, AlisFiyati, SatisFiyati, EklenmeTarihi)
                VALUES (@UrunKodu, @UrunAdi, @Miktar, @Birim, @AlisFiyati, @SatisFiyati, @EklenmeTarihi);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, stok);
        }

        public async Task<bool> UpdateAsync(Stok stok)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                UPDATE STOKLAR SET
                    UrunKodu = @UrunKodu,
                    UrunAdi = @UrunAdi,
                    Miktar = @Miktar,
                    Birim = @Birim,
                    AlisFiyati = @AlisFiyati,
                    SatisFiyati = @SatisFiyati
                WHERE Id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, stok);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            var affectedRows = await connection.ExecuteAsync("DELETE FROM STOKLAR WHERE Id = @Id", new { Id = id });
            return affectedRows > 0;
        }
    }
}
