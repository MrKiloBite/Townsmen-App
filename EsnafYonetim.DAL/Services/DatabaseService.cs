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
            // Geliştirme aşamasında şema değişikliklerini kolaylaştırmak için
            // veritabanını her başlangıçta silip yeniden oluşturuyoruz.
            // TODO: Üretime geçmeden önce bu satırı kaldır ve bir migration sistemi kullan.
            if (File.Exists(_databasePath))
            {
                File.Delete(_databasePath);
            }

            if (File.Exists(_databasePath))
            {
                return;
            }

            using var connection = GetConnection();
            connection.Open();

            // MUSTERI Tablosu
            connection.Execute(@"
            CREATE TABLE MUSTERI (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AdSoyad TEXT,
                Telefon TEXT,
                Adres TEXT,
                Eposta TEXT,
                Notlar TEXT,
                OlusturmaTarihi TEXT NOT NULL,
                MusteriFotograf BLOB,
                FisFotograf BLOB,
                Status TEXT NOT NULL
            );");

            // STOKLAR Tablosu
            connection.Execute(@"
            CREATE TABLE STOKLAR (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UrunKodu TEXT,
                UrunAdi TEXT,
                Miktar REAL NOT NULL,
                Birim TEXT,
                AlisFiyati REAL NOT NULL,
                SatisFiyati REAL NOT NULL,
                EklenmeTarihi TEXT NOT NULL
            );");

            // MUHASEBE Tablosu
            connection.Execute(@"
            CREATE TABLE MUHASEBE (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                MusteriId INTEGER,
                IslemTipi TEXT NOT NULL,
                Kategori TEXT,
                BrutTutar REAL NOT NULL,
                OdemeTipi TEXT NOT NULL,
                UygulananKDVOrani REAL NOT NULL,
                UygulananKomisyonOrani REAL NOT NULL,
                IslemTarihi TEXT NOT NULL,
                VadeTarihi TEXT,
                OdemeDurumu TEXT NOT NULL,
                Aciklama TEXT,
                FOREIGN KEY (MusteriId) REFERENCES MUSTERI(Id)
            );");

            // ISLEMLER Tablosu
            connection.Execute(@"
            CREATE TABLE ISLEMLER (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                MusteriId INTEGER NOT NULL,
                MuhasebeId INTEGER NOT NULL,
                IslemTipi TEXT NOT NULL,
                IslemTarihi TEXT NOT NULL,
                ToplamTutar REAL NOT NULL,
                Aciklama TEXT,
                FOREIGN KEY (MusteriId) REFERENCES MUSTERI(Id),
                FOREIGN KEY (MuhasebeId) REFERENCES MUHASEBE(Id)
            );");

            // BILDIRIMLER Tablosu
            connection.Execute(@"
            CREATE TABLE BILDIRIMLER (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Aktif INTEGER NOT NULL,
                Mesaj TEXT NOT NULL,
                Tip INTEGER NOT NULL,
                TetiklenmeZamani TEXT,
                HedefId INTEGER,
                Operator INTEGER NOT NULL,
                Deger REAL NOT NULL,
                SesDosyasiYolu TEXT,
                OlusturmaTarihi TEXT NOT NULL
            );");
        }
    }
}
