using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System;
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
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QueryAsync<Stok>("SELECT * FROM Stok ORDER BY UrunAdi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (StokRepository.GetAllAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<Stok?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QuerySingleOrDefaultAsync<Stok>("SELECT * FROM Stok WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (StokRepository.GetByIdAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateAsync(Stok stok)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    INSERT INTO Stok (UrunKodu, UrunAdi, Miktar, Birim, AlisFiyati, SatisFiyati, EklenmeTarihi)
                    VALUES (@UrunKodu, @UrunAdi, @Miktar, @Birim, @AlisFiyati, @SatisFiyati, @EklenmeTarihi);
                    SELECT last_insert_rowid();";
                return await connection.ExecuteScalarAsync<int>(sql, stok);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (StokRepository.CreateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Stok stok)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    UPDATE Stok SET
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
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (StokRepository.UpdateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Stok WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (StokRepository.DeleteAsync): {ex.Message}");
                throw;
            }
        }
    }
}
