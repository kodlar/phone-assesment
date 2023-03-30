namespace PhoneBook.Infrastructure.Settings
{
    public class PhoneBookDatabaseSettings : IPhoneBookDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string ReportCollectionName { get; set; }
    }
}
