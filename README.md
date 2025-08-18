# Esnaf Yönetim Sistemi

Bu proje, C# ve Avalonia UI kullanılarak küçük Türk işletmeleri ("esnaf") için geliştirilmiş modern, yüksek performanslı bir masaüstü yönetim uygulamasıdır.

## Özellikler

- **Müşteri Yönetimi:** Müşteri profillerini oluşturun, düzenleyin ve yönetin.
- **Stok Yönetimi:** Envanterinizi takip edin, ürün ekleyin ve güncelleyin.
- **Muhasebe Yönetimi:** Gelir ve giderlerinizi kaydederek finansal durumunuzu takip edin.
- **Gelecek Özellikler:** Veritabanı yedekleme/geri yükleme ve basit raporlama gibi özellikler planlanmaktadır.

## Teknoloji ve Mimari

- **Platform:** .NET 7
- **Kullanıcı Arayüzü (UI):** Avalonia UI
- **Veritabanı:** SQLite
- **Veri Erişimi:** Dapper (Micro ORM) ve Microsoft.Data.Sqlite

Proje, bakımı ve genişletilebilirliği kolaylaştırmak için n-katmanlı bir mimari kullanır:
- **EsnafYonetim.UI:** Kullanıcı arayüzü (View ve ViewModel'ler).
- **EsnafYonetim.BLL:** İş mantığı katmanı (Manager sınıfları).
- **EsnafYonetim.DAL:** Veri erişim katmanı (Repository'ler ve veritabanı hizmeti).
- **EsnafYonetim.Core:** Temel veri modelleri (POCO sınıfları).

## Veritabanı Yapısı

Uygulama, ilk çalıştırmada `EsnafYonetim.db` adında bir SQLite veritabanı dosyası oluşturur. Temel tabloların yapısı aşağıdadır:

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
    Tutar REAL NOT NULL,
    IslemTarihi TEXT NOT NULL,
    VadeTarihi TEXT,
    OdemeDurumu TEXT NOT NULL,
    Aciklama TEXT,
    FOREIGN KEY (MusteriId) REFERENCES MUSTERI(Id)
);

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
);
```

## Nasıl Çalıştırılır

1. Depoyu klonlayın.
2. .NET 7 SDK'sının yüklü olduğundan emin olun.
3. Projenin kök dizininde bir terminal açın ve aşağıdaki komutu çalıştırın:

```bash
dotnet run --project EsnafYonetim.UI/EsnafYonetim.UI.csproj
```
