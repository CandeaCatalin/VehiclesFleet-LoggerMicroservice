using LoggerMicroservice.Domain;

namespace LoggerMicroservice.Repository.Contracts;

public interface ILoggerRepository
{
    Task LogInfo(string message,string? email, LogStatus status);
}