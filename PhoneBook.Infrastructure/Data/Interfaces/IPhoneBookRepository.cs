namespace PhoneBook.Infrastructure.Data.Interfaces
{
    public interface IPhoneBookRepository
    {
        /// <summary>
        /// Rehberdeki kişilerin listelenmesi
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookListAsync();
        /// <summary>
        ///Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Domain.Entities.PhoneBook> GetPhoneBookItemByIdAsync(string id);
        Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookItemByNameAsync(string name);

        /// <summary>
        /// Deneme amaçlı rapor oluşturmak için event tetiklenir.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task CreateReportAsync(Domain.Entities.PhoneBookReports report);
        /// <summary>
        /// UUID • Raporun Talep Edildiği Tarih • Rapor Durumu (Hazırlanıyor, Tamamlandı)
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<Domain.Entities.PhoneBookReports> GetPhoneBookLocationReportStatusAsync(string location);
        /// <summary>
        /// • Konum Bilgisi • O konumda yer alan rehbere kayıtlı kişi sayısı • O konumda yer alan rehbere kayıtlı telefon numarası sayısı
        /// </summary>
        /// <param name="mongodbId"></param>
        /// <returns></returns>
        Task<Domain.Entities.PhoneBookReports> GetPhoneBookReportDetailAsync(string traceId);


        //Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi

        /// <summary>
        /// Rehbere kişi ekle
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task CreateAsync(Domain.Entities.PhoneBook product);
        
        /// <summary>
        /// Rehberdeki ilgili öğeyi güncelle
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Domain.Entities.PhoneBook product);

        /// <summary>
        /// Rehberden kişiyi kaldır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
    }
}
