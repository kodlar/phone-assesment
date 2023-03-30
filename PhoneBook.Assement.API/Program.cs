using Microsoft.Extensions.Options;
using PhoneBook.Infrastructure.Data.Impl;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);


ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddSingleton<IPhoneBookDatabaseSettings>(sp => sp.GetRequiredService<IOptions<IPhoneBookDatabaseSettings>>().Value);
builder.Services.Configure<PhoneBookDatabaseSettings>(configuration.GetSection(nameof(PhoneBookDatabaseSettings)));

builder.Services.AddScoped<IPhoneBookContext, PhoneBookContext>();
builder.Services.AddTransient<IPhoneBookRepository, PhoneBookRepository>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
