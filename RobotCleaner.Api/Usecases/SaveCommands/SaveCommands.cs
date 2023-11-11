using System.Diagnostics;
using CommunityToolkit.Diagnostics;

namespace RobotCleaner.Api.Usecases.SaveCommands;

public interface ISaveCommands
{
    public Task<SaveCommandsResult> ExecuteAsync(Request request);
    public record struct SaveCommandsResult
    {
        public DateTimeOffset TimeStamp;
        public int Commands;
        public int Result;
        public TimeSpan Duration;
    }
}

public class SaveCommands : ISaveCommands
{
    private readonly ISaveCommandsRepository _repository;

    public SaveCommands(ISaveCommandsRepository repository)
    {
        _repository = repository;
    }

    public async Task<ISaveCommands.SaveCommandsResult> ExecuteAsync(Request request)
    {
        Validate(request);
        
        var robotCleaner = RobotCleaner.Create(request); //Todo DI this instance of RobotCleaner -- but how does it get the map and instructions?
        var sw = new Stopwatch();
        sw.Start();
        robotCleaner.LetsGo();
        sw.Stop();

        var dto = ToDb(robotCleaner, sw.Elapsed);
        var saveResult = await _repository.SaveAsync(dto);
        return ToResult(saveResult);
    }

    private static ISaveCommands.SaveCommandsResult ToResult(Execution saveResult) 
        => new()
        {
            Commands = saveResult.Commands,
            Duration = saveResult.Duration,
            Result = saveResult.Result,
            TimeStamp = saveResult.TimeStamp
        };

    private static void Validate(Request request)
    {
        const int max = 100000;
        const int maxCommands = 10000;
        Guard.IsInRange(request.Commands.Length, 0, maxCommands+1, nameof(request.Commands));
        Guard.IsInRange(request.Start.X, -max-1, max+1, nameof(request.Start.X));
        Guard.IsInRange( request.Start.Y, -max-1, max+1, nameof(request.Start.Y));
    }
    private static Execution ToDb(RobotCleaner robotCleaner, TimeSpan duration) => new()
    {
        Commands = robotCleaner.MoveInstructions.Length,
        Result = robotCleaner.UniqueVisitedPositions.Count(),
        Duration = duration
    };
}