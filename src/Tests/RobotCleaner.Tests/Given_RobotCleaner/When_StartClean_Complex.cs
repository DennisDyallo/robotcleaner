// ReSharper disable All

using RobotCleaner.Tests.Given_RobotCleaner;

namespace RobotCleaner.Tests.Given_RobotCleanerNew;

public class When_StartClean_Complex : Arrange
{
    public When_StartClean_Complex()
    {
        Start = new(-5, 5);
        Commands = new[]
        {
            new Command("West", 5),
            new Command("East", 5),
            new Command("North", 4),
            new Command("East", 3),
            new Command("South", 2),
            new Command("West", 1),
        };
        Bounds = new Bounds(
            new Coordinate(-10, -10),
            new Coordinate(10, 10)
        );
    }

    [Fact]
    public void Should_have_expected_commands()
    {
        Assert.Equal(6, Result.Commands.Length);
    }

    [Fact]
    public void Should_contain_all_positions()
    {
        const string expected =
            "-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-9,5;-8,5;-7,5;-6,5;-5,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7";
        Assert.Equal(expected.Split(";").Length, Result.PositionsVisited.Count());
    }

    [Fact]
    public void Should_contain_correct_unique_positions()
    {
        const string expectedUnique =
            "-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7";
        Assert.Equal(expectedUnique.Split(";").Length, Result.UniquePositionsVisited.Count());
    }
}