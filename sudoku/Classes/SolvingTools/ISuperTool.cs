using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Classes.SolvingTools
{
    public interface ISuperTool
    {
        public int[,] SolveSudoku();

        public bool IsSolved();
    }
}
