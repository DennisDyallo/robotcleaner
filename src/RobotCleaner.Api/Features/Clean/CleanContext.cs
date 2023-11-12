using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RobotCleaner.Api.Features.Clean;

public class CleanContext : DbContext
{
    private readonly ILogger<CleanContext> _logger;
    public DbSet<Execution> Executions { get; set; } = null!;

    public CleanContext(DbContextOptions options, ILogger<CleanContext> logger):base(options)
    {
        _logger = logger;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, DateTime>(
            dateTimeOffset => dateTimeOffset.UtcDateTime, 
            dateTime => new DateTimeOffset(dateTime));

        modelBuilder.Entity<Execution>()
            .Property(e => e.TimeStamp)
            .HasConversion(dateTimeOffsetConverter);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.LogTo(line =>
        {
            var newGuid = Guid.NewGuid();
            var message = $"\n### NEW QUERY: {newGuid} ###\n{line}\n### END QUERY: {newGuid} ###\n";
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }, LogLevel.Information);
#endif

        base.OnConfiguring(optionsBuilder);
    }
}

public record Execution
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
    public int Commands { get; set; } 
    public int Result { get; set; }
    public TimeSpan Duration { get; set; }
}