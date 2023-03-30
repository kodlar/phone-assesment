using MongoDB.Driver;

namespace PhoneBook.Infrastructure.Data
{
    public static class PhoneBookContextSeed
    {
        public static void SeedData(IMongoCollection<Domain.Entities.PhoneBook> phoneBooksCollection)
        {
            bool existPhoneBook = phoneBooksCollection.Find(p => true).Any();
            if (!existPhoneBook)
            {
                phoneBooksCollection.InsertManyAsync(GetConfigurePhoneBooks());
            }

        }

        private static IEnumerable<Domain.Entities.PhoneBook> GetConfigurePhoneBooks()
        {
            return new List<Domain.Entities.PhoneBook>()
            {
                new Domain.Entities.PhoneBook()
                {
                    Company = "Google",
                    FirstName = "Hasan",
                    LastName = "Pekmez",
                    Contact = new Domain.Entities.ContactInfo()
                    {
                        Address = "3109 W 50th St",
                        City = "Minneapolis",
                        Country = "USA",
                        Email = new List<Domain.Entities.EmailInfo>()
                        {
                            new Domain.Entities.EmailInfo()
                            {
                                Email = "hasan.pekmez@gmail.com",
                                IsDeleted = false,
                                IsSelected = true
                            },
                            new Domain.Entities.EmailInfo()
                            {
                                Email = "hasanpekmez@yahoo.com",
                                IsDeleted = false,
                                IsSelected = false
                            }
                        },
                        PhoneNumber = new List<Domain.Entities.PhoneInfo>()
                        {
                            new Domain.Entities.PhoneInfo()
                            {
                                CountryCode = 1,
                                IsDeleted = false,
                                IsSelected = true,
                                PhoneNumber = "6129250555",
                                Type = Domain.Enums.PhoneEnum.Work

                            }
                        }
                    }
                },
                new Domain.Entities.PhoneBook()
                {
                    Company = "Apple",
                    FirstName = "Mehmet",
                    LastName = "Terim",
                    Contact = new Domain.Entities.ContactInfo()
                    {
                        Address = "4635 Greenwood Rd",
                        City = "Shreveport",
                        Country = "USA",   
                        Email = new List<Domain.Entities.EmailInfo>()
                        {
                            new Domain.Entities.EmailInfo()
                            {
                                Email = "mehmetterim@gmail.com",
                                IsDeleted = false,
                                IsSelected = true
                            }
                        },
                        PhoneNumber = new List<Domain.Entities.PhoneInfo>()
                        {
                            new Domain.Entities.PhoneInfo()
                            {
                                CountryCode = 1,
                                IsDeleted = false,
                                IsSelected = true,
                                PhoneNumber = "3186363367",
                                Type = Domain.Enums.PhoneEnum.Mobile

                            }
                        }

                    }
                },
                new Domain.Entities.PhoneBook()
                {
                    Company = "Meta",
                    FirstName = "Bülent",
                    LastName= "Zaraoğlu",
                    Contact = new Domain.Entities.ContactInfo()
                    {
                        City= "Weiterstadt",
                        Address = "In der Krumme 16-20",
                        Country = "Germany",
                       Email = new List<Domain.Entities.EmailInfo>()
                        {
                            new Domain.Entities.EmailInfo()
                            {
                                Email = "bulent.zaraoglu@gmail.com",
                                IsDeleted = false,
                                IsSelected = true
                            }
                        },
                        PhoneNumber = new List<Domain.Entities.PhoneInfo>()
                        {
                            new Domain.Entities.PhoneInfo()
                            {
                                CountryCode = 1,
                                IsDeleted = false,
                                IsSelected = true,
                                PhoneNumber = "061518708170",
                                Type = Domain.Enums.PhoneEnum.Home

                            }
                        }
                    }
                }
            };
        }

    }
}
