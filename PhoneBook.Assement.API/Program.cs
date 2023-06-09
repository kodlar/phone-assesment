using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PhoneBook.API.Middlewares;
using PhoneBook.Application.Bootstrap;
using PhoneBook.Application.Commands.CreateContactInfo;
using PhoneBook.Application.Commands.CreateLocationReport;
using PhoneBook.Application.Commands.CreateUser;
using PhoneBook.Application.Commands.ReemoveUser;
using PhoneBook.Application.Commands.RemoveContactInfo;
using PhoneBook.Application.Queries.GetAllPhoneBook;
using PhoneBook.Application.Queries.GetPhoneBookById;
using PhoneBook.Application.Queries.GetReportDetailById;
using PhoneBook.Application.Queries.GetReportStatusById;
using PhoneBook.Domain.Dto.Api;
using PhoneBook.Infrastructure.Data.Impl;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddSingleton<IPhoneBookDatabaseSettings>(sp => sp.GetRequiredService<IOptions<PhoneBookDatabaseSettings>>().Value);
builder.Services.Configure<PhoneBookDatabaseSettings>(configuration.GetSection(nameof(PhoneBookDatabaseSettings)));
builder.Services.AddScoped<IPhoneBookContext, PhoneBookContext>();
builder.Services.AddTransient<IPhoneBookRepository, PhoneBookRepository>();
builder.Services.AddApplication(configuration);
builder.Services
.AddHttpClient("reportservice", client => {
    client.BaseAddress = new Uri("http://LocationReport.API");
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//live ortamda bunu developmenta �ek
app.UseMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/GetAllPhoneBook", async ([FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var request = new GetAllPhoneBookRequest();
    var result = await _mediatr.Send(request, token);
    var response = new ResponseDto<GetAllPhoneBookResponse>();
    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapGet("/GetAllPhoneBookById/{id}", async ([FromRoute] string id, [FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var request = new GetPhoneBookByIdRequest();
    var response = new ResponseDto<GetPhoneBookByIdResponse>();
    if (string.IsNullOrEmpty(id))
    {
        var lst = new List<string>();
        lst.Add("Id field mandatory");
        response.Fail(lst, 400);
        response.IsSuccess = true;
        return response;
    }
    request.Id = id;
    var result = await _mediatr.Send(request, token);

    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapGet("/GetReportDetailById/{id}", async ([FromRoute] string id, IHttpClientFactory _httpClientFactory, CancellationToken token) =>
{
    
    var response = new ResponseDto<GetReportDetailByIdResponse>();
    if (string.IsNullOrEmpty(id))
    {
        var lst = new List<string>();
        lst.Add("Id field mandatory");
        response.Fail(lst, 400);
        response.IsSuccess = true;
        return response;
    }
    
  
    var httpClient = _httpClientFactory.CreateClient("reportservice");
    var result = await httpClient.GetFromJsonAsync<ResponseDto<GetReportDetailByIdResponse>>("/GetReportDetailById/"+ id);
    response.IsSuccess = true;
    response.Data = result.Data;
    return response;
});

app.MapGet("/GetReportStatusById/{id}", async ([FromRoute] string id, IHttpClientFactory _httpClientFactory, CancellationToken token) =>
{
   
    var response = new ResponseDto<GetReportStatusByIdResponse>();
    if (string.IsNullOrEmpty(id))
    {
        var lst = new List<string>();
        lst.Add("Id field mandatory");
        response.Fail(lst, 400);
        response.IsSuccess = true;
        return response;
    }
    

    var httpClient = _httpClientFactory.CreateClient("reportservice");
    var result = await httpClient.GetFromJsonAsync<ResponseDto<GetReportStatusByIdResponse>>("/GetReportStatusById/" + id);

    response.IsSuccess = true;
    response.Data = result.Data;
    return response;
});

app.MapPost("/CreateContactInfo", async ([FromBody] CreateContactInfoRequest request, [FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var result = await _mediatr.Send(request, token);
    var response = new ResponseDto<CreateContactInfoResponse>();
    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapPost("/CreateLocationReport", async (IHttpClientFactory _httpClientFactory, CancellationToken token) =>
{
    var response = new RootDto();
    var httpClient = _httpClientFactory.CreateClient("reportservice");
    
    var result = await httpClient.PostAsync("/CreateLocationReport",null);
    var responseBody = await result.Content.ReadAsStringAsync();
    var obj = JsonSerializer.Deserialize<RootDto>(responseBody);
    response = obj;
    return response;
});

app.MapPost("/CreateUser", async ([FromBody] CreateUserRequest request, [FromServices] IMediator _mediatr, CancellationToken token) =>
{
    var result = await _mediatr.Send(request, token);
    var response = new ResponseDto<CreateUserResponse>();
    response.IsSuccess = true;
    response.Data = result;
    return response;
});

app.MapDelete("/RemoveUser", async ([FromBody] RemoveUserRequest request, [FromServices] IMediator _mediatr, CancellationToken token) =>
   {
       var result = await _mediatr.Send(request, token);
       var response = new ResponseDto<RemoveUserResponse>();
       response.IsSuccess = true;
       response.Data = result;
       return response;
   });

app.MapDelete("/RemoveContactInfo", async ([FromBody] RemoveContactInfoRequest request, [FromServices] IMediator _mediatr, CancellationToken token) =>
    {
        var result = await _mediatr.Send(request, token);
        var response = new ResponseDto<RemoveContactInfoResponse>();
        response.IsSuccess = true;
        response.Data = result;
        return response;
    })
.WithOpenApi();

app.Run();


