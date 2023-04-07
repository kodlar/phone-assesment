#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LocationReport.API/LocationReport.API.csproj", "LocationReport.API/"]
COPY ["PhoneBook.Application/PhoneBook.Application.csproj", "PhoneBook.Application/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["PhoneBook.Infrastructure/PhoneBook.Infrastructure.csproj", "PhoneBook.Infrastructure/"]
COPY ["PhoneBook.Domain/PhoneBook.Domain.csproj", "PhoneBook.Domain/"]
RUN dotnet restore "LocationReport.API/LocationReport.API.csproj"
COPY . .
WORKDIR "/src/LocationReport.API"
RUN dotnet build "LocationReport.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocationReport.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationReport.API.dll"]