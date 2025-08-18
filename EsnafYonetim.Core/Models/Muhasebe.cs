using System;

namespace EsnafYonetim.Core.Models
{
    public class Muhasebe
    {
        public int Id { get; set; }
        public int? MusteriId { get; set; }
        public string IslemTipi { get; set; } = "Gelir"; // "Gelir", "Gider"
        public string Kategori { get; set; } = "Satış"; // "Satış", "Fatura", "Maaş" etc.

        // Ana tutar, vergiler ve komisyonlar hariç.
        public decimal BrutTutar { get; set; }

        // --- Gelişmiş Özellikler ---
        public string OdemeTipi { get; set; } = "Nakit"; // "Nakit", "POS", "Havale", "Borç"

        // O anki işlem için uygulanan oranlar veritabanında saklanır.
        public decimal UygulananKDVOrani { get; set; }
        public decimal UygulananKomisyonOrani { get; set; }

        // Bu alanlar UI'da gösterim için kolaylık sağlar ve veritabanında saklanmaz.
        public decimal KDV_Tutari => BrutTutar * UygulananKDVOrani;
        public decimal KomisyonTutari => OdemeTipi == "POS" ? BrutTutar * UygulananKomisyonOrani : 0;
        public decimal NetTutar => BrutTutar - KomisyonTutari;
        public decimal GenelToplam => BrutTutar + KDV_Tutari;
        // -------------------------

        public DateTime IslemTarihi { get; set; }
        public DateTime? VadeTarihi { get; set; }
        public string OdemeDurumu { get; set; } = "Ödenmedi"; // "Ödendi", "Ödenmedi", "Kısmi Ödendi"
        public string? Aciklama { get; set; }
    }
}
