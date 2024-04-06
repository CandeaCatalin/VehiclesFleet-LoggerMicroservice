using LoggerMicroservice.DataAccess.Entities;
using LoggerMicroservice.Domain;

namespace LoggerMicroservice.Repository.Contracts;

public interface ILoggerMapper
{
    public Log LogDataAccessFromDomain(LoggerMessage loggerMessage,LogStatus logStatus);
}