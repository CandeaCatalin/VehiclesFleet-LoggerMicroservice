using LoggerMicroservice.Domain.Dto;
using LoggerMicroservice.Repository.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace VehiclesFleet_LoggerMicroservice.Controllers;

[ApiController]
[Route("logger")]
public class LoggerController : ControllerBase
{
    private readonly ILoggerRepository logger;
    public LoggerController(ILoggerRepository logger)
    {
        this.logger = logger;
    }

    [HttpPost("logInfo")]
    public IActionResult LogInfo(LogInfoDto dto)
    {
        var token = GetToken();
        logger.LogInfo(dto.Message,token);
        return Ok();
    }
    [HttpPost("logError")]
    public IActionResult LogError(LogErrorDto dto)
    {
        logger.LogError(dto.Message,dto.Source);
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
