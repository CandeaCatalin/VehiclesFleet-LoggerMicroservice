using LoggerMicroservice.Domain;

namespace LoggerMicroservice.Repository.Contracts;

public interface ILoggerRepository
{
    Task LogInfo(string message,string? token);
    Task LogError(string message,string source);
}