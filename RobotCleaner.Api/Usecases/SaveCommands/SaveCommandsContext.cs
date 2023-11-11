using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RobotCleaner.Api.Usecases.SaveCommands;

public class SaveCommandsContext : DbContext
{
    public DbSet<Execution> Executions { get; set; } = null!;

    public SaveCommandsContext(DbContextOptions options):base(options)
    {
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
    public int Id { get; init; }
    public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    public int Commands { get; set; }
    public int Result { get; set; }
    public TimeSpan Duration { get; set; }
}