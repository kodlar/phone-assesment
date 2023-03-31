using MongoDB.Driver;

namespace PhoneBook.Infrastructure.Data
{
    public static class PhoneBookReportContextSeed
    {
        public static void SeedData(IMongoCollection<Domain.Entities.PhoneBookReports> phoneBooksReportCollection)
        {
            bool existPhoneBookReport = phoneBooksReportCollection.Find(p => true).Any();
            if (!existPhoneBookReport)
            {
                phoneBooksReportCollection.InsertManyAsync(GetConfigurePhoneBookReports());
            }


        }

        public static IEnumerable<Domain.Entities.PhoneBookReports> GetConfigurePhoneBookReports()
        {
            return new List<Domain.Entities.PhoneBookReports>()
            {
                new Domain.Entities.PhoneBookReports(){
                    CreatedAt= DateTime.Now,
                    Report = new Domain.Entities.LocationReportItem()
                    {
                        Location = "Italy",
                        PersonCount= 1,
                        PhoneNumberCount = 1,
                    },
                    Status = Domain.Enums.ReportEnum.Completed,
                    UpdatedAt= DateTime.Now,
                    TraceReportId = Guid.NewGuid().ToString()
                },
                 new Domain.Entities.PhoneBookReports(){
                    CreatedAt= DateTime.Now,
                    Report = new Domain.Entities.LocationReportItem()
                    {
                        Location = "London",
                        PersonCount= 10,
                        PhoneNumberCount = 13,
                    },
                    Status = Domain.Enums.ReportEnum.Completed,
                    UpdatedAt= DateTime.Now,
                    TraceReportId = Guid.NewGuid().ToString()
                }
            };
        }

    }
}
