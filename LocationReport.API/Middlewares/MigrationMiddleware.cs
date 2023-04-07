using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;

namespace LocationReport.API.Middlewares
{
    public class MigrationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public MigrationMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("MigrationMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("MigrationMiddleware executing..");

            var _context = httpContext.RequestServices.GetRequiredService<IPhoneBookContext>();
            var _settings = httpContext.RequestServices.GetRequiredService<IPhoneBookDatabaseSettings>();
           
            var phoneBookReportCollection = _context.GetCollection<PhoneBook.Domain.Entities.PhoneBookReports>(_settings.ReportCollectionName);

            _context.SeedData(phoneBookReportCollection);

            await _next(httpContext); // calling next middleware

        }
    }
}
