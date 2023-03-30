using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Infrastructure.Data.Impl
{
    public class PhoneBookRepository : IPhoneBookRepository
    {
        public Task CreateAsync(Domain.Entities.PhoneBook product)
        {
            throw new NotImplementedException();
        }

        public Task CreateReportAsync(PhoneBookReports report)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Entities.PhoneBook> GetPhoneBookItemByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookItemByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PhoneBookReports> GetPhoneBookLocationReportStatusAsync(string location)
        {
            throw new NotImplementedException();
        }

        public Task<PhoneBookReports> GetPhoneBookReportDetailAsync(string traceId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Domain.Entities.PhoneBook product)
        {
            throw new NotImplementedException();
        }
    }
}
