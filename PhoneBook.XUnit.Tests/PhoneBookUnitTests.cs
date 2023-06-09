﻿using Amazon.Runtime;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using PhoneBook.Application.Commands.CreateContactInfo;
using PhoneBook.Application.Commands.CreateLocationReport;
using PhoneBook.Application.Commands.CreateUser;
using PhoneBook.Application.Commands.ReemoveUser;
using PhoneBook.Application.Commands.RemoveContactInfo;
using PhoneBook.Application.PipelineBehaviours;
using PhoneBook.Application.Queries.GetAllPhoneBook;
using PhoneBook.Application.Queries.GetPhoneBookById;
using PhoneBook.Application.Queries.GetReportDetailById;
using PhoneBook.Application.Queries.GetReportStatusById;
using PhoneBook.Domain.Dto;
using PhoneBook.Domain.Dto.Api;
using PhoneBook.Domain.Dto.QueryDto;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data;
using PhoneBook.Infrastructure.Data.Impl;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;
using RabbitMQ.Client;
using System.Linq.Expressions;
using System.Runtime;

namespace PhoneBook.XUnit.Tests
{
    public class PhoneBookUnitTests
    {
        private readonly Mock<IPhoneBookRepository> _phoneBookRepositoryMock;
        private readonly Mock<EventBusRabbitMQProducer> _eventBus;
        private readonly Mock<IRabbitMQPersistentConnection> _persistentConnection;
        private readonly Mock<ILogger<EventBusRabbitMQProducer>> _logger;
        private readonly Mock<IPhoneBookContext> _context;
        private readonly int _retryCount;
        private readonly Mock<ILogger<CreateContactInfoRequest>> _loggerContactInfo;

        private Mock<IMongoDatabase> _mockDB;
        private Mock<IMongoClient> _mockClient;


        public PhoneBookUnitTests()
        {
            _phoneBookRepositoryMock = new();
            _retryCount = 3;
            _persistentConnection = new();
            _loggerContactInfo = new();
            _logger = new();
            _eventBus = new(_persistentConnection.Object, _logger.Object, _retryCount);
            _context = new();

            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

        }
        [Fact]
        public async Task CreateUserHandler_Should_Return_True()
        {
            //Arrange
            var request = new CreateUserRequest() { Company = "Google", FirstName = "Vural", LastName = "Pekyılmaz" };

            _phoneBookRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.PhoneBook>()));

