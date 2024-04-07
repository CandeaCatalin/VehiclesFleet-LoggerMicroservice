using LoggerMicroservice.Domain;
using LoggerMicroservice.Repository.Contracts;
using LoggerMicroservice.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace VehiclesFleet_LoggerMicroservice.Controllers;

[ApiController]
[Route("logger")]
[Authorize]
public class LoggerController : ControllerBase
{
    private readonly ILoggerRepository logger;
    private readonly IJwtService jwtService;
    public LoggerController(ILoggerRepository logger, IJwtService jwtService)
    {
        this.logger = logger;
        this.jwtService = jwtService;
    }

    [HttpPost(Name = "log")]
    [Authorize]
    public IActionResult LogInfo(string message,LogStatus status)
    {
        var token = GetToken();
        var email = jwtService.GetUserEmailFromToken(token);
        logger.LogInfo(message,email,status);
        return Ok();
    }
    
    private string? GetToken()
    {
        if (Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue))
        {
            var token = authHeaderValue.ToString().Replace("Bearer ", "");
              
            return token;
        }

        return null;
    }
}
