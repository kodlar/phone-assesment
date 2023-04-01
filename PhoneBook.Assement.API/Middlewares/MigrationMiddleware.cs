using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;


namespace PhoneBook.API.Middlewares
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

            var phoneBookCollection = _context.GetCollection<Domain.Entities.PhoneBook>(_settings.CollectionName);
            var phoneBookReportCollection = _context.GetCollection<Domain.Entities.PhoneBookReports>(_settings.ReportCollectionName);

            _context.SeedData(phoneBookCollection, phoneBookReportCollection);
            
            await _next(httpContext); // calling next middleware

        }
    }
}
