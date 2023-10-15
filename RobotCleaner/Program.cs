using Microsoft.Extensions.Logging;

namespace RobotCleaner
{
    internal abstract class Program
    {
        private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        static void Main(string[] args)
        {
            Console.WriteLine("Robot Cleaner!");
            // var userInput = args;
            var userInput = "M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]";
            
            var robotCleaner = RobotCleaner.Create(userInput, LoggerFactory.CreateLogger<RobotCleaner>());
            robotCleaner.LetsGo();
            Console.WriteLine($"""
                               All positions cleaned: {robotCleaner.DisplayVisitedPositions}
                               Unique positions cleaned: {robotCleaner.DisplayUniqueVisitedPositions}
                               Total positions cleaned: {robotCleaner.VisitedPositions.Count()}
                               Unique positions cleaned: {robotCleaner.UniqueVisitedPositions.Count()}
                               """);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}