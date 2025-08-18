using System;

namespace EsnafYonetim.Core.Models
{
    /// <summary>
    /// Müşteriler, ürünler ve muhasebe kayıtları arasındaki tüm işlemleri birbirine bağlayan detaylı bir kayıt defteri.
    /// Örneğin, bir satış işlemi, bir müşteri, bir veya daha fazla stok ürünü ve bir muhasebe kaydı oluşturur.
    /// </summary>
    public class Islem
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public int MuhasebeId { get; set; }
        public string IslemTipi { get; set; } = "Satış"; // "Satış", "Alış", "İade"
        public DateTime IslemTarihi { get; set; }
        public decimal ToplamTutar { get; set; }
        public string? Aciklama { get; set; }

        // Bu işlemle ilişkili stok hareketlerini tutmak için bir liste.
        // Bu, bir satışta birden fazla ürünün satılabilmesini sağlar.
        // Bu alan veritabanında doğrudan bir sütuna karşılık gelmez, ancak iş mantığında kullanılır.
        // Veritabanı tarafında IslemDetay gibi ayrı bir tablo ile temsil edilir.
        // public System.Collections.Generic.List<Stok> IliskiliStoklar { get; set; } = new();
    }
}
