namespace RobotCleaner.Api.Features.Clean;

public interface IClean
{
    public Task<Result> ExecuteAsync(Request request);

    public record Result
    {
        public DateTimeOffset TimeStamp { get; init; }
        public int Commands { get; init; }
        public int RoomsCleaned { get; init; }
        public TimeSpan Duration { get; init; }
    }

    public record Request
    {
        public Coordinate Start { get; init; }
        public Command[] Commands { get; init; }
        public Bounds? Bounds { get; init; }
    };
}