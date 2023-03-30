namespace PhoneBook.Domain.Entities
{
    public class LocationReportItem
    {
        public string Location { get; set; }
        /// <summary>
        /// O konumda yer alan rehbere kayıtlı kişi sayısı
        /// </summary>
        public int PersonCount { get; set; }
        /// <summary>
        /// O konumda yer alan rehbere kayıtlı telefon numarası sayısı
        /// </summary>
        public int PhoneNumberCount { get; set; }

    }
}
