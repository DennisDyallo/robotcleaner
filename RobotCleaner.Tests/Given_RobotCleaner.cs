using Xunit.Abstractions;

namespace RobotCleaner.Tests;


public class Arrange<T>
{
    protected readonly ITestOutputHelper _testOutputHelper;
    protected readonly RobotCleaner _robotCleaner;
    protected const string ExampleInput = "M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]";
    public Arrange(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _robotCleaner = RobotCleaner.Create(ExampleInput);
    }
}

public class Given_RobotCleaner : Arrange<RobotCleaner>
{
    public Given_RobotCleaner(ITestOutputHelper testOutputHelper):base(testOutputHelper) 
    {
  
    }

    [Fact]
    public void When_Create_With_ValidRequest_ReturnsRobotCleaner()
    {
        var request = new Request(new Coordinate(0,0), new []
        {
            new Command("East", 1),
            new Command("East", 1)
        });
        var robotcleaner = RobotCleaner.Create(request);
        robotcleaner.LetsGo();
        Assert.Equal(3,  robotcleaner.VisitedPositions.Count());
    }
    
    [Fact]
    public void MapIsNotEmptyWhenCreated() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.Map));

    [Fact]
    public void HasMoveInstructions() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.MoveInstructionsStr));

    [Fact]
    public void HasMoveInstructionsWhenCreated() => Assert.False(string.IsNullOrWhiteSpace(_robotCleaner.MoveInstructionsStr));

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
