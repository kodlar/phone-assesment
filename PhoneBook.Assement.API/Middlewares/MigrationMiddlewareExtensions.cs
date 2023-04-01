namespace PhoneBook.API.Middlewares
{
    public static class MigrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MigrationMiddleware>();
        }
    }
}
