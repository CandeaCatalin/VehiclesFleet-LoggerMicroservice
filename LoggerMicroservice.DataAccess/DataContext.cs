using LoggerMicroservice.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoggerMicroservice.DataAccess;

public class DataContext :DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    

    public DbSet<Log> Logs { get; set; }
}