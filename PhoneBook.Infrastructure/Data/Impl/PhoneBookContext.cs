﻿using MongoDB.Driver;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;

namespace PhoneBook.Infrastructure.Data.Impl
{
    public class PhoneBookContext : IPhoneBookContext
    {
        public PhoneBookContext(IPhoneBookDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionStrings);
            //Console.WriteLine("ConnectionString:" + settings.ConnectionStrings);
            var database = client.GetDatabase(settings.DatabaseName);
            //Console.WriteLine("DatabaseName:" + settings.DatabaseName);
            PhoneBooks = database.GetCollection<Domain.Entities.PhoneBook>(settings.CollectionName);
            PhoneBookReports = database.GetCollection<Domain.Entities.PhoneBookReports>(settings.ReportCollectionName);
            //Console.WriteLine("PhoneBooksCollectionName:" + settings.CollectionName);
            PhoneBookContextSeed.SeedData(PhoneBooks);
            PhoneBookReportContextSeed.SeedData(PhoneBookReports);
        }

        public IMongoCollection<Domain.Entities.PhoneBook> PhoneBooks { get; }
        public IMongoCollection<Domain.Entities.PhoneBookReports> PhoneBookReports { get; }
    }
}