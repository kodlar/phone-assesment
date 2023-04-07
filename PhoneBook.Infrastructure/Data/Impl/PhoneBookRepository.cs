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
            await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).InsertOneAsync(phoneBook);
        }

        public async Task<bool> CreateReportAsync(PhoneBookReports report)
        {
            await _phoneBookContext.GetCollection<PhoneBookReports>(nameof(PhoneBookReports).ToLower()).InsertOneAsync(report);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Domain.Entities.PhoneBook>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Domain.Entities.PhoneBook> GetPhoneBookItemByIdAsync(string id)
        {
            return await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).Find(p => p.Id == id).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookItemByNameAsync(string name)
        {
            var filter1 = Builders<Domain.Entities.PhoneBook>.Filter.ElemMatch(p => p.FirstName, name);
            var filter2 = Builders<Domain.Entities.PhoneBook>.Filter.Eq("IsDeleted", false);
            var filter = Builders<Domain.Entities.PhoneBook>.Filter.And(filter1, filter2);
            return await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.PhoneBook>> GetPhoneBookListAsync()
        {
            return await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).Find(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<PhoneBookReports> GetPhoneBookLocationReportStatusAsync(string traceId)
        {
            return await _phoneBookContext.GetCollection<PhoneBookReports>(nameof(PhoneBookReports).ToLower()).Find(p => p.TraceReportId == traceId).FirstOrDefaultAsync();
        }

        public async Task<PhoneBookReports> GetPhoneBookReportDetailAsync(string traceId)
        {
            return await _phoneBookContext.GetCollection<PhoneBookReports>(nameof(PhoneBookReports).ToLower()).Find(p => p.TraceReportId == traceId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Domain.Entities.PhoneBook phoneBook)
        {
            var updateResult = await _phoneBookContext.GetCollection<Domain.Entities.PhoneBook>(nameof(Domain.Entities.PhoneBook).ToLower()).ReplaceOneAsync(filter: g => g.Id == phoneBook.Id, replacement: phoneBook);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateAsync(PhoneBookReports phoneBookReports)
        {
            var updateResult = await _phoneBookContext.GetCollection<Domain.Entities.PhoneBookReports>(nameof(Domain.Entities.PhoneBookReports).ToLower()).ReplaceOneAsync(filter: g => g.Id == phoneBookReports.Id, replacement: phoneBookReports);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public List<Domain.Dto.EmailInfoDto> EmailList(List<EmailInfo> lst)
        {
            List<Domain.Dto.EmailInfoDto> retlst = new List<Domain.Dto.EmailInfoDto>();

            if (lst != null && lst.Any())
            {
                foreach (var lstItem in lst)
                {
                    if(lstItem.IsDeleted == false)
                    {
                        var item = new Domain.Dto.EmailInfoDto();
                        item.Id = lstItem.Id;
                        item.Email = lstItem.Email;
                        item.IsSelected = lstItem.IsSelected;
                        retlst.Add(item);
                    }
                    
                }
            }

            return retlst;
        }

        public List<Domain.Dto.PhoneInfoDto> PhoneList(List<PhoneInfo> lst)
        {
            List<Domain.Dto.PhoneInfoDto> retlst = new List<Domain.Dto.PhoneInfoDto>();
            if (lst != null && lst.Any())
            {
                foreach (var lstItem in lst)
                {
                    if(lstItem.IsDeleted == false)
                    {
                        var item = new Domain.Dto.PhoneInfoDto();
                        item.Id = lstItem.Id;
                        item.PhoneNumber = lstItem.PhoneNumber;
                        item.CountryCode = lstItem.CountryCode;
                        item.Type = lstItem.Type;
                        item.IsSelected = lstItem.IsSelected;
                        retlst.Add(item);
                    }
                    
                }
            }

            return retlst;
        }
    }
}
