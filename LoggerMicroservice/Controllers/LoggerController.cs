using LoggerMicroservice.Domain;
using LoggerMicroservice.Repository.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VehiclesFleet_LoggerMicroservice.Controllers;

[ApiController]
[Route("logger")]
[Authorize]
public class LoggerController : ControllerBase
{
    private readonly ILoggerRepository logger;
    public LoggerController(ILoggerRepository logger)
    {
        this.logger = logger;
    }

    [HttpPost(Name = "log")]
    [Authorize]
    public IActionResult LogInfo(string message,string? email,LogStatus status)
    {
        logger.LogInfo(message,email,status);
        return Ok();
    }
}
