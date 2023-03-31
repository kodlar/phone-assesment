using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneBook.Application.Commands.CreateContactInfo;
using PhoneBook.Application.Commands.CreateLocationReport;
using PhoneBook.Application.Commands.CreateUser;
using PhoneBook.Application.Commands.ReemoveUser;
using PhoneBook.Application.Commands.RemoveContactInfo;
using PhoneBook.Infrastructure.Data.Interfaces;
using RabbitMQ.Client;

namespace PhoneBook.XUnit.Tests
{
    public class PhoneBookUnitTests
    {
        private readonly Mock<IPhoneBookRepository> _phoneBookRepositoryMock;
        private readonly Mock<EventBusRabbitMQProducer> _eventBus;
        private readonly Mock<IRabbitMQPersistentConnection> _persistentConnection;
        private readonly Mock<ILogger<EventBusRabbitMQProducer>> _logger;
        private readonly int _retryCount;
        public PhoneBookUnitTests()
        {
            _phoneBookRepositoryMock = new();
            _retryCount = 3;
            _persistentConnection = new();
            _logger = new();
            _eventBus = new(_persistentConnection.Object, _logger.Object, _retryCount);
        }
        [Fact]
        public async Task CreateUserHandler_Should_Return_True()
        {
            //Arrange
            var request = new CreateUserRequest() { Company = "Google", FirstName = "Vural", LastName = "Pekyılmaz"};

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
            var request = new RemoveContactInfoRequest() { EmailIds = new List<string>() {"test"}, PhoneIds = null, UserId = Guid.NewGuid().ToString() };

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

            _phoneBookRepositoryMock.Setup(x => x.CreateReportAsync(It.IsAny<Domain.Entities.PhoneBookReports>())).ReturnsAsync(true);

            var eventMessage = new ReportCreateEvent();
            eventMessage.TraceId = traceReportId;
            _persistentConnection.Setup(z => z.CreateModel()).Returns(It.IsAny<IModel>());
           
            var handler = new CreateLocationReportHandler(_phoneBookRepositoryMock.Object,_eventBus.Object);

            var response = await handler.Handle(request,default);

            response.TraceId = traceReportId;

            response.TraceId.Should().Be(traceReportId);
        }

        [Fact]
        public async Task CreateContactInfoHandler_Should_Return_CreateContactInfoResponse()
        {
            var request = new CreateContactInfoRequest();
            request.UserId= Guid.NewGuid().ToString();
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

    }
}