using System;

namespace EsnafYonetim.Core.Models
{
    public class Musteri
    {
        public int Id { get; set; }
        public string? AdSoyad { get; set; }
        public string? Telefon { get; set; }
        public string? Adres { get; set; }
        public string? Eposta { get; set; }
        public string? Notlar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        // Pillar 1'de belirtilen alanlar
        public byte[]? MusteriFotograf { get; set; }
        public byte[]? FisFotograf { get; set; }
        public string Status { get; set; } = "active"; // Varsayılan değer "active"
    }
}
