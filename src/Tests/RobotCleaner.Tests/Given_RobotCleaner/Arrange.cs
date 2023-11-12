using CtorMock.NSubstitute;

namespace RobotCleaner.Tests.Given_RobotCleaner;

public class Arrange : MockBase<RobotCleaner>
{
    protected Coordinate Start = new();
    protected Command[] Commands = Array.Empty<Command>();
    protected Bounds Bounds = Bounds.Infinite;
    protected RobotCleaner.CleanResult Result => Subject.StartClean(Start, Commands, Bounds);
}