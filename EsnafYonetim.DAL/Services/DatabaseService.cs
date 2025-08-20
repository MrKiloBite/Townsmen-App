using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace EsnafYonetim.DAL.Services
{
    public class DatabaseService
    {
        private readonly string _databasePath;

        public DatabaseService()
        {
            // Veritabanı, uygulamanın çalıştığı dizinde oluşturulacak.
            var dbFolder = Path.Combine(AppContext.BaseDirectory, "Data");
            Directory.CreateDirectory(dbFolder); // Data klasörünü oluştur
            _databasePath = Path.Combine(dbFolder, "EsnafYonetim.db");
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={_databasePath}");
        }

        public void InitializeDatabase()
        {
            if (File.Exists(_databasePath))
            {
                // Veritabanı zaten var, bir şey yapma.
                // İleride buraya migrasyon mantığı eklenebilir.
                return;
            }

            using var connection = GetConnection();
            connection.Open();

            // docs/schema.sql dosyasından alınan yeni şema
            var schema = @"
                CREATE TABLE IF NOT EXISTS Ayarlar (
                    AyarID      INTEGER PRIMARY KEY,
                    Anahtar     TEXT NOT NULL UNIQUE,
                    Deger       TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS MusteriTurleri (
                    MusteriTuruID   INTEGER PRIMARY KEY,
                    TurAdi          TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Acentalar (
                    AcentaID    INTEGER PRIMARY KEY,
                    AcentaAdi   TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Musteriler (
                    Id                  INTEGER PRIMARY KEY,
                    AdSoyad             TEXT NOT NULL,
                    Telefon             TEXT,
                    Adres               TEXT,
                    Eposta              TEXT,
                    Notlar              TEXT,
                    OlusturmaTarihi     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    MusteriFotograf     BLOB,
                    FisFotograf         BLOB,
                    Status              TEXT NOT NULL DEFAULT 'active',
                    MusteriTuruID       INTEGER,
                    AcentaID            INTEGER,
                    FOREIGN KEY (MusteriTuruID) REFERENCES MusteriTurleri(MusteriTuruID),
                    FOREIGN KEY (AcentaID) REFERENCES Acentalar(AcentaID)
                );

                CREATE TABLE IF NOT EXISTS Stok (
                    Id              INTEGER PRIMARY KEY,
                    UrunKodu        TEXT UNIQUE,
                    UrunAdi         TEXT NOT NULL,
                    Miktar          REAL NOT NULL DEFAULT 0,
                    Birim           TEXT,
                    AlisFiyati      REAL,
                    SatisFiyati     REAL NOT NULL,
                    EklenmeTarihi   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Bildirimler (
                    Id                  INTEGER PRIMARY KEY,
                    Aktif               INTEGER NOT NULL DEFAULT 1,
                    Mesaj               TEXT NOT NULL,
                    Tip                 TEXT NOT NULL,
                    TetiklenmeZamani    DATETIME,
                    HedefStokID         INTEGER,
                    Operator            TEXT,
                    Deger               REAL,
                    SesDosyasiYolu      TEXT,
                    OlusturmaTarihi     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (HedefStokID) REFERENCES Stok(Id)
                );

                CREATE TABLE IF NOT EXISTS HesapPlani (
                    HesapID     INTEGER PRIMARY KEY,
                    HesapKodu   TEXT NOT NULL UNIQUE,
                    HesapAdi    TEXT NOT NULL,
                    HesapTipi   TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS KDV_Oranlari (
                    KDV_OranID      INTEGER PRIMARY KEY,
                    KategoriAdi     TEXT NOT NULL UNIQUE,
                    Oran            REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS POS_Komisyonlari (
                    POS_KomisyonID              INTEGER PRIMARY KEY,
                    BankaAdi                    TEXT NOT NULL UNIQUE,
                    KomisyonOraniTekCekim       REAL NOT NULL,
                    KomisyonOraniTaksitli       REAL,
                    ValorSuresi                 INTEGER
                );

                CREATE TABLE IF NOT EXISTS Muhasebe (
                    Id                      INTEGER PRIMARY KEY,
                    IslemTarihi             DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    Aciklama                TEXT,
                    IslemTipi               TEXT NOT NULL,
                    OdemeDurumu             TEXT NOT NULL,
                    BrutTutar               REAL NOT NULL,
                    NetTutar                REAL NOT NULL,
                    GenelToplam             REAL NOT NULL,
                    FaturaFotograf          BLOB,
                    KDV_Oran_ID             INTEGER,
                    UygulananKDVOrani       REAL,
                    KDV_Tutari              REAL,
                    POS_Komisyon_ID         INTEGER,
                    UygulananKomisyonOrani  REAL,
                    KomisyonTutari          REAL,
                    FOREIGN KEY (KDV_Oran_ID) REFERENCES KDV_Oranlari(KDV_OranID),
                    FOREIGN KEY (POS_Komisyon_ID) REFERENCES POS_Komisyonlari(POS_KomisyonID)
                );
            ";

            connection.Execute(schema);
        }
    }
}
