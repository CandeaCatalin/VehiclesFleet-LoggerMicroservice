using System.Net;
using System.Text;
using System.Text.Json;
using LoggerMicroservice.Domain;
using LoggerMicroservice.Domain.CustomExceptions;
using LoggerMicroservice.Repository.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

namespace VehiclesFleet_LoggerMicroservice.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILoggerRepository loggerRepository;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerRepository loggerRepository)
    {
        this.next = next;
        this.loggerRepository = loggerRepository;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpStatusCodeException ex)
        {
            await LogErrorWithRequestInput(context, ex);

            await HandleStatusCodeException(context, ex);
        }
        catch (Exception ex)
        {
            await LogErrorWithRequestInput(context, ex);

            await HandleUnknownException(context, ex);
        }
    }

    private static Task HandleStatusCodeException(HttpContext context, HttpStatusCodeException ex)
    {
        context.Response.StatusCode = (int)ex.StatusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = ex.DisplayMessage }));
    }

    private static Task HandleUnknownException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { ex.Message }));
    }

    private async Task LogErrorWithRequestInput(HttpContext context, Exception ex)
    {
        var result = new StringBuilder();
       
        result.AppendLine($"Message: {ex.Message}");


        result.AppendLine($"Exception: {ex}");

        await loggerRepository.LogError(result.ToString(),"LoggerMicroservice");
    }
}