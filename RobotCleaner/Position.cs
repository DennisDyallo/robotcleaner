using System.Text.RegularExpressions;

namespace RobotCleaner;

public record Position
{
    public Position(string input)
    {
        var match = Regex.Match(input, @"(-?\d+),(-?\d+)");
        X = int.Parse(match.Groups[1].Value);
        Y = int.Parse(match.Groups[2].Value);
    }

    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString() => $"{X},{Y}";
}