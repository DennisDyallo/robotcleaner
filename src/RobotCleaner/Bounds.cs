namespace RobotCleaner;

public record struct Bounds(Coordinate Min, Coordinate Max)
{
    public static readonly Bounds Infinite =
        new()
        {
            Min = new Coordinate(int.MinValue, int.MinValue),
            Max = new Coordinate(int.MaxValue, int.MaxValue)
        };
}