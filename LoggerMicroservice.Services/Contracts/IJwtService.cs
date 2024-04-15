namespace LoggerMicroservice.Services.Contracts;

public interface IJwtService
{
     string GetUserEmailFromToken(string? token);
}