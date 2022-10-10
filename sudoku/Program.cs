using System;
using sudoku.Classes;
using sudoku.Classes.SolvingTools;
using sudoku.Enums;
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

                ISuperTool tool;
                switch(Settings.TOOL)
                {
                    case ToolEnum.Tools.MatrixTool:
                        tool = new MatrixTool(grid.Matrix);
                        break;
                }

                var watch = System.Diagnostics.Stopwatch.StartNew();
                grid.Matrix = tool.SolveSudoku();
                grid.Print();

                if (tool.IsSolved()) { Console.WriteLine("Sudoku completed!"); }
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
