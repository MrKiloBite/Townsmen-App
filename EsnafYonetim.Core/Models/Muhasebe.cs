using System;

namespace EsnafYonetim.Core.Models
{
    public class Muhasebe
    {
        public int Id { get; set; }
        public int? MusteriId { get; set; } // Nullable if not directly tied to a customer
        public string IslemTipi { get; set; } = "Gelir"; // "Gelir", "Gider"
        public string Kategori { get; set; } = "Satış"; // "Satış", "Fatura", "Maaş" etc.
        public decimal Tutar { get; set; }
        public DateTime IslemTarihi { get; set; }
        public DateTime? VadeTarihi { get; set; }
        public string OdemeDurumu { get; set; } = "Ödenmedi"; // "Ödendi", "Ödenmedi", "Kısmi Ödendi"
        public string? Aciklama { get; set; }
    }
}
