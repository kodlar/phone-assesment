using MongoDB.Driver;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;

namespace PhoneBook.Infrastructure.Data.Impl
{
    public class PhoneBookContext : IPhoneBookContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public PhoneBookContext(IPhoneBookDatabaseSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionStrings);
            _db = _mongoClient.GetDatabase(settings.DatabaseName);
        }


        public Task SeedData(IMongoCollection<Domain.Entities.PhoneBook> phoneBook, IMongoCollection<PhoneBookReports> phoneBookReport)
        {
            PhoneBookContextSeed.SeedData(phoneBook);
            PhoneBookReportContextSeed.SeedData(phoneBookReport);
            return Task.CompletedTask;
        }
        public Task SeedData(IMongoCollection<Domain.Entities.PhoneBook> phoneBook)
        {
            PhoneBookContextSeed.SeedData(phoneBook);            
            return Task.CompletedTask;
        }
        public Task SeedData(IMongoCollection<PhoneBookReports> phoneBookReport)
        {            
            PhoneBookReportContextSeed.SeedData(phoneBookReport);
            return Task.CompletedTask;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _db.GetCollection<T>(name);
        }
    }
}
