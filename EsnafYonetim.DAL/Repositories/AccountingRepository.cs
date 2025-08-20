using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnafYonetim.DAL.Repositories
{
    public class AccountingRepository
    {
        private readonly DatabaseService _databaseService;

        public AccountingRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Muhasebe>> GetAllAsync()
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QueryAsync<Muhasebe>("SELECT * FROM Muhasebe ORDER BY IslemTarihi DESC");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (AccountingRepository.GetAllAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<Muhasebe?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                return await connection.QuerySingleOrDefaultAsync<Muhasebe>("SELECT * FROM Muhasebe WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (AccountingRepository.GetByIdAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateAsync(Muhasebe transaction)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    INSERT INTO Muhasebe (IslemTarihi, Aciklama, IslemTipi, OdemeDurumu, BrutTutar, NetTutar, GenelToplam, FaturaFotograf, KDV_Oran_ID, UygulananKDVOrani, KDV_Tutari, POS_Komisyon_ID, UygulananKomisyonOrani, KomisyonTutari)
                    VALUES (@IslemTarihi, @Aciklama, @IslemTipi, @OdemeDurumu, @BrutTutar, @NetTutar, @GenelToplam, @FaturaFotograf, @KDV_Oran_ID, @UygulananKDVOrani, @KDV_Tutari, @POS_Komisyon_ID, @UygulananKomisyonOrani, @KomisyonTutari);
                    SELECT last_insert_rowid();";
                return await connection.ExecuteScalarAsync<int>(sql, transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (AccountingRepository.CreateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Muhasebe transaction)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var sql = @"
                    UPDATE Muhasebe SET
                        IslemTarihi = @IslemTarihi,
                        Aciklama = @Aciklama,
                        IslemTipi = @IslemTipi,
                        OdemeDurumu = @OdemeDurumu,
                        BrutTutar = @BrutTutar,
                        NetTutar = @NetTutar,
                        GenelToplam = @GenelToplam,
                        FaturaFotograf = @FaturaFotograf,
                        KDV_Oran_ID = @KDV_Oran_ID,
                        UygulananKDVOrani = @UygulananKDVOrani,
                        KDV_Tutari = @KDV_Tutari,
                        POS_Komisyon_ID = @POS_Komisyon_ID,
                        UygulananKomisyonOrani = @UygulananKomisyonOrani,
                        KomisyonTutari = @KomisyonTutari
                    WHERE Id = @Id;";
                var affectedRows = await connection.ExecuteAsync(sql, transaction);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (AccountingRepository.UpdateAsync): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _databaseService.GetConnection();
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Muhasebe WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA (AccountingRepository.DeleteAsync): {ex.Message}");
                throw;
            }
        }
    }
}
