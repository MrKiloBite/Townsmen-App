# Esnaf Yönetim Sistemi

Bu proje, C# ve Avalonia UI kullanılarak küçük Türk işletmeleri ("esnaf") için geliştirilmiş modern, yüksek performanslı bir masaüstü yönetim uygulamasıdır.

## Özellikler

- **Tam Kapsamlı Modül Yönetimi:** Müşteriler, Stoklar ve Muhasebe modülleri için tam CRUD (Oluştur, Oku, Güncelle, Sil) işlevselliği.
- **Gelişmiş Muhasebe:**
  - Harici bir `config.txt` dosyasından okunan KDV ve POS komisyon oranları.
  - Her işlem için ödeme tipi (Nakit, POS, Havale, Borç) takibi.
  - İşlem anındaki oranların kaydedilmesiyle geçmişe dönük veri tutarlılığı.
  - Net kâr, KDV tutarı gibi hesaplanmış değerlerin gösterimi.
- **Ayarlar Modülü:**
  - Açık ve Koyu tema arasında geçiş yapabilme.
  - Finansal oranları (`config.txt` üzerinden) uygulama içinden yönetme.
- **Bildirimler Modülü (Altyapı):** Zamanlanmış ve koşullu bildirimler için temel altyapı ve arayüz oluşturulmuştur.

## Teknoloji ve Mimari

- **Platform:** .NET 7
- **Kullanıcı Arayüzü (UI):** Avalonia UI
- **Veritabanı:** SQLite
- **Veri Erişimi:** Dapper (Micro ORM) ve Microsoft.Data.Sqlite

Proje, bakımı ve genişletilebilirliği kolaylaştırmak için n-katmanlı bir mimari kullanır:
- **EsnafYonetim.UI:** Kullanıcı arayüzü (View ve ViewModel'ler).
- **EsnafYonetim.BLL:** İş mantığı katmanı (Manager ve Service sınıfları).
- **EsnafYonetim.DAL:** Veri erişim katmanı (Repository'ler ve veritabanı hizmeti).
- **EsnafYonetim.Core:** Temel veri modelleri (POCO sınıfları).

## Veritabanı Yapısı

Uygulama, ilk çalıştırmada `Data/EsnafYonetim.db` adında bir SQLite veritabanı dosyası oluşturur. Temel tabloların yapısı aşağıdadır:

```sql
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
);

CREATE TABLE STOKLAR (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UrunKodu TEXT,
    UrunAdi TEXT,
    Miktar REAL NOT NULL,
    Birim TEXT,
    AlisFiyati REAL NOT NULL,
    SatisFiyati REAL NOT NULL,
    EklenmeTarihi TEXT NOT NULL
);

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
);

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
);
```

## Nasıl Çalıştırılır

1. Depoyu klonlayın.
2. .NET 7 SDK'sının yüklü olduğundan emin olun.
3. Projenin kök dizininde bir terminal açın ve aşağıdaki komutu çalıştırın:

```bash
dotnet run --project EsnafYonetim.UI/EsnafYonetim.UI.csproj
```
