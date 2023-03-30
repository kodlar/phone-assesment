using MongoDB.Driver;

namespace PhoneBook.Infrastructure.Data.Interfaces
{
    public interface IPhoneBookContext
    {
        IMongoCollection<Domain.Entities.PhoneBook> PhoneBooks { get; }
        IMongoCollection<Domain.Entities.PhoneBookReports> PhoneBookReports { get; }
    }
}
