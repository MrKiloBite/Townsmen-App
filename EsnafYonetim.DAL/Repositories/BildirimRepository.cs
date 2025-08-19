using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.DAL.Repositories
{
    public class BildirimRepository
    {
        private readonly DatabaseService _databaseService;

        public BildirimRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Bildirim>> GetAllAsync()
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QueryAsync<Bildirim>("SELECT * FROM BILDIRIMLER");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (BildirimRepository.GetAllAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateAsync(Bildirim bildirim)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    INSERT INTO BILDIRIMLER (Aktif, Mesaj, Tip, TetiklenmeZamani, HedefId, Operator, Deger, SesDosyasiYolu, OlusturmaTarihi)
                    VALUES (@Aktif, @Mesaj, @Tip, @TetiklenmeZamani, @HedefId, @Operator, @Deger, @SesDosyasiYolu, @OlusturmaTarihi);
                    SELECT last_insert_rowid();";
                return await connection.ExecuteScalarAsync<int>(sql, bildirim);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (BildirimRepository.CreateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Bildirim bildirim)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    UPDATE BILDIRIMLER SET
                        Aktif = @Aktif,
                        Mesaj = @Mesaj,
                        Tip = @Tip,
                        TetiklenmeZamani = @TetiklenmeZamani,
                        HedefId = @HedefId,
                        Operator = @Operator,
                        Deger = @Deger,
                        SesDosyasiYolu = @SesDosyasiYolu
                    WHERE Id = @Id;";
                var affectedRows = await connection.ExecuteAsync(sql, bildirim);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (BildirimRepository.UpdateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var affectedRows = await connection.ExecuteAsync("DELETE FROM BILDIRIMLER WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (BildirimRepository.DeleteAsync): {ex.Message}");
                throw;
            }
        }
    }
}
