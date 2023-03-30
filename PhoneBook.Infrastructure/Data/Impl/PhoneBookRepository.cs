using MongoDB.Driver;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Infrastructure.Data.Impl
{
    public class PhoneBookRepository : IPhoneBookRepository
    {
        private readonly IPhoneBookContext _phoneBookContext;

        public PhoneBookRepository(IPhoneBookContext phoneBookContext)
        {
            _phoneBookContext = phoneBookContext;
        }

        public async Task CreateAsync(Domain.Entities.PhoneBook phoneBook)
        {
           await _phoneBookContext.PhoneBooks.InsertOneAsync(phoneBook);
        }

        public async Task CreateReportAsync(PhoneBookReports report)
        {
            await _phoneBookContext.PhoneBookReports.InsertOneAsync(report);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Domain.Entities.PhoneBook>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _phoneBookContext.PhoneBooks.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Domain.Entities.PhoneBook> GetPhoneBookItemByIdAsync(string id)
        {
            return await _phoneBookContext.PhoneBooks.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

       

        public async Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookItemByNameAsync(string name)
        {
            var filter1 = Builders<Domain.Entities.PhoneBook>.Filter.ElemMatch(p => p.FirstName, name);
            var filter2 = Builders<Domain.Entities.PhoneBook>.Filter.Eq("IsDeleted", false);
            var filter = Builders<Domain.Entities.PhoneBook>.Filter.And(filter1, filter2);
            return await _phoneBookContext.PhoneBooks.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookListAsync()
        {
            return await _phoneBookContext.PhoneBooks.Find(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<PhoneBookReports> GetPhoneBookLocationReportStatusAsync(string location)
        {
            return await _phoneBookContext.PhoneBookReports.Find(p => p.Report.Location == location).FirstOrDefaultAsync();
        }

        public async Task<PhoneBookReports> GetPhoneBookReportDetailAsync(string traceId)
        {
            return await _phoneBookContext.PhoneBookReports.Find(p => p.TraceReportId == traceId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Domain.Entities.PhoneBook phoneBook)
        {
            var updateResult = await _phoneBookContext.PhoneBooks.ReplaceOneAsync(filter: g => g.Id == phoneBook.Id, replacement: phoneBook);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
