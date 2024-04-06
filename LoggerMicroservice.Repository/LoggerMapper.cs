using LoggerMicroservice.DataAccess.Entities;
using LoggerMicroservice.Domain;
using LoggerMicroservice.Repository.Contracts;

namespace LoggerMicroservice.Repository;

public class LoggerMapper:ILoggerMapper
{
    public Log LogDataAccessFromDomain(LoggerMessage loggerMessage,LogStatus logStatus)
    {
        return new Log
        {
            Id = Guid.NewGuid(),
            Message = loggerMessage.Message,
            UserEmail = loggerMessage.UserEmail,
            Status = logStatus.ToString(),
            CreateTime = DateTime.Now
        };
    }
}