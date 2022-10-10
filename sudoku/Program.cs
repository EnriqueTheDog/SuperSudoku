using System;
using sudoku.Classes;
using sudoku.Utils;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            bool stop = false;
            do
            {
                Screen.PrintTitle();
                Console.Write("\nSelect a sudoku level (standard = 3) ");

                int size = int.Parse(Console.ReadLine());
                Grid grid = new Grid(size, true);
                grid.ReadInput();

                var watch = System.Diagnostics.Stopwatch.StartNew();
                Solver sudoku = new Solver(grid.Matrix);
                sudoku.SolveSudoku();
                grid.Matrix = sudoku.GetMatrixAs2D();
                grid.Print();

                if (sudoku.IsSolved()) { Console.WriteLine("Sudoku completed!"); }
                else { Console.WriteLine("This sudoku is impossible or has multiple solutions"); }
                watch.Stop();
                if (Settings.DEV_MODE) Console.WriteLine("this sudoku took " + watch.ElapsedMilliseconds + "ms to be solved");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            } while (!stop);

        }
    }



}
