using RobotCleaner.Tests.Given_RobotCleaner;
using Xunit.Abstractions;
// ReSharper disable All

namespace RobotCleaner.Tests.Given_RobotCleanerNew;

public class When_StartClean : Arrange
{
    private readonly ITestOutputHelper _testOutputHelper;

    public When_StartClean(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void With_OneCommand_Should_CleanThatPosition()
    {
        //Arrange
        Commands = new[] { new Command("North", 1) };
        Start = new Coordinate(0, 0);
        
        //Act
        var result = Result;
        
        //Assert
        Assert.Equal(2, result.PositionsVisited.Count()); // Starting position and one North step
        Assert.Contains(new Coordinate(0, 1), Result.PositionsVisited);
    }

    [Fact]
    public void With_OutOfBoundsCommands_Throws_ImpossibleMoveException()
    {
        //Arrange
        Commands = new[] { new Command("North", 2) };
        Bounds =   new Bounds
        {
            Min = new Coordinate(0, 0),
            Max = new Coordinate(0, 1)
        };
        
        //Act
        //Assert
        Assert.Throws<ImpossibleMoveException>(() =>
        {
            _testOutputHelper.WriteLine(Result.ToString());
        });
    }
}