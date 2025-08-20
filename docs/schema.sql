-- =================================================================================
-- Esnaf Yönetim Sistemi - Veritabanı Şeması
-- Bu şema, uygulamanın tüm fonksiyonel gereksinimlerini karşılamak üzere tasarlanmıştır.
-- =================================================================================


-- =================================================================================
-- Çekirdek ve Ayarlar (Core & Settings)
-- =================================================================================

-- Uygulama genelindeki ayarları (tema tercihi gibi) saklar.
CREATE TABLE IF NOT EXISTS Ayarlar (
    AyarID      INTEGER PRIMARY KEY,
    Anahtar     TEXT NOT NULL UNIQUE, -- Örn: 'Theme', 'DefaultKDV_ID', 'Username'
    Deger       TEXT NOT NULL
);


-- =================================================================================
-- Müşteri İlişkileri Yönetimi (CRM) Modülü
-- =================================================================================

-- Müşterileri kategorize etmek ve aramak için kullanılır.
CREATE TABLE IF NOT EXISTS MusteriTurleri (
    MusteriTuruID   INTEGER PRIMARY KEY,
    TurAdi          TEXT NOT NULL UNIQUE
);

-- Acentaları kategorize etmek ve aramak için kullanılır.
CREATE TABLE IF NOT EXISTS Acentalar (
    AcentaID    INTEGER PRIMARY KEY,
    AcentaAdi   TEXT NOT NULL UNIQUE
);

-- Müşteri fotoğrafları, durumu ve diğer tüm istenen alanları içeren ana müşteri tablosu.
CREATE TABLE IF NOT EXISTS Musteriler (
    Id                  INTEGER PRIMARY KEY,
    AdSoyad             TEXT NOT NULL,
    Telefon             TEXT,
    Adres               TEXT,
    Eposta              TEXT,
    Notlar              TEXT,
    OlusturmaTarihi     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MusteriFotograf     BLOB, -- Müşteri fotoğrafını saklamak için.
    FisFotograf         BLOB, -- Varsayılan bir fiş/belge fotoğrafını saklamak için.
    Status              TEXT NOT NULL DEFAULT 'active', -- 'active', 'archived', 'deleted'
    MusteriTuruID       INTEGER,
    AcentaID            INTEGER,
    FOREIGN KEY (MusteriTuruID) REFERENCES MusteriTurleri(MusteriTuruID),
    FOREIGN KEY (AcentaID) REFERENCES Acentalar(AcentaID)
);


-- =================================================================================
-- Envanter / Stok Modülü
-- =================================================================================

-- Stok kalemleri/ürünleri için ana tablo.
CREATE TABLE IF NOT EXISTS Stok (
    Id              INTEGER PRIMARY KEY,
    UrunKodu        TEXT UNIQUE,
    UrunAdi         TEXT NOT NULL,
    Miktar          REAL NOT NULL DEFAULT 0,
    Birim           TEXT, -- Örn: 'adet', 'kg', 'litre'
    AlisFiyati      REAL,
    SatisFiyati     REAL NOT NULL,
    EklenmeTarihi   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);


-- =================================================================================
-- Bildirimler Modülü
-- =================================================================================

-- Zamanlanmış ve koşullu uyarıları destekleyen, kullanıcı tanımlı bildirimleri saklar.
CREATE TABLE IF NOT EXISTS Bildirimler (
    Id                  INTEGER PRIMARY KEY,
    Aktif               INTEGER NOT NULL DEFAULT 1, -- 1: true, 0: false
    Mesaj               TEXT NOT NULL,
    Tip                 TEXT NOT NULL, -- 'Zamanlanmis' veya 'StokSeviyesi'
    TetiklenmeZamani    DATETIME, -- Zamanlanmış bildirimler için.
    HedefStokID         INTEGER, -- Stok seviyesi uyarıları için, Stok(Id) tablosuna referans.
    Operator            TEXT, -- Örn: '<', '>', '='
    Deger               REAL, -- Örn: Karşılaştırılacak stok miktarı.
    SesDosyasiYolu      TEXT,
    OlusturmaTarihi     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (HedefStokID) REFERENCES Stok(Id)
);


-- =================================================================================
-- Muhasebe Modülü
-- =================================================================================

-- Muhasebe sisteminin bel kemiği olan Hesap Planı.
CREATE TABLE IF NOT EXISTS HesapPlani (
    HesapID     INTEGER PRIMARY KEY,
    HesapKodu   TEXT NOT NULL UNIQUE,
    HesapAdi    TEXT NOT NULL,
    HesapTipi   TEXT NOT NULL -- Örn: 'Varlık', 'Borç', 'Gelir', 'Gider'
);

-- Farklı kategoriler için dinamik KDV oranlarını saklar.
CREATE TABLE IF NOT EXISTS KDV_Oranlari (
    KDV_OranID      INTEGER PRIMARY KEY,
    KategoriAdi     TEXT NOT NULL UNIQUE,
    Oran            REAL NOT NULL -- Örn: 20.0 (20% için), 8.0 (8% için)
);

-- Farklı bankalar için dinamik POS komisyon oranlarını saklar.
CREATE TABLE IF NOT EXISTS POS_Komisyonlari (
    POS_KomisyonID              INTEGER PRIMARY KEY,
    BankaAdi                    TEXT NOT NULL UNIQUE,
    KomisyonOraniTekCekim       REAL NOT NULL,
    KomisyonOraniTaksitli       REAL,
    ValorSuresi                 INTEGER -- gün olarak
);

-- Tüm finansal işlemleri (yevmiye kayıtları) için ana tablo.
CREATE TABLE IF NOT EXISTS Muhasebe (
    Id                      INTEGER PRIMARY KEY,
    IslemTarihi             DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Aciklama                TEXT,
    IslemTipi               TEXT NOT NULL, -- 'Gelir', 'Gider'
    OdemeDurumu             TEXT NOT NULL, -- 'Ödendi', 'Ödenmedi'
    BrutTutar               REAL NOT NULL,
    NetTutar                REAL NOT NULL,
    GenelToplam             REAL NOT NULL,
    FaturaFotograf          BLOB, -- Fatura/fiş resmini saklamak için.
    -- Dinamik KDV Alanları
    KDV_Oran_ID             INTEGER,
    UygulananKDVOrani       REAL,
    KDV_Tutari              REAL,
    -- Dinamik POS Komisyon Alanları
    POS_Komisyon_ID         INTEGER,
    UygulananKomisyonOrani  REAL,
    KomisyonTutari          REAL,
    FOREIGN KEY (KDV_Oran_ID) REFERENCES KDV_Oranlari(KDV_OranID),
    FOREIGN KEY (POS_Komisyon_ID) REFERENCES POS_Komisyonlari(POS_KomisyonID)
);
