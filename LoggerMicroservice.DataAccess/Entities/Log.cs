using System.ComponentModel.DataAnnotations;

namespace LoggerMicroservice.DataAccess.Entities;

public class Log
{
    [Key] public Guid Id { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
    public string Status { get; set; }
    public DateTime CreateTime { get; set; }
}