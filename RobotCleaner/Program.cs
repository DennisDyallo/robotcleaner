using Microsoft.Extensions.Logging;

using Console = System.Console;

namespace RobotCleaner
{
    internal abstract class Program
    {
        protected static int origRow;
        protected static int origCol;

        private static readonly ILoggerFactory LoggerFactory =
            Microsoft.Extensions.Logging.LoggerFactory.Create(builder => { builder.AddConsole(); });


        public static void Main(string[] args)
        {
            Console.WriteLine("Robot Cleaner!");
            // var userInput = args;
            // var userInput = "M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]";

            var userInput = "M:-1,1,-1,1;S:0,0;[E1]";
            var robotCleaner = RobotCleaner.Create(userInput, null);

            robotCleaner.LetsGo();
            var logger = LoggerFactory.CreateLogger("RobotCleaner");

            Console.Clear();

            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;


            int xRange = 0,
                yRange = 0;

            for (var y = robotCleaner.YMin; y <= robotCleaner.YMax; y++)
            {
                for (var x = robotCleaner.XMin; x <= robotCleaner.XMax; x++)
                {
                    Console.Write("#");
                }

                yRange++;
                Console.WriteLine();
            }

            var xPosStart = 0;
            var yPosStart = 0;

            var newMid = yRange / 2;
            var xPosNew = xPosStart + newMid;
            var yPosNew = yPosStart + newMid;

            WriteAt("@", xPosNew, yPosNew);
            Console.ReadLine();
        }

        private static int AddAbsolute(int x, int y)
        {
            return Math.Abs(x) + Math.Abs(y);
        }


        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        public static void Example()
        {
            // Clear the screen, then save the top and left coordinates.
            Console.Clear();
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;

            // Draw the left side of a 5x5 rectangle, from top to bottom.
            WriteAt("+", 0, 0);
            WriteAt("|", 0, 1);
            WriteAt("|", 0, 2);
            WriteAt("|", 0, 3);
            WriteAt("+", 0, 4);

            // Draw the bottom side, from left to right.
            WriteAt("-", 1, 4); // shortcut: WriteAt("---", 1, 4)
            WriteAt("-", 2, 4); // ...
            WriteAt("-", 3, 4); // ...
            WriteAt("+", 4, 4);

            // Draw the right side, from bottom to top.
            WriteAt("|", 4, 3);
            WriteAt("|", 4, 2);
            WriteAt("|", 4, 1);
            WriteAt("+", 4, 0);

            // Draw the top side, from right to left.
            WriteAt("-", 3, 0); // shortcut: WriteAt("---", 1, 0)
            WriteAt("-", 2, 0); // ...
            WriteAt("-", 1, 0); // ...
                                //
            WriteAt("All done!", 0, 6);
            Console.WriteLine();
        }
    }
}

// Console.Clear();
// Console.WriteLine($"Robot Cleaner! {i++}");
// Console.WriteLine($"""
//                All positions cleaned: {robotCleaner.DisplayVisitedPositions}
//                Unique positions cleaned: {robotCleaner.DisplayUniqueVisitedPositions}
//                Total positions cleaned: {robotCleaner.VisitedPositions.Count()}
//                Unique positions cleaned: {robotCleaner.UniqueVisitedPositions.Count()}
//                """);
// Console.WriteLine("Press any key to exit...");