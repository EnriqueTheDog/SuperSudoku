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
            Solver solver = new Solver();
            solver.Start();
        }
    }



}
