using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System;
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
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QueryAsync<Musteri>("SELECT * FROM MUSTERI ORDER BY AdSoyad");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (CustomerRepository.GetAllAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<Musteri?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QuerySingleOrDefaultAsync<Musteri>("SELECT * FROM MUSTERI WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (CustomerRepository.GetByIdAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateAsync(Musteri musteri)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    INSERT INTO MUSTERI (AdSoyad, Telefon, Adres, Eposta, Notlar, OlusturmaTarihi, MusteriFotograf, FisFotograf, Status)
                    VALUES (@AdSoyad, @Telefon, @Adres, @Eposta, @Notlar, @OlusturmaTarihi, @MusteriFotograf, @FisFotograf, @Status);
                    SELECT last_insert_rowid();";
                return await connection.ExecuteScalarAsync<int>(sql, musteri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (CustomerRepository.CreateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Musteri musteri)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (CustomerRepository.UpdateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var affectedRows = await connection.ExecuteAsync("DELETE FROM MUSTERI WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (CustomerRepository.DeleteAsync): {ex.Message}");
                throw;
            }
        }
    }
}
