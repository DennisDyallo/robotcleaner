using CommunityToolkit.Diagnostics;

namespace RobotCleaner.Api.Features.Clean;

public class Clean : IClean
{
    private readonly ISaveCommandsRepository _saveCommandsRepository;
    private readonly RobotCleaner _robotCleaner;

    public Clean(ISaveCommandsRepository saveCommandsRepository, RobotCleaner robotCleaner)
    {
        _saveCommandsRepository = saveCommandsRepository;
        _robotCleaner = robotCleaner;
    }

    public async Task<IClean.Result> ExecuteAsync(IClean.Request request)
    {
        Validate(request);
        
        var cleanResult = _robotCleaner.StartClean(request.Start, request.Commands);
        var dto = ToDbItem(cleanResult);
        var saveResult = await _saveCommandsRepository.SaveAsync(dto);
        return ToResult(saveResult);
    }
    
    private static void Validate(IClean.Request request)
    {
        const int max = 100000;
        const int maxCommands = 10000;
        Guard.IsInRange(request.Commands.Length, 0, maxCommands+1, nameof(request.Commands));
        Guard.IsInRange(request.Start.X, -max-1, max+1, nameof(request.Start.X));
        Guard.IsInRange( request.Start.Y, -max-1, max+1, nameof(request.Start.Y));
    }

    private static IClean.Result ToResult(Execution saveResult) 
        => new()
        {
            Commands = saveResult.Commands,
            Duration = saveResult.Duration,
            RoomsCleaned = saveResult.Result,
            TimeStamp = saveResult.TimeStamp
        };

    private static Execution ToDbItem(
        RobotCleaner.CleanResult cleanResult) => new()
    {
        Commands = cleanResult.Commands.Length,
        Result = cleanResult.UniquePositionsVisited.Count(),
        Duration = cleanResult.Duration
    };
}