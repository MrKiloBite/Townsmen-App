using System;

namespace EsnafYonetim.Core.Models
{
    public enum BildirimTipi { Zamanlanmis, StokSeviyesi }
    public enum KarsilastirmaOperatoru { Esittir, Kucuktur, Buyuktur }

    public class Bildirim
    {
        public int Id { get; set; }
        public bool Aktif { get; set; } = true;
        public string Mesaj { get; set; } = string.Empty;
        public BildirimTipi Tip { get; set; }

        // Zamanlanmış bildirimler için
        public DateTimeOffset? TetiklenmeZamani { get; set; }

        // Koşullu bildirimler için (örn: Stok Seviyesi)
        public int? HedefStokID { get; set; } // örn: Stok ID'si
        public KarsilastirmaOperatoru Operator { get; set; }
        public double Deger { get; set; } // örn: Stok Miktarı

        public string? SesDosyasiYolu { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}
