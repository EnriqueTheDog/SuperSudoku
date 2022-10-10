using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Utils
{
    public static class MatrixHelper
    {
        

        // This method is for development only
        public static int[,] GetSampleSudoku()
        {
            int[,] basicSudoku = new int[,] {
                { 0, 6, 0, 1, 0, 0, 9, 0, 0 },
                { 8, 0, 1, 0, 0, 3, 0, 0, 6 },
                { 0, 2, 4, 0, 0, 0, 0, 0, 5 },
                { 0, 7, 8, 0, 6, 1, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 8 },
                { 0, 0, 0, 0, 3, 9, 0, 0, 1 },
                { 0, 8, 6, 4, 0, 0, 5, 0, 0 },
                { 9, 3, 0, 0, 0, 5, 0, 0, 0 },
                { 0, 0, 5, 3, 9, 8, 0, 0, 7 },
            };
            return basicSudoku;
        }
        // This method is for testing only
        public static int[,] GetSampleSolution()
        {
            int[,] basicSudoku = new int[,] {
                { 5, 6, 3, 1, 7, 4, 9, 8, 2 },
                { 8, 9, 1, 2, 5, 3, 4, 7, 6 },
                { 7, 2, 4, 9, 8, 6, 1, 3, 5 },
                { 3, 7, 8, 5, 6, 1, 2, 4, 9 },
                { 1, 5, 9, 7, 4, 2, 3, 6, 8 },
                { 6, 4, 2, 8, 3, 9, 7, 5, 1 },
                { 2, 8, 6, 4, 1, 7, 5, 9, 3 },
                { 9, 3, 7, 6, 2, 5, 8, 1, 4 },
                { 4, 1, 5, 3, 9, 8, 6, 2, 7 },
            };
            return basicSudoku;
        }
    }
}
