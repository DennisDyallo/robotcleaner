namespace RobotCleaner.Api.Features.Clean;

public interface ISaveCommandsRepository
{
    public Task<Execution> SaveAsync(Execution result);
}

public class CleanRepository : ISaveCommandsRepository
{
    private readonly CleanContext _cleanContext;

    public CleanRepository(CleanContext cleanContext)
    {
        _cleanContext = cleanContext;
    }

    public async Task<Execution> SaveAsync(Execution result)
    {
        await _cleanContext.AddAsync(result);
        await _cleanContext.SaveChangesAsync();
        return result;
    }
}