using System.Text;
using System.Text.Json;
using LoggerMicroservice.DataAccess;
using LoggerMicroservice.Repository;
using LoggerMicroservice.Repository.Contracts;
using LoggerMicroservice.Services;
using LoggerMicroservice.Services.Contracts;
using LoggerMicroservice.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace LoggerMicroservice.DI;

public static class DependencyResolver
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        var configurationRoot = LoadConfiguration();

        services.AddSingleton(configurationRoot);
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        RegisterSwaggerWithAuthorization(services);
        services.AddSingleton<IAppSettingsReader, AppSettingsReader>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<ILoggerRepository, LoggerRepository>();
        services.AddSingleton<ILoggerMapper, LoggerMapper>();


        services.AddControllers();
        
        AddDataServices(services);
        AddAuthorization(services);
        DoMigrations(services);
        return services;
    }

    private static string GetConnectionString(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var appReader = serviceProvider.GetService<IAppSettingsReader>();
        return appReader.GetValue(AppSettingsConstants.Section.Database, AppSettingsConstants.Keys.ConnectionString);
    }

    public static void AddDataServices(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(GetConnectionString(services), b => b.MigrationsAssembly("LoggerMicroservice.DataAccess")));
    }

    private static void RegisterSwaggerWithAuthorization(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vehicles Fleet API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter the word Bearer following a space and token",
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }

    private static void DoMigrations(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var servicesproviders = scope.ServiceProvider;

            var context = servicesproviders.GetRequiredService<DataContext>();
            context.Database.Migrate();
        }
    }

    private static IConfigurationRoot LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        var configuration = builder.Build();
        return configuration;
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var appReader = serviceProvider.GetService<IAppSettingsReader>();
        string secretKey = appReader.GetValue(AppSettingsConstants.Section.Authorization,
            AppSettingsConstants.Keys.JwtSecretKey);


        var key = Encoding.UTF8.GetBytes(secretKey);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

        });
    }
}