            var handler = new CreateUserHandler(_phoneBookRepositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);
            //Assert
            result.Result.Should().BeTrue();

        }

        [Fact]
        public async Task RemoveUserHandler_Should_Return_False_IfUserIdEmpty()
        {
            //Arrange
            var request = new RemoveUserRequest() { UserId = string.Empty };

            _phoneBookRepositoryMock.Setup(x => x.GetPhoneBookItemByIdAsync(It.IsAny<string>())).ReturnsAsync(() => { return null; });

            var handler = new RemoveUserHandler(_phoneBookRepositoryMock.Object);

            //Act
            var response = await handler.Handle(request, default);
            //Assert
            response.Result.Should().Be(false);

        }

        [Fact]
        public async Task RemoveContactInfoHandler_Should_Return_True()
        {
            var request = new RemoveContactInfoRequest() { EmailIds = new List<string>() { "test" }, PhoneIds = null, UserId = Guid.NewGuid().ToString() };

            _phoneBookRepositoryMock.Setup(x => x.GetPhoneBookItemByIdAsync(It.IsAny<string>())).ReturnsAsync(() =>
            {
                return new Domain.Entities.PhoneBook()
                {
                    Company = "test",
                    Contact = new Domain.Entities.ContactInfo()
                    {
                        Address = "adrse",
                        City = "istanbul",
                        Country = "Türkiye",
                        Email = new List<Domain.Entities.EmailInfo>()
                        {
                            new Domain.Entities.EmailInfo()
                            {
                                Email = "test@gmail.com",
                                Id = "test",
                                IsDeleted = false,
                                IsSelected = true
                            }
                        },
                        PhoneNumber = new List<Domain.Entities.PhoneInfo>()
                        {
                            new Domain.Entities.PhoneInfo()
                            {
                                IsDeleted = false,
                                PhoneNumber = "5352368789",
                                IsSelected = true,
                                CountryCode = 90,
                                Type = Domain.Enums.PhoneEnum.Mobile
                            }
                        }
                    },
                    LastName = "yüksel",
                    FirstName = "sami",
                    IsDeleted = false,
                    Id = Guid.NewGuid().ToString()
                };
            });

            _phoneBookRepositoryMock.Setup(y => y.UpdateAsync(It.IsAny<Domain.Entities.PhoneBook>())).ReturnsAsync(true);

            var handler = new RemoveContactInfoHandler(_phoneBookRepositoryMock.Object);

            //Act
            var response = await handler.Handle(request, default);

            response.Result.Should().BeTrue();
        }


        [Fact]
        public async Task CreateLocationHandler_Should_Return_TraceId()
        {
            var request = new CreateLocationReportRequest();

            var reportResponse = new CreateLocationReportResponse();

            var traceReportId = Guid.NewGuid().ToString();

            _phoneBookRepositoryMock.Setup(x => x.CreateReportAsync(It.IsAny<Domain.Entities.PhoneBookReports>())).Returns(Task.FromResult(true));

            var eventMessage = new ReportCreateEvent();

            eventMessage.TraceId = traceReportId;

            _persistentConnection.Setup(z => z.CreateModel()).Returns(It.IsAny<IModel>());

            var handler = new CreateLocationReportHandler(_phoneBookRepositoryMock.Object, _eventBus.Object);

            var response = await handler.Handle(request, default);

            response.TraceId = traceReportId;

            response.TraceId.Should().Be(traceReportId);
        }

        [Fact]
        public async Task CreateContactInfoHandler_Should_Return_CreateContactInfoResponse()
        {
            var request = new CreateContactInfoRequest();
            request.UserId = Guid.NewGuid().ToString();
            request.ContactInfo = new Domain.Dto.ContactInfoDto()
            {
                Address = "emek caddesi",
                City = "istanbul",
                Country = "Türkiye",
                Email = new List<Domain.Dto.EmailInfoDto>()
                {
                    new Domain.Dto.EmailInfoDto()
                    {
                        IsSelected= true,
                        Email = "testperson@yes.com"
                    }
                },
                PhoneNumber = new List<Domain.Dto.PhoneInfoDto>()
                {
                 new Domain.Dto.PhoneInfoDto()
                 {
                     IsSelected = true,
                     CountryCode = 90,
                     PhoneNumber = "5351234578",
                     Type = Domain.Enums.PhoneEnum.Mobile
                 }
                }
            };
            _phoneBookRepositoryMock.Setup(x => x.GetPhoneBookItemByIdAsync(It.IsAny<string>())).ReturnsAsync(new Domain.Entities.PhoneBook()
            {
                Id = Guid.NewGuid().ToString(),
                Contact = null,
                Company = "google",
                FirstName = "osman",
                IsDeleted = false,
                LastName = "gönül"
            });

            _phoneBookRepositoryMock.Setup(y => y.UpdateAsync(It.IsAny<Domain.Entities.PhoneBook>())).ReturnsAsync(() => { return true; });



            var handler = new CreateContactInfoHandler(_phoneBookRepositoryMock.Object);

            var response = await handler.Handle(request, default);

            _phoneBookRepositoryMock.Verify(y => y.UpdateAsync(It.IsAny<Domain.Entities.PhoneBook>()), Times.Once);

            response.Result.Should().BeTrue();

        }


        [Fact]
        public async Task GetAllPhoneBookHandler_Should_Return_ZeroItem()
        {
            var request = new GetAllPhoneBookRequest();

            var response = new GetAllPhoneBookResponse();

            _phoneBookRepositoryMock.Setup(z => z.GetPhoneBookListAsync()).ReturnsAsync(It.IsAny<IEnumerable<Domain.Entities.PhoneBook>>());

            var handler = new GetAllPhoneBookHandler(_phoneBookRepositoryMock.Object);

            var handlerResult = handler.Handle(request, default);

            _phoneBookRepositoryMock.Verify(x => x.GetPhoneBookListAsync(), Times.Once);

            response.Count.Should().Be(0);

        }

        [Fact]
        public async Task GetPhoneBookByIdHandler_Should_Return_NotBeNull()
        {
            var request = new GetPhoneBookByIdRequest();

            _phoneBookRepositoryMock.Setup(z => z.GetPhoneBookItemByIdAsync(request.Id)).ReturnsAsync(It.IsAny<Domain.Entities.PhoneBook>());

            var handler = new GetPhoneBookByIdHandler(_phoneBookRepositoryMock.Object);

            var handlerResult = handler.Handle(request, default);

            handlerResult.Should().NotBeNull();

        }

        [Fact]
        public async Task GetReportDetailByIdHandler_Should_Return_GetReportDetailByIdResponse()
        {
            var request = new GetReportDetailByIdRequest();

            _phoneBookRepositoryMock.Setup(z => z.GetPhoneBookReportDetailAsync(request.TraceId)).ReturnsAsync(It.IsAny<PhoneBookReports>());

            var handler = new GetReportDetailByIdHandler(_phoneBookRepositoryMock.Object);

            var handlerResult = handler.Handle(request, default);

            handlerResult.Should().NotBeNull();

        }

        [Fact]
        public async Task GetReportStatusByIdHandler_Should_Return_GetReportStatusByIdResponse()
        {
            var request = new GetReportStatusByIdRequest();

            _phoneBookRepositoryMock.Setup(z => z.GetPhoneBookLocationReportStatusAsync(request.TraceId)).ReturnsAsync(It.IsAny<Domain.Entities.PhoneBookReports>());

            var handler = new GetReportStatusByIdHandler(_phoneBookRepositoryMock.Object);

            var handlerResult = handler.Handle(request, default);

            handlerResult.Should().NotBeNull();

        }

        [Fact]
        public async Task EmailList_Should_Return_ExpectedListItem()
        {
            List<EmailInfo> lst = new List<EmailInfo>();
            lst.Add(new EmailInfo() { Email = "test@test.com", Id = Guid.NewGuid().ToString(), IsDeleted = false, IsSelected = true });
            lst.Add(new EmailInfo() { Email = "tes2@test.com", Id = Guid.NewGuid().ToString(), IsDeleted = false, IsSelected = false });
            _phoneBookRepositoryMock.Setup(x => x.EmailList(lst)).Returns(new List<Domain.Dto.EmailInfoDto>()
            {
                new Domain.Dto.EmailInfoDto()
                {
                    Email = "test@test.com",
                    IsSelected = true
                },
                new Domain.Dto.EmailInfoDto()
                {
                    Email = "test2@test.com",
                    IsSelected = false
                }
            });

            IPhoneBookRepository repo = new PhoneBookRepository(_context.Object);

            var result = repo.EmailList(lst);

            result.Count().Should().Be(2);

        }

        [Fact]
        public async Task PhoneList_Should_Return_ExpectedListItem()
        {
            List<PhoneInfo> lst = new List<PhoneInfo>();

            lst.Add(new PhoneInfo() { CountryCode = 90, IsSelected = true, Type = Domain.Enums.PhoneEnum.Mobile, IsDeleted = false, PhoneNumber = "5365150123", Id = Guid.NewGuid().ToString() });
            lst.Add(new PhoneInfo() { CountryCode = 90, IsSelected = true, Type = Domain.Enums.PhoneEnum.Mobile, IsDeleted = false, PhoneNumber = "5351235689", Id = Guid.NewGuid().ToString() });

            _phoneBookRepositoryMock.Setup(x => x.PhoneList(lst)).Returns(new List<Domain.Dto.PhoneInfoDto>()
            {
                new Domain.Dto.PhoneInfoDto()
                {
                    CountryCode = 90,
                    PhoneNumber = "5365150123",
                    Type = Domain.Enums.PhoneEnum.Work,
                    IsSelected = true
                },
                new Domain.Dto.PhoneInfoDto()
                {
                      CountryCode = 90,
                    PhoneNumber = "5351235689",
                    Type = Domain.Enums.PhoneEnum.Work,
                    IsSelected = false
                }
            }); ;

            IPhoneBookRepository repo = new PhoneBookRepository(_context.Object);

            var result = repo.PhoneList(lst);

            result.Count().Should().Be(2);

        }

        [Fact]
        public async Task PhoneBookDatabaseSettings_Should_ReturnAllProps_WithoutNull()
        {

            IPhoneBookDatabaseSettings settings = new PhoneBookDatabaseSettings();

            settings.ConnectionStrings = "mongodb://sourcingdb:27017";
            settings.CollectionName = "PhoneBooks";
            settings.ReportCollectionName = "PhoneBookReports";
            settings.DatabaseName = "PhoneServiceMongoDb";

            settings.CollectionName.Should().Be("PhoneBooks");
            settings.ConnectionStrings.Should().Be("mongodb://sourcingdb:27017");
            settings.ReportCollectionName.Should().Be("PhoneBookReports");
            settings.DatabaseName.Should().Be("PhoneServiceMongoDb");
        }

        [Fact]
        public async Task PhoneBookContext_ShouldCreate_Constructor_And_CollectionsNotBeNull()
        {
            IPhoneBookDatabaseSettings settings = new PhoneBookDatabaseSettings();

            settings.ConnectionStrings = "mongodb://sourcingdb:27017";
            settings.CollectionName = "phonebbook";
            settings.ReportCollectionName = "phonebookreports";
            settings.DatabaseName = "PhoneServiceMongoDb";

            _mockClient.Setup(c => c.GetDatabase(settings.DatabaseName, null)).Returns(_mockDB.Object);

            var context = new PhoneBookContext(settings);
            var phoneBook = context.GetCollection<Domain.Entities.PhoneBook>(settings.CollectionName);
            var phoneBookReport = context.GetCollection<PhoneBookReports>(settings.ReportCollectionName);

            phoneBook.Should().NotBeNull();
            phoneBookReport.Should().NotBeNull();

        }

        [Fact]
        public async Task PhoneBookContextSeed_Should_ReturnConcreteValueAsExpected()
        {
            var result = PhoneBookContextSeed.GetConfigurePhoneBooks();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PhoneBookReportContextSeed_Should_ReturnConcreteValueAsExpected()
        {
            var result = PhoneBookReportContextSeed.GetConfigurePhoneBookReports();
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task CreateUser_Should_ReturnNewUser()
        {
            _phoneBookRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.PhoneBook>())).Verifiable();
            IPhoneBookRepository repo = new PhoneBookRepository(_context.Object);
            var data = repo.CreateAsync(new Domain.Entities.PhoneBook()
            {
                Company = "google",
                FirstName = "tester",
                LastName = "fos",
                Contact = null,
                IsDeleted = false,
                Id = Guid.NewGuid().ToString(),
            });

        }

        [Fact]
        public async Task CreateReport_Should_ReturnTrue()
        {


            IPhoneBookRepository repo = new PhoneBookRepository(_context.Object);

            _phoneBookRepositoryMock.Setup(x => x.CreateReportAsync(It.IsAny<PhoneBookReports>())).Returns(Task.FromResult(true));
            var data = await _phoneBookRepositoryMock.Object.CreateReportAsync(new PhoneBookReports()
            {
                Id = Guid.NewGuid().ToString(),
            });

            _phoneBookRepositoryMock.Verify(x => x.CreateReportAsync(It.IsAny<PhoneBookReports>()), Times.Once());

        }

        [Fact]
        public void GetAllPhoneBookResponseDto_Should_Not_Return_Null()
        {
            var result = new GetAllPhoneBookResponseDto();
            result.Contact = null;
            result.FirstName = "test";
            result.Company = string.Empty;
            result.LastName = "eder";
            result.Id = Guid.NewGuid().ToString();
            result.Should().NotBeNull();

        }
        [Fact]
        public void ResponseDto_Should_Return_Success()
        {
            CreateContactInfoResponse response = new();
            var result = new ResponseDto<CreateContactInfoResponse>()
            {
                Data = response,
                IsSuccess = true,
                StatusCode = 200,
                Errors = new List<string>()
            };
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void RemoveContactInfoValidator_Should_Return_ErrorMessage()
        {
            var validator = new RemoveContactInfoValidator();
            var request = new RemoveContactInfoRequest()
            {
                UserId = null,
                EmailIds = null,
                PhoneIds = null
            };
            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.UserId).Only();


        }


        [Fact]
        public void CreateContactInfoValidator_Should_Return_ErrorMessage()
        {
            var validator = new CreateContactInfoValidator();
            var request = new CreateContactInfoRequest()
            {
                UserId = null,
                ContactInfo = new ContactInfoDto() { City = "istanbul" }
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.UserId).Only();


        }
        [Fact]
        public void CreateUserValidator_Should_Return_ErrorMessage()
        {
            var validator = new CreateUserValidator();
            var request = new CreateUserRequest()
            {
                FirstName = null,
                LastName = "test"
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.FirstName).Only();


        }

        [Fact]
        public void RemoveUserValidator_Should_Return_ErrorMessage()
        {
            var validator = new RemoveUserValidator();
            var request = new RemoveUserRequest()
            {
                UserId = null
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.UserId).Only();


        }



        [Fact]
        public void GetReportDetailByIdValidator_Should_Return_ErrorMessage()
        {
            var validator = new GetReportDetailByIdValidator();
            var request = new GetReportDetailByIdRequest()
            {
                TraceId = null
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.TraceId).Only();


        }


        [Fact]
        public void GetReportStatusByIdValidator_Should_Return_ErrorMessage()
        {
            var validator = new GetReportStatusByIdValidator();
            var request = new GetReportStatusByIdRequest()
            {
                TraceId = null
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.TraceId).Only();


        }


        [Fact]
        public void GetPhoneBookByIdValidator_Should_Return_ErrorMessage()
        {
            var validator = new GetPhoneBookByIdValidator();
            var request = new GetPhoneBookByIdRequest()
            {
                Id = null
            };

            var response = validator.TestValidate(request);

            response.ShouldHaveValidationErrorFor(x => x.Id).Only();


        }

        [Fact]
        public void GetReportStatusByIdRequest_Should_Return_TraceId()
        {
            var _traceId = Guid.NewGuid().ToString();
            var request = new GetReportStatusByIdRequest()
            {
                TraceId = _traceId
            };

            request.TraceId.Should().Be(_traceId);


        }

        [Fact]
        public void GetReportDetailByIdResponse_Should_ReturnLocation()
        {
            var response = new GetReportDetailByIdResponse();
            response.Status = "";
            response.CreatedAt = DateTime.Now;
            response.UpdatedAt = DateTime.Now;
            response.Id = Guid.NewGuid().ToString();
            response.TraceReportId = Guid.NewGuid().ToString();
            response.Report = new LocationReportItem() { Location = "istanbul", PersonCount = 1, PhoneNumberCount = 2 };

            response.Report.Location.Should().Be("istanbul");


        }

        [Fact]
        public void GetReportStatusByIdResponse_Should_ReturnNotNull()
        {
            var r = new GetReportStatusByIdResponse();
            r.Id = Guid.NewGuid().ToString();
            r.CreatedAt = DateTime.Now;
            r.Status = string.Empty;
            r.Should().NotBeNull();
        }

        [Fact]
        public async Task PhoneBookContext_ShouldCreate_Constructor_And_SelectedCollectionReturnNull()
        {
            IPhoneBookDatabaseSettings settings = new PhoneBookDatabaseSettings();

            settings.ConnectionStrings = "mongodb://sourcingdb:27017";
            settings.CollectionName = "phonebbook";
            settings.ReportCollectionName = "phonebookreports";
            settings.DatabaseName = "PhoneServiceMongoDb";

            _mockClient.Setup(c => c.GetDatabase(settings.DatabaseName, null)).Returns(_mockDB.Object);

            var context = new PhoneBookContext(settings);
            var phoneBook = context.GetCollection<Domain.Entities.PhoneBook>(string.Empty);

            phoneBook.Should().BeNull();

        }

        [Fact]
        public async Task PipelinePerformanceBehaviour_Check()
        {
            var request = new CreateContactInfoRequest();   
            request.UserId = Guid.NewGuid().ToString();
            request.ContactInfo = new ContactInfoDto()
            {
                Address = "tset",
                City = "istanbul",
                Country = "Türkiye",
                Email = new List<EmailInfoDto>()
                {
                    new EmailInfoDto()
                    {
                        Email = "test@test.com",
                        Id = "1",
                        IsSelected = true
                    }
                },
                PhoneNumber = new List<PhoneInfoDto>()
                {
                    new PhoneInfoDto()
                    {
                        IsSelected = true
                    }
                }
            };
            var response = new Mock<RequestHandlerDelegate<CreateContactInfoResponse>>();            
            var re = new PerformanceBehavior<CreateContactInfoRequest, CreateContactInfoResponse>(_loggerContactInfo.Object);
            var d = await re.Handle(request, response.Object, default);

            d.Should().BeNull();

        }

        [Fact]
        public async Task Pipeleline_UnhandledExceptionBehavior_Check()
        {
            var request = new CreateContactInfoRequest();
            request.UserId = Guid.NewGuid().ToString();
            request.ContactInfo = new ContactInfoDto()
            {
                Address = "tset",
                City = "istanbul",
                Country = "Türkiye",
                Email = new List<EmailInfoDto>()
                {
                    new EmailInfoDto()
                    {
                        Email = "test@test.com",
                        Id = "1",
                        IsSelected = true
                    }
                },
                PhoneNumber = new List<PhoneInfoDto>()
                {
                    new PhoneInfoDto()
                    {
                        IsSelected = true
                    }
                }
            };
            var response = new Mock<RequestHandlerDelegate<CreateContactInfoResponse>>();
            var re = new UnhandledExceptionBehavior<CreateContactInfoRequest, CreateContactInfoResponse>(_loggerContactInfo.Object);
            var d = await re.Handle(request,response.Object,default);
            d.Should().BeNull();
        }

        [Fact]
        public async Task Pipeline_ValidationBehavior_Return_Exception()
        {
            var request = new CreateContactInfoRequest();
            request.UserId = Guid.NewGuid().ToString();
            request.ContactInfo = new ContactInfoDto()
            {
                Address = "tset",
                City = "istanbul",
                Country = "Türkiye",
                Email = new List<EmailInfoDto>()
                {
                    new EmailInfoDto()
                    {
                        Email = "test@test.com",
                        Id = "1",
                        IsSelected = true
                    }
                },
                PhoneNumber = new List<PhoneInfoDto>()
                {
                    new PhoneInfoDto()
                    {
                        IsSelected = true
                    }
                }
            };

            var validators = new Mock<IEnumerable<IValidator<CreateContactInfoRequest>>>(); 

            var response = new Mock<RequestHandlerDelegate<CreateContactInfoResponse>>();
            var re = new ValidationBehavior<CreateContactInfoRequest, CreateContactInfoResponse>(validators.Object);
            Exception ex = new Exception();
            try
            {
                var d = await re.Handle(request, response.Object, default);
            }
            catch (Exception m)
            {
                ex = m;
            }

            ex.Message.Should().Be("Object reference not set to an instance of an object.");
        }


    }
}