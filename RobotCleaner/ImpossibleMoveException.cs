namespace RobotCleaner;

public class ImpossibleMoveException : Exception
{
    public ImpossibleMoveException(string message) : base(message)
    {
    }
}