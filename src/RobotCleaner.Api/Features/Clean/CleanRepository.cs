namespace RobotCleaner.Api.Features.Clean;

public interface ICleanRepository
{
    public Task<Execution> SaveAsync(Execution result);
}

public class CleanRepository : ICleanRepository
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