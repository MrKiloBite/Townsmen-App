using System;

namespace EsnafYonetim.Core.Models
{
    public class Muhasebe
    {
        public int Id { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string? Aciklama { get; set; }
        public string IslemTipi { get; set; } = string.Empty; // 'Gelir', 'Gider'
        public string OdemeDurumu { get; set; } = string.Empty; // 'Ödendi', 'Ödenmedi'

        public decimal BrutTutar { get; set; }
        public decimal NetTutar { get; set; }
        public decimal GenelToplam { get; set; }

        public byte[]? FaturaFotograf { get; set; }

        // Foreign Key for dynamic KDV rate
        public int? KDV_Oran_ID { get; set; }
        // Snapshot of the rate at the time of transaction
        public decimal? UygulananKDVOrani { get; set; }
        public decimal? KDV_Tutari { get; set; }

        // Foreign Key for dynamic POS commission
        public int? POS_Komisyon_ID { get; set; }
        // Snapshot of the rate at the time of transaction
        public decimal? UygulananKomisyonOrani { get; set; }
        public decimal? KomisyonTutari { get; set; }
    }
}
