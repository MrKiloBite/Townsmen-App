using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
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
            using var connection = _databaseService.GetConnection();
            return await connection.QueryAsync<Muhasebe>("SELECT * FROM MUHASEBE ORDER BY IslemTarihi DESC");
        }

        public async Task<Muhasebe?> GetByIdAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            return await connection.QuerySingleOrDefaultAsync<Muhasebe>("SELECT * FROM MUHASEBE WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Muhasebe transaction)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                INSERT INTO MUHASEBE (MusteriId, IslemTipi, Kategori, BrutTutar, OdemeTipi, UygulananKDVOrani, UygulananKomisyonOrani, IslemTarihi, VadeTarihi, OdemeDurumu, Aciklama)
                VALUES (@MusteriId, @IslemTipi, @Kategori, @BrutTutar, @OdemeTipi, @UygulananKDVOrani, @UygulananKomisyonOrani, @IslemTarihi, @VadeTarihi, @OdemeDurumu, @Aciklama);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, transaction);
        }

        public async Task<bool> UpdateAsync(Muhasebe transaction)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                UPDATE MUHASEBE SET
                    MusteriId = @MusteriId,
                    IslemTipi = @IslemTipi,
                    Kategori = @Kategori,
                    BrutTutar = @BrutTutar,
                    OdemeTipi = @OdemeTipi,
                    UygulananKDVOrani = @UygulananKDVOrani,
                    UygulananKomisyonOrani = @UygulananKomisyonOrani,
                    IslemTarihi = @IslemTarihi,
                    VadeTarihi = @VadeTarihi,
                    OdemeDurumu = @OdemeDurumu,
                    Aciklama = @Aciklama
                WHERE Id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, transaction);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            var affectedRows = await connection.ExecuteAsync("DELETE FROM MUHASEBE WHERE Id = @Id", new { Id = id });
            return affectedRows > 0;
        }
    }
}
