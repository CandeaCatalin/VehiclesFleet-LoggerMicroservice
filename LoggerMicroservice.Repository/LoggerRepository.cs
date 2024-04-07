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
    public LoggerRepository(IServiceProvider serviceProvider, ILoggerMapper loggerMapper, IJwtService jwtService)
    {
        this.serviceProvider = serviceProvider;
        this.loggerMapper = loggerMapper;
        this.jwtService = jwtService;
    }
   

    public async Task LogInfo(string message, string? email, LogStatus status)
    {
        if (String.IsNullOrEmpty(message))
        {
            throw new Exception("Cannot log an empty message!");
        }

        var loggedMessage = new LoggerMessage();
        loggedMessage.Message = message;
       

        if (email is not null)
        {
            loggedMessage.UserEmail = email;
        }
        
        var log = loggerMapper.LogDataAccessFromDomain(loggedMessage, status);

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}