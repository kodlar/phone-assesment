using MongoDB.Driver;

namespace PhoneBook.Infrastructure.Data.Interfaces
{
    public interface IPhoneBookContext
    {
        IMongoCollection<Domain.Entities.PhoneBook> PhoneBooks { get; }
    }
}
