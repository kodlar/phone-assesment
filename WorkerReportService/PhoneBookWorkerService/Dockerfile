#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WorkerReportService/PhoneBookWorkerService/PhoneBookWorkerService.csproj", "WorkerReportService/PhoneBookWorkerService/"]
RUN dotnet restore "WorkerReportService/PhoneBookWorkerService/PhoneBookWorkerService.csproj"
COPY . .
WORKDIR "/src/WorkerReportService/PhoneBookWorkerService"
RUN dotnet build "PhoneBookWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhoneBookWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhoneBookWorkerService.dll"]