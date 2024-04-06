using LoggerMicroservice.Domain;

namespace LoggerMicroservice.Repository.Contracts;

public interface ILoggerRepository
{
    Task LogInfo(LoggerMessage message, string? token);
    Task LogError(LoggerMessage message);
}