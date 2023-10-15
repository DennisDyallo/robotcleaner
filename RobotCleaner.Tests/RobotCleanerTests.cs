using Xunit.Abstractions;

namespace RobotCleaner.Tests;

public class RobotCleanerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly RobotCleaner _robotCleaner;
    const string ExampleInput = "M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]";

    public RobotCleanerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _robotCleaner = RobotCleaner.Create(ExampleInput);
    
    }
    [Fact]
    public void MapIsNotEmptyWhenCreated() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.Map));

    [Fact]
    public void HasMoveInstructions() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.MoveInstructions));

    [Fact]
    public void HasMoveInstructionsWhenCreated() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.MoveInstructions));

    [Fact]
    public void HasCurrentPositionWhenCreated() => Assert.NotNull(_robotCleaner.CurrentPosition);

    [Fact]
    public void CurrentPositionIsStartingPositionWhenCreated()
    {
        Assert.True(_robotCleaner.CurrentPosition.X == -5);
        Assert.True(_robotCleaner.CurrentPosition.Y == 5);
    }
    
    [Fact]
    public void CanMoveNorth()
    {
        var robotCleaner = RobotCleaner.Create("[N1];S:0,0;M:0,1,0,1");
        robotCleaner.LetsGo();
        var length = robotCleaner.DisplayVisitedPositions.Split(';').Length;
        _testOutputHelper.WriteLine($"Visited Positions: {robotCleaner.DisplayVisitedPositions}, Count: {length}");
        Assert.Equal(2, length);
        Assert.Contains("0,1", robotCleaner.DisplayVisitedPositions);
    }
    
    [Fact]
    public void RobotOutputsExpectedResult()
    {
        _robotCleaner.LetsGo();
        _testOutputHelper.WriteLine("All: " +_robotCleaner.DisplayVisitedPositions);

        const string expected = "-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-9,5;-8,5;-7,5;-6,5;-5,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7";
        Assert.Equal(expected.Split(";").Length, _robotCleaner.DisplayVisitedPositions.Split(";").Length);

    }
    
    [Fact]
    public void RobotOutputsExpectedUnique()
    {
        _robotCleaner.LetsGo();
        _testOutputHelper.WriteLine("Unique: " +_robotCleaner.DisplayUniqueVisitedPositions);

        const string expectedUnique = "-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7";
        Assert.Equal(expectedUnique.Split(";").Length, _robotCleaner.DisplayUniqueVisitedPositions.Split(";").Length);
        Assert.Equal(expectedUnique, _robotCleaner.DisplayUniqueVisitedPositions);
    }
    
    [Fact]
    public void RobotCannotMoveNorthBeyondMap()
    {
        var robotCleaner = RobotCleaner.Create("[N1,N1];S:0,0;M:0,0,0,1");
        Assert.Throws<ImpossibleMoveException>(() => robotCleaner.LetsGo());
    }
}