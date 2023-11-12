using System.Diagnostics;
namespace RobotCleaner;
public class RobotCleaner
{
    private Position CurrentPosition { get; set; } = null!;
    private readonly List<Coordinate> _visitedPositions = new();
    private Command[] _commands = Enumerable.Empty<Command>().ToArray();
    private Bounds _bounds = Bounds.Infinite;
    private IEnumerable<Coordinate> VisitedPositions => _visitedPositions.AsEnumerable();
    private IEnumerable<Coordinate> UniqueVisitedPositions => _visitedPositions.Distinct();
    public CleanResult StartClean(
        Coordinate start,
        Command[] commands,
        Bounds? bounds = null)
    {
        SetInitialState(start, commands, bounds);
        _visitedPositions.Add(new Coordinate(CurrentPosition.X, CurrentPosition.Y));

        var sw = new Stopwatch();
        sw.Start();
        
        foreach (var command in _commands)
            Move(command.Direction[0], command.Steps);

        sw.Stop();

        return new (){
            Duration = sw.Elapsed,
            PositionsVisited = VisitedPositions,
            UniquePositionsVisited = UniqueVisitedPositions,
            Commands = _commands
        };
    }

    private void SetInitialState(Coordinate start, Command[] commands, Bounds? bounds)
    {
        CurrentPosition = new Position(start);
        _commands = commands;
        _bounds = bounds ?? Bounds.Infinite;
    }

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
            
            _visitedPositions.Add(new Coordinate(CurrentPosition.X, CurrentPosition.Y));
        }
    }

    private bool CanMove((int dx, int dy) change, out string error)
    {
        //Map checks here
        var horizontalMove = change.dx is 1 or -1;
        if (horizontalMove)
        {
            var newX = CurrentPosition.X + change.dx;
            if (newX > _bounds.Max.X)
            {
                error = "About to go out of right-side of the map";
                return false;
            }

            if (newX < _bounds.Min.X)
            {
                error = "About to go out of left-side of the map";
                return false;
            }
        }

        var verticalMove = change.dy is 1 or -1;
        if (verticalMove)
        {
            var newY = CurrentPosition.Y + change.dy;
            if (newY > _bounds.Max.Y)
            {
                error = "About to go out of top of the map";
                return false;
            }

            if (newY < _bounds.Min.Y)
            {
                error = "About to go out of bottom of the map";
                return false;
            }
        }

        //Obstacle checks here

        error = "";
        return true;
    }

    public record struct CleanResult
    {
        public Command[] Commands { get; init; }
        public IEnumerable<Coordinate> PositionsVisited { get; init; }
        public IEnumerable<Coordinate> UniquePositionsVisited { get; init; }
        public TimeSpan Duration { get; init; }
    }
}