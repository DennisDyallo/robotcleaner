using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.RegularExpressions;

namespace RobotCleaner;

public record struct Request(Coordinate Start, Command[] Commands);
public record struct Coordinate(int X, int Y);
public record struct Command(string Direction, int Steps);

public partial class RobotCleaner
{
    public int XMin = int.MinValue;
    public int XMax = int.MaxValue;
    public int YMin = int.MinValue;
    public int YMax = int.MaxValue;
    public string Map = string.Empty;
    public string MoveInstructionsStr { get; }
    public string[] MoveInstructions => MoveInstructionsStr.Split(",");
    public Position CurrentPosition { get; }
    private readonly ILogger<RobotCleaner> _logger;
    private readonly List<Position> _visitedPositions = new();

    private RobotCleaner(string? map, string moveInstructionsStr, Position startingPosition, ILogger<RobotCleaner>? logger)
    {
        _logger = logger ?? NullLogger<RobotCleaner>.Instance;

        SetMap(map);

        MoveInstructionsStr = moveInstructionsStr;
        CurrentPosition = startingPosition;
    }

    private void SetMap(string? map)
    {
        if (map is null)
            return;

        var mapSegments = map.Split(',');
        XMin = int.Parse(mapSegments[0]);
        XMax = int.Parse(mapSegments[1]);
        YMin = int.Parse(mapSegments[2]);
        YMax = int.Parse(mapSegments[3]);
        Map = map;
    }

    public static RobotCleaner Create(Request request) //Todo eventually change properties of RobotCleaner
    {
        var moves = "";
        foreach (var command in request.Commands)
        {
            switch (command.Direction)
            {
                case "West":
                    moves += "W" + command.Steps + ",";
                    break;
                case "East":
                    moves += "E" + command.Steps  + ",";
                    break;
                case "North":
                    moves += "N" + command.Steps  + ",";
                    break;
                case "South":
                    moves += "S" + command.Steps  + ",";
                    break;
            }
        }

        var moveStr = $"[{moves[..^1]}]";
        var starStr = $"S:{request.Start.X},{request.Start.Y}";
        var input = $"{starStr};{moveStr}";
        if (input is null)
            throw new ArgumentNullException(nameof(input), "Input cannot be null");

        var moveInstructions = MoveInstructionsRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(moveInstructions))
            throw new ArgumentException("Move instructions cannot be invalid or empty");

        var startingPosition = StartingPositionRegex().Match(input).Groups[1].Value;
        return string.IsNullOrWhiteSpace(startingPosition)
            ? throw new ArgumentException("Starting position cannot be invalid or empty")
            : new RobotCleaner(null, moveInstructions, new Position(startingPosition), null);
    }

    public static RobotCleaner Create(string input, ILogger<RobotCleaner>? logger = null)
    {
        var loggerInput = logger ?? NullLogger<RobotCleaner>.Instance;
        if (input is null)
            throw new ArgumentNullException(nameof(input), "Input cannot be null");

        var map = MapRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(map))
            throw new ArgumentException("Map cannot be invalid or empty");

        var moveInstructions = MoveInstructionsRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(moveInstructions))
            throw new ArgumentException("Move instructions cannot be invalid or empty");

        var startingPosition = StartingPositionRegex().Match(input).Groups[1].Value;
        return string.IsNullOrWhiteSpace(startingPosition)
            ? throw new ArgumentException("Starting position cannot be invalid or empty")
            : new RobotCleaner(map, moveInstructions, new Position(startingPosition), loggerInput);
    }

    public IEnumerable<Position> VisitedPositions => new List<Position>(_visitedPositions);
    public IEnumerable<Position> UniqueVisitedPositions => _visitedPositions.Distinct();
    public string DisplayVisitedPositions => string.Join(';', _visitedPositions);
    public string DisplayUniqueVisitedPositions => string.Join(';', _visitedPositions.Distinct());

    public void LetsGo()
    {
        _visitedPositions.Add(new Position(CurrentPosition));
        try
        {
            foreach (var moveInstruction in MoveInstructions)
            {
                var direction = moveInstruction[0];
                var steps = int.Parse(moveInstruction[1..]);
                Move(direction, steps);
            }
        }
        catch (ImpossibleMoveException e)
        {
            _logger.LogInformation(
                $"""
                 Cannot proceed because of an error: {e.Message}
                 Here's what we cleaned so far: {DisplayUniqueVisitedPositions}
                 """);
            throw;
        }
    }

    public override string ToString()
    {
        return $"""
                Map: {Map}
                Starting Position: {CurrentPosition}
                Move Instructions: {MoveInstructionsStr}
                Current Position: {CurrentPosition}
                """;
    }

    [GeneratedRegex(@"M:(-?\d+,\d+,-?\d+,\d+)")]
    private static partial Regex MapRegex();

    [GeneratedRegex(@"S:(.*?);")]
    private static partial Regex StartingPositionRegex();

    [GeneratedRegex(@"\[([NSEWnsew\d,]+)]")]
    private static partial Regex MoveInstructionsRegex();

    private void Move(char direction, int steps)
    {
        var directionChanges = new Dictionary<char, (int dx, int dy)>
        {
            { 'N', (0, 1) },
            { 'S', (0, -1) },
            { 'E', (1, 0) },
            { 'W', (-1, 0) }
        };

        var change = directionChanges[direction];
        for (var step = 1; step <= steps; step++)
        {
            if (!CanMove(change, out var error))
                throw new ImpossibleMoveException(error);

            CurrentPosition.X += change.dx;
            CurrentPosition.Y += change.dy;
            _visitedPositions.Add(new Position(CurrentPosition));

            _logger.LogInformation($"Moved {direction} {step} to {CurrentPosition}");
        }
    }

    private bool CanMove((int dx, int dy) change, out string error)
    {
        //Map checks here
        if (change.dx is 1 or -1)
        {
            var newX = CurrentPosition.X + change.dx;
            if (newX > XMax)
            {
                error = "About to go out of right-side of the map";
                return false;
            }

            if (newX < XMin)
            {
                error = "About to go out of left-side of the map";
                return false;
            }
        }

        if (change.dy is 1 or -1)
        {
            var newY = CurrentPosition.Y + change.dy;
            if (newY > YMax)
            {
                error = "About to go out of top of the map";
                return false;
            }

            if (newY < YMin)
            {
                error = "About to go out of bottom of the map";
                return false;
            }
        }

        //Obstacle checks here

        error = "";
        return true;
    }
}