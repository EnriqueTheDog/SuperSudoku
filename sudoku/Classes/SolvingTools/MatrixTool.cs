using sudoku.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Classes.SolvingTools
{
    internal class MatrixTool : ISuperTool
    {
        private int[,,] Matrix { get; set; }
        private int RowLength { get; set; }
        private int QuadSize { get; set; }

        public MatrixTool(int[,] matrix)
        {
            Matrix = Transpose(matrix);
            RowLength = Convert.ToInt32(Math.Cbrt(Matrix.Length));
            QuadSize = Convert.ToInt32(Math.Sqrt(RowLength));
        }

        #region public methods

        public int[,] SolveSudoku()
        {
            int cont = 0;
            int sec = Matrix.Length;
            do
            {
                for (int y = 0; y < RowLength; y++)
                {
                    for (int x = 0; x < RowLength; x++)
                    {
                        Matrix = SetCellByRow(Matrix, x, y);
                        Matrix = SetCellByCol(Matrix, x, y);
                        Matrix = SetCellByQuad(Matrix, x, y);
                    }
                    cont++;
                    Matrix = CheckRow(Matrix, y);
                    if (cont % QuadSize == 0)
                    {
                        for (int q = 0; q < QuadSize; q++)
                        {
                            Matrix = CheckQuad(Matrix, q * QuadSize, y);
                        }
                    }
                    if (y == RowLength - 1)
                    {
                        for (int i = 0; i < RowLength; i++)
                        {
                            Matrix = CheckCol(Matrix, i);
                        }
                    }
                }
                sec--;
            } while (!IsSolved() && sec != 0);

            return Transpose(Matrix);
        }

        public bool IsSolved()
        {
            for (int y = 0; y < RowLength; y++)
            {
                for (int x = 0; x < RowLength; x++)
                {
                    if (GetCell(Matrix, x, y) <= 0) return false;
                }
            }
            return true;
        }

        #endregion

        #region private methods

        //Setting by row: Set the current cell possible numbers according to the rest of the row.
        //Returns the matrix with the cell rewritten (There's not a better way to return this??)
        private int[,,] SetCellByRow(int[,,] mat, int x, int y)
        {
            for (int c = 0; c < Math.Cbrt(mat.Length); c++)
            {
                if (c != x) ChangeCell(ref mat, x, y, c, null);
            }
            return mat;
        }

        //Setting by column. Works like the row setting but with current column
        private int[,,] SetCellByCol(int[,,] mat, int x, int y)
        {
            for (int i = 0; i < Math.Cbrt(mat.Length); i++)
            {
                if (i != y) ChangeCell(ref mat, x, y, null, i);
            }
            return mat;
        }

        //Setting by quadrant
        private int[,,] SetCellByQuad(int[,,] mat, int x, int y)
        {
            int quadLength = LocateQuad(mat, -1);
            int quadX = LocateQuad(mat, x);
            int quadY = LocateQuad(mat, y);

            //x
            for (int c = quadX; c < quadX + quadLength; c++)
            {
                //y
                for (int i = quadY; i < quadY + quadLength; i++)
                {
                    if (i != y || c != x) ChangeCell(ref mat, x, y, c, i);
                }
            }
            return mat;

        }

        //Checking -- With the same logic as Set, Check looks for possibles that aren't repeated in a whole row, column or quadrant
        //This is done separatedly from Sets cuz while Sets go once per cell, Checks go only once for row, column or quadrant, and they affect every cell in them. It also must be placed after Sets

        //Checking row
        private int[,,] CheckRow(int[,,] mat, int y)
        {
            //Each possible number
            for (int n = 0; n < Math.Cbrt(mat.Length); n++)
            {
                int single = 0;
                int count = 0;

                //Each cell
                for (int c = 0; c < Math.Cbrt(mat.Length); c++)
                {
                    if (mat[c, y, n] == 1)
                    {
                        single = c;
                        count++;
                    }
                }
                if (count == 1) mat = SetNumber(mat, single, y, n);
            }
            return mat;
        }

        private int[,,] CheckCol(int[,,] mat, int x)
        {
            //Each possible number
            for (int n = 0; n < Math.Cbrt(mat.Length); n++)
            {
                int single = 0;
                int count = 0;

                //Each cell
                for (int c = 0; c < Math.Cbrt(mat.Length); c++)
                {
                    if (mat[x, c, n] == 1)
                    {
                        single = c;
                        count++;
                    }
                }
                if (count == 1) mat = SetNumber(mat, x, single, n);
            }
            return mat;
        }

        private int[,,] CheckQuad(int[,,] mat, int x, int y)
        {
            int quadLength = LocateQuad(mat, -1);
            int quadX = LocateQuad(mat, x);
            int quadY = LocateQuad(mat, y);

            //each possible number
            for (int n = 0; n < Math.Cbrt(mat.Length); n++)
            {
                int singleX = 0;
                int singleY = 0;
                int count = 0;
                //Each quadrant column
                for (int c = quadX; c < quadX + quadLength; c++)
                {
                    //Each quadrant row
                    for (int i = quadY; i < quadY + quadLength; i++)
                    {
                        if (mat[c, i, n] == 1)
                        {
                            singleX = c;
                            singleY = i;
                            count++;
                        }
                    }
                }
                if (count == 1) mat = SetNumber(mat, singleX, singleY, n);
            }
            return mat;
        }

        //Set all possibles of the given cell to 0 except from the given number
        //Remember z must be the matrix number ('true' number minus 1)
        private int[,,] SetNumber(int[,,] mat, int x, int y, int z)
        {
            for (int i = 0; i < Math.Cbrt(mat.Length); i++)
            {
                if (i != z) mat[x, y, i] = 0;
            }
            return mat;
        }

        //Locates the quadrant 0 x or y coordinates corresponding to the current cell x or y position
        //If given a negative number, returns the length of the Quadrant
        private int LocateQuad(int[,,] mat, int c)
        {
            //is there an easier way to do this??
            int quadLength = Convert.ToInt32(Math.Sqrt(Math.Cbrt(mat.Length)));
            if (c >= 0) return c / quadLength * quadLength;
            else return quadLength;
        }

        private void ChangeCell(ref int[,,] mat, int x, int y, int? c, int? i)
        {
            c = c.HasValue ? c : x;
            i = i.HasValue ? i : y;
            int checker = GetCell(mat, (int)c, (int)i);
            if (checker > 0) mat[x, y, checker - 1] = 0;
        }

        //Cell Checking --
        //If a cell has only one possible, returns that number
        //If not, returns 0
        //If it has 0 possibles (error) returns -1
        private int GetCell(int[,,] mat, int x, int y)
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
        private int[,,] Transpose(int[,] matrix)
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
        private int[,] Transpose(int[,,] matrix)
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

        #endregion
    }
}
