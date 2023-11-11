using CtorMock.NSubstitute;
using NSubstitute;
using RobotCleaner.Api.Usecases.SaveCommands;

namespace RobotCleaner.Api.Tests.Usecases.Given_SaveCommands;

public class Arrange : MockBase<SaveCommands>
{
    protected Arrange()
    {
        Mocker.MockOf<ISaveCommandsRepository>()!.SaveAsync(Arg.Any<Execution>()).ReturnsForAnyArgs(SaveAsyncResult);
    }
    protected virtual Execution SaveAsyncResult => null!;
    protected virtual Request Request => new();
    protected virtual ISaveCommands.SaveCommandsResult SaveCommandsResult => Subject.ExecuteAsync(Request).Result;
}

public class When_ExecuteAsync : Arrange
{
    protected override Execution SaveAsyncResult => new()
    {
        Commands = 1,
        Result = 2
    };

    protected override Request Request => new()
        {
            Start = new Coordinate(0, 0),
            Commands = new[] { new Command("West", 1) }
        };


    [Fact]
    public void With_ValidRequest_ReturnsExpectedResult()
    {
        Assert.Equal(1, SaveCommandsResult.Commands);
        Assert.Equal(2, SaveCommandsResult.Result);
    }

    [Fact]
    public async Task With_TooLowPosition_ThrowsException()
    {
        var tooLowPosition = new Request
        {
            Start = new Coordinate(0, int.MaxValue),
            Commands = Enumerable.Range(0, 1).Select(p => new Command("East", 1)).ToArray()
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await Subject.ExecuteAsync(tooLowPosition));
    }

    [Fact]
    public async Task With_TooHighPosition_ThrowsException()
    {
        var tooHighPosition = new Request
        {
            Start = new Coordinate(0, int.MaxValue),
            Commands = Enumerable.Range(0, 1).Select(p => new Command("East", 1)).ToArray()
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await Subject.ExecuteAsync(tooHighPosition));
    }

    [Fact]
    public async Task With_TooManyCommands_ThrowsException()
    {
        var tooManyCommands = new Request
        {
            Start = new Coordinate(0, 0),
            Commands = Enumerable.Range(0, 10001).Select(p => new Command("East", 1)).ToArray()
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await Subject.ExecuteAsync(tooManyCommands));
    }
}

// public class When_RunQuery : Arrange
// {
//     private readonly IEnumerable<Product> _result;
//     protected override string ConnectionString => "myConnectionString";
//
//     protected override IEnumerable<DbObject> SqlResult => new[]
//     {
//         new DbObject(),
//         new DbObject()
//     };
//
//     public When_RunQuery()
//         => _result = Subject.RunQuery("select * from products");
//
//     [Fact]
//     public void Should_Use_connectionString_from_appSettings()
//         => Mocker.MockOf<IMyDatabase>().Verify(v => v.Execute("myConnectionString", It.IsAny<string>()));
//
//     [Fact]
//     public void Should_map_result_to_products()
//         => Assert.Equal(2, _result.Count());
// }