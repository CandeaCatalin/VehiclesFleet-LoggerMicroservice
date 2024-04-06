FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LoggerMicroservice/LoggerMicroservice.csproj", "LoggerMicroservice/"]
COPY ["LoggerMicroservice.DI/LoggerMicroservice.DI.csproj", "LoggerMicroservice.DI/"]
COPY ["LoggerMicroservice.Settings/LoggerMicroservice.Settings.csproj", "LoggerMicroservice.Settings/"]
COPY ["LoggerMicroservice.Services/LoggerMicroservice.Services.csproj", "LoggerMicroservice.Services/"]
COPY ["LoggerMicroservice.DataAccess/LoggerMicroservice.DataAccess.csproj", "LoggerMicroservice.DataAccess/"]
COPY ["LoggerMicroservice.Domain/LoggerMicroservice.Domain.csproj", "LoggerMicroservice.Domain/"]
COPY ["LoggerMicroservice.Repository/LoggerMicroservice.Repository.csproj", "LoggerMicroservice.Repository/"]
RUN dotnet restore "LoggerMicroservice/LoggerMicroservice.csproj"
COPY . .
WORKDIR "/src/LoggerMicroservice"
RUN dotnet build "LoggerMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoggerMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoggerMicroservice.dll"]
