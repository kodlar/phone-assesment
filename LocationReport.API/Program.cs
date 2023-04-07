using LocationReport.API.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PhoneBook.Application.Bootstrap;
using PhoneBook.Application.Commands.CreateLocationReport;
using PhoneBook.Application.Queries.GetReportDetailById;
using PhoneBook.Application.Queries.GetReportStatusById;
using PhoneBook.Domain.Dto.Api;
using PhoneBook.Infrastructure.Data.Impl;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddSingleton<IPhoneBookDatabaseSettings>(sp => sp.GetRequiredService<IOptions<PhoneBookDatabaseSettings>>().Value);
builder.Services.Configure<PhoneBookDatabaseSettings>(configuration.GetSection(nameof(PhoneBookDatabaseSettings)));
builder.Services.AddScoped<IPhoneBookContext, PhoneBookContext>();
builder.Services.AddTransient<IPhoneBookRepository, PhoneBookRepository>();
builder.Services.AddApplication(configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//live ortamda bunu developmenta çek
app.UseMigrations();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();


app.MapPost("/CreateLocationReport", async ([FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var request = new CreateLocationReportRequest();
    var result = await _mediatr.Send(request, token);
    var response = new ResponseDto<CreateLocationReportResponse>();
    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapGet("/GetReportDetailById/{id}", async ([FromRoute] string id, [FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var request = new GetReportDetailByIdRequest();
    var response = new ResponseDto<GetReportDetailByIdResponse>();
    if (string.IsNullOrEmpty(id))
    {
        var lst = new List<string>();
        lst.Add("Id field mandatory");
        response.Fail(lst, 400);
        response.IsSuccess = true;
        return response;
    }
    request.TraceId = id;
    var result = await _mediatr.Send(request, token);

    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapGet("/GetReportStatusById/{id}", async ([FromRoute] string id, [FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var request = new GetReportStatusByIdRequest();
    var response = new ResponseDto<GetReportStatusByIdResponse>();
    if (string.IsNullOrEmpty(id))
    {
        var lst = new List<string>();
        lst.Add("Id field mandatory");
        response.Fail(lst, 400);
        response.IsSuccess = true;
        return response;
    }
    request.TraceId = id;
    var result = await _mediatr.Send(request, token);

    response.IsSuccess = true;
    response.Data = result;
    return response;
}).WithOpenApi();

app.Run();

