using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace EFstokTakip
{
    internal class VeritabaniAyarlari
    {
        public static string Sunucu { get; set; } = "127.0.0.1,1433";
        public static string Veritabani { get; set; } = "StokTakipDb";
        public static string KullaniciAdi { get; set; } = "sa";
        public static string Sifre { get; set; } = "12";

        // Bu metot, EF6'nın (ADO.NET Model) istediği karmaşık metni oluşturur
        public static string GetEfBaglantiMetni()
        {
            // 1. Standart SQL Bağlantısını oluştur
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = Sunucu,
                InitialCatalog = Veritabani,
                UserID = KullaniciAdi,
                Password = Sifre,
                IntegratedSecurity = false,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true // Performans için önerilir
            };

            // 2. EF Metadatasını ekle (Model1 adını .edmx dosyanın adıyla aynı yap)
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlBuilder.ToString(),
                // Buradaki 'Model1' senin .edmx dosyanın adıdır. Eğer farklıysa değiştir.
                Metadata = @"res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl"
            };

            return entityBuilder.ToString();
        }
    }
}