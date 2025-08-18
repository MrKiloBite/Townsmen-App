using Dapper;
using EsnafYonetim.Core.Models;
using EsnafYonetim.DAL.Services;
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
            using var connection = _databaseService.GetConnection();
            return await connection.QueryAsync<Bildirim>("SELECT * FROM BILDIRIMLER");
        }

        public async Task<int> CreateAsync(Bildirim bildirim)
        {
            using var connection = _databaseService.GetConnection();
            var sql = @"
                INSERT INTO BILDIRIMLER (Aktif, Mesaj, Tip, TetiklenmeZamani, HedefId, Operator, Deger, SesDosyasiYolu, OlusturmaTarihi)
                VALUES (@Aktif, @Mesaj, @Tip, @TetiklenmeZamani, @HedefId, @Operator, @Deger, @SesDosyasiYolu, @OlusturmaTarihi);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, bildirim);
        }

        public async Task<bool> UpdateAsync(Bildirim bildirim)
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

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _databaseService.GetConnection();
            var affectedRows = await connection.ExecuteAsync("DELETE FROM BILDIRIMLER WHERE Id = @Id", new { Id = id });
            return affectedRows > 0;
        }
    }
}
