using LoggerMicroservice.DataAccess;
using LoggerMicroservice.Domain;
using LoggerMicroservice.Repository.Contracts;
using LoggerMicroservice.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace LoggerMicroservice.Repository;

public class LoggerRepository:ILoggerRepository
{
    private IServiceProvider serviceProvider;
    private ILoggerMapper loggerMapper;
    private IJwtService jwtService;
    public LoggerRepository(IServiceProvider serviceProvider, ILoggerMapper loggerMapper,IJwtService jwtService)
    {
        this.serviceProvider = serviceProvider;
        this.loggerMapper = loggerMapper;
        this.jwtService = jwtService;
    }
   

    public async Task LogInfo(string message, string? token)
    {
        if (String.IsNullOrEmpty(message))
        {
            throw new Exception("Cannot log an empty message!");
        }

        var loggedMessage = new LoggerMessage();
        loggedMessage.Message = message;
       
        if (token is not null)
        {
            loggedMessage.Source = jwtService.GetUserEmailFromToken(token);
        }
        
        
        var log = loggerMapper.LogDataAccessFromDomain(loggedMessage, LogStatus.Info);

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task LogError(string message, string source)
    {
        if (String.IsNullOrEmpty(message))
        {
            throw new Exception("Cannot log an empty message!");
        }
     
        if (String.IsNullOrEmpty(source))
        {
            throw new Exception("Cannot log an empty source!");
        }
        var loggedMessage = new LoggerMessage();
        loggedMessage.Message = message;
        loggedMessage.Source = source;
        var log = loggerMapper.LogDataAccessFromDomain(loggedMessage, LogStatus.Error);

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}