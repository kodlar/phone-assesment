using MongoDB.Driver;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;
using System.Diagnostics;

namespace PhoneBook.Infrastructure.Data.Impl
{
    public class PhoneBookContext : IPhoneBookContext
    {
        public PhoneBookContext(IPhoneBookDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionStrings);            
            Debug.WriteLine("ConnectionString:" + settings.ConnectionStrings);
            var database = client.GetDatabase(settings.DatabaseName);            
            Debug.WriteLine("DatabaseName:" + settings.DatabaseName);
            PhoneBooks = database.GetCollection<Domain.Entities.PhoneBook>(settings.CollectionName);
            PhoneBookReports = database.GetCollection<Domain.Entities.PhoneBookReports>(settings.ReportCollectionName);            
            Debug.WriteLine("PhoneBooksCollectionName:" + settings.CollectionName);
            PhoneBookContextSeed.SeedData(PhoneBooks);
            PhoneBookReportContextSeed.SeedData(PhoneBookReports);
        }

        public IMongoCollection<Domain.Entities.PhoneBook> PhoneBooks { get; }
        public IMongoCollection<Domain.Entities.PhoneBookReports> PhoneBookReports { get; }
    }
}
