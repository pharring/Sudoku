using System;
using System.Diagnostics;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldForegroundColor = Console.ForegroundColor;
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.ForegroundColor = oldForegroundColor;
            };

            try
            {
                //Go(".................................................................................");
                //Go("987654321........................................................................");
                Go("4..5..71......9.6..6.3..2853....45..542...176..72....4634..1.2..5.6......91..3..8");
                Go(".623.7...3.5.9.......6......79....35.2.7.4.9.54....28......3.......6.7.1...5.986.");
                //Go("8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..");
                //Go(".8....15.4.65.9.8......8..............2.4...33..8.1...9...7....6.......415.....9.");
            }
            finally
            {
                Console.ForegroundColor = oldForegroundColor;
            }
        }

        static void Go(string puzzle)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Puzzle: {0}", puzzle);

            var startPosition = (Console.CursorLeft, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(EmptyGrid);
            var finalPosition = (Console.CursorLeft, Console.CursorTop);

            // Paint the given values in white
            Console.ForegroundColor = ConsoleColor.White;
            var sudoku = new Sudoku(puzzle, (pos, val) =>
            {
                Console.SetCursorPosition(startPosition.CursorLeft + 2 + (pos % 9) * 4, startPosition.CursorTop + 1 + (pos / 9) * 2);
                Console.Write(val == 0 ? '.' : (char)(val + '0'));
            });


            bool solved;
            Stopwatch sw;

            try
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Blue;
                sw = Stopwatch.StartNew();
                solved = sudoku.Solve();
                sw.Stop();
            }
            finally
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(finalPosition.CursorLeft, finalPosition.CursorTop);
            }

            if (solved)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solved in {0:f2}ms", sw.Elapsed.TotalMilliseconds);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No solution found.");
            }
        }

        private static string EmptyGrid
            => @"╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╟───┼───┼───╫───┼───┼───╫───┼───┼───╢
║   │   │   ║   │   │   ║   │   │   ║
╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝
";
    }
}
