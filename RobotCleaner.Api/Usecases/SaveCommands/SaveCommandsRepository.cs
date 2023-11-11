namespace RobotCleaner.Api.Usecases.SaveCommands;

public interface ISaveCommandsRepository
{
    public Task<Execution> SaveAsync(Execution result);
}

public class SaveCommandsRepository : ISaveCommandsRepository
{
    private readonly SaveCommandsContext _saveCommandsContext;

    public SaveCommandsRepository(SaveCommandsContext saveCommandsContext)
    {
        _saveCommandsContext = saveCommandsContext;
    }

    public async Task<Execution> SaveAsync(Execution result)
    {
        await _saveCommandsContext.AddAsync(result);
        await _saveCommandsContext.SaveChangesAsync();
        return result;
    }
}