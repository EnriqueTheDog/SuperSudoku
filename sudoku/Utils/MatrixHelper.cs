using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Utils
{
    public static class MatrixHelper
    {
        //Cell Checking --
        //If a cell has only one possible, returns that number
        //If not, returns 0
        //If it has 0 possibles (error) returns -1
        public static int GetCell(int[,,] mat, int x, int y)
        {
            int result = 0;
            int count = 0;
            for (int z = 0; z < Math.Cbrt(mat.Length); z++)
            {
                if (mat[x, y, z] != 0)
                {
                    result = z;
                    count++;
                }
            }
            return (count == 0) ? -1 : (count == 1) ? result + 1 : 0;
        }

        //Translates a 2d matrix to a 3d matrix
        // (1,4 = 7) = (1,4 = [0,0,0,0,0,0,1,0,0)
        // (1,4 = 0) = (1,4 = [1,1,1,1,1,1,1,1,1])
        public static int[,,] Transpose(int[,] matrix)
        {
            int[,,] blue = new int[Convert.ToInt32(Math.Sqrt(matrix.Length)), Convert.ToInt32(Math.Sqrt(matrix.Length)), Convert.ToInt32(Math.Sqrt(matrix.Length))];
            for (int y = 0; y < Math.Sqrt(matrix.Length); y++)
            {
                for (int x = 0; x < Math.Sqrt(matrix.Length); x++)
                {
                    if (matrix[x, y] != 0)
                    {
                        blue[x, y, matrix[x, y] - 1] = 1;
                    }
                    else
                    {
                        for (int z = 0; z < Math.Sqrt(matrix.Length); z++)
                        {
                            blue[x, y, z] = 1;
                        }
                    }
                }
            }
            return blue;
        }

        //This one re-translates 3d info to a 2d matrix
        public static int[,] Transpose(int[,,] matrix)
        {
            int[,] blue = new int[Convert.ToInt32(Math.Cbrt(matrix.Length)), Convert.ToInt32(Math.Cbrt(matrix.Length))];
            for (int x = 0; x < Math.Cbrt(matrix.Length); x++)
            {
                for (int y = 0; y < Math.Cbrt(matrix.Length); y++)
                {
                    blue[x, y] = GetCell(matrix, x, y);
                }
            }
            return blue;
        }

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
