using PhoneBook.Domain.Entities;

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
        Task<bool> CreateReportAsync(PhoneBookReports report);
        /// <summary>
        /// UUID • Raporun Talep Edildiği Tarih • Rapor Durumu (Hazırlanıyor, Tamamlandı)
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<PhoneBookReports> GetPhoneBookLocationReportStatusAsync(string traceId);
        /// <summary>
        /// • Konum Bilgisi • O konumda yer alan rehbere kayıtlı kişi sayısı • O konumda yer alan rehbere kayıtlı telefon numarası sayısı
        /// </summary>
        /// <param name="mongodbId"></param>
        /// <returns></returns>
        Task<PhoneBookReports> GetPhoneBookReportDetailAsync(string traceId);


        //Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi

        /// <summary>
        /// Rehbere kişi ekle
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task CreateAsync(Domain.Entities.PhoneBook phoneBook);
        
        /// <summary>
        /// Rehberdeki ilgili öğeyi güncelle
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Domain.Entities.PhoneBook phoneBook);

        /// <summary>
        /// Rehberden kişiyi kaldır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
        /// <summary>
        /// Domainden dtoya emaillistesi
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        List<Domain.Dto.EmailInfoDto> EmailList(List<EmailInfo> lst);
        /// <summary>
        /// Domainden dtoya phonelistesi
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        List<Domain.Dto.PhoneInfoDto> PhoneList(List<PhoneInfo> lst);


    }
}
