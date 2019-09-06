using System;
using System.Diagnostics;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {

            //var puzzle = new Sudoku(".................................................................................");
            //var puzzle = new Sudoku("987654321........................................................................");
            Go(".623.7...3.5.9.......6......79....35.2.7.4.9.54....28......3.......6.7.1...5.986.");
            Go("8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..");
            Go(".8....15.4.65.9.8......8..............2.4...33..8.1...9...7....6.......415.....9.");
        }

        static void Go(string puzzle)
        {
            var sudoku = new Sudoku(puzzle);
            var sw = Stopwatch.StartNew();
            var solved = sudoku.Solve();
            sw.Stop();
            if (solved)
            {
                Console.WriteLine("{0} solved in {1:f2}ms", puzzle, sw.Elapsed.TotalMilliseconds);
                Console.WriteLine(sudoku.ToString());
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
        }
    }
}
