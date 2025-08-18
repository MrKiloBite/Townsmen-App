using System;

namespace EsnafYonetim.Core.Models
{
    public class Stok
    {
        public int Id { get; set; }
        public string? UrunKodu { get; set; }
        public string? UrunAdi { get; set; }
        public double Miktar { get; set; }
        public string? Birim { get; set; } // "Adet", "Kg", "Litre" etc.
        public decimal AlisFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public DateTime EklenmeTarihi { get; set; }
    }
}
