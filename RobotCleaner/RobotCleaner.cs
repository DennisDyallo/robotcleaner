using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RobotCleaner;

public partial class RobotCleaner
{
    private readonly ILogger<RobotCleaner>? _logger;
    private readonly List<Position> _visitedPositions = new();
    private readonly int _mapXMin;
    private readonly int _mapXMax;
    private readonly int _mapYMin;
    private readonly int _mapYMax;

    private RobotCleaner(string map, string moveInstructions, Position startingPosition, ILogger<RobotCleaner> logger)
    {
        _logger = logger;
        Map = map;
        var mapSegments = Map.Split(',');
        _mapXMin = int.Parse(mapSegments[0]);
        _mapXMax = int.Parse(mapSegments[1]);
        _mapYMin = int.Parse(mapSegments[2]);
        _mapYMax = int.Parse(mapSegments[3]);

        MoveInstructions = moveInstructions;
        StartingPosition = startingPosition;
        CurrentPosition = StartingPosition;
    }
    public string Map { get; }
    public string MoveInstructions { get; }
    public Position CurrentPosition { get; }
    public IEnumerable<Position> VisitedPositions => new List<Position>(_visitedPositions);
    public IEnumerable<Position> UniqueVisitedPositions => _visitedPositions.Distinct();
    public string DisplayVisitedPositions => string.Join(';', _visitedPositions);
    public string DisplayUniqueVisitedPositions => string.Join(';', _visitedPositions.Distinct());
    private Position StartingPosition { get; }

    public static RobotCleaner Create(string input, ILogger<RobotCleaner>? logger = null)
    {
        var _logger = logger ?? NullLogger<RobotCleaner>.Instance;
        if (input is null)
            throw new ArgumentNullException(nameof(input), "Input cannot be null");

        var map = MapRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(map))
            throw new ArgumentException("Map cannot be invalid or empty");

        var moveInstructions = MoveInstructionsRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(moveInstructions))
            throw new ArgumentException("Move instructions cannot be invalid or empty");

        var startingPosition = StartingPositionRegex().Match(input).Groups[1].Value;
        if (string.IsNullOrWhiteSpace(startingPosition))
            throw new ArgumentException("Starting position cannot be invalid or empty");

        return new RobotCleaner(map, moveInstructions, new Position(startingPosition), logger);
    }

    public void LetsGo()
    {
        _visitedPositions.Add(new Position(CurrentPosition));
        try
        {
            var moveInstructions = MoveInstructions.Split(',');
            foreach (var moveInstruction in moveInstructions)
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
                Starting Position: {StartingPosition}
                Move Instructions: {MoveInstructions}
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
        //Map checks
        if (change.dx is 1 or -1)
        {
            var newX = CurrentPosition.X + change.dx;
            if (newX > _mapXMax)
            {
                error = "About to go out of right-side of the map";
                return false;
            }
            if ( newX < _mapXMin)
            {
                error = "About to go out of left-side of the map";
                return false;
            }
        }

        if (change.dy is 1 or -1)
        {
            var newY = CurrentPosition.Y + change.dy;
            if (newY > _mapYMax)
            {
                error = "About to go out of top of the map";
                return false;
            }
            if (newY < _mapYMin)
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

public class ImpossibleMoveException : Exception
{
    public ImpossibleMoveException(string message) : base(message)
    {
    }
}

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