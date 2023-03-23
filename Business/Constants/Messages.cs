using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages  // static kullandığımız zaman newlemeye gerek yok. "Messages." yazıp kullanmaya devam edebiliriz.
    {
        public static string ProductAdded = "Ürün eklendi.";
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";
        public static string MaintenanceTime = "Şuan listeleme zamanı değil.";
        public static string Listed = "Listelendi.";
        public static string DetailsListed = "Ürünlerin Detayları Listelendi.";
        public static string ProductCount = "Kategorinin ürün sayısı 10'dan fazla olamaz..";
        public static string SameProductName = "Aynı isimle birden fazla ürün olamaz.";
        public static string CategoryCount = "15'den fazla kategori var ise ürün ekleme olamaz.";
    }
}
