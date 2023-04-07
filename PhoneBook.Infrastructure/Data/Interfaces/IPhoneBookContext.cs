using MongoDB.Driver;
using PhoneBook.Domain.Entities;

namespace PhoneBook.Infrastructure.Data.Interfaces
{
    public interface IPhoneBookContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
        Task SeedData(IMongoCollection<Domain.Entities.PhoneBook> phoneBook, IMongoCollection<PhoneBookReports> phoneBookReport);
        Task SeedData(IMongoCollection<Domain.Entities.PhoneBook> phoneBook);
        Task SeedData(IMongoCollection<PhoneBookReports> phoneBookReport);
    }
}
