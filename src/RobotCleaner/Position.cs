namespace RobotCleaner;

public record Position
{
    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public Position(Coordinate coordinate)
    {
        (X, Y) = coordinate;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString() => $"{X},{Y}";
}