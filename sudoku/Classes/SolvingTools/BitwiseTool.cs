using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sudoku.Classes.SolvingTools
{
    internal class BitwiseTool : ISuperTool
    {
        private int[,] Sudoku { get; set; }
        private int[,] Solution { get; set; }
        private int RowLength { get; set; }
        private Dictionary<int, int> BitValues { get; set; }
        private int MaxValue { get; set; }

        public BitwiseTool(int[,] sudoku)
        {
            Sudoku = sudoku;
            RowLength = Convert.ToInt32(Math.Sqrt(Sudoku.Length));
        }

        public bool IsSolved()
        {
            throw new NotImplementedException();
        }

        public int[,] SolveSudoku()
        {
            int cont = 0;
            int sec = Sudoku.Length;
            do
            {
                for (int y = 0; y < RowLength; y++)
                {
                    for (int x = 0; x < RowLength; x++)
                    {
                        Solution = SetCellByRow(Solution, x, y);
                        Solution = SetCellByCol(Solution, x, y);
                        Solution = SetCellByQuad(Solution, x, y);
                    }
                    cont++;
                    Solution = CheckRow(Solution, y);
                    if (cont % QuadSize == 0)
                    {
                        for (int q = 0; q < QuadSize; q++)
                        {
                            Solution = CheckQuad(Solution, q * QuadSize, y);
                        }
                    }
                    if (y == RowLength - 1)
                    {
                        for (int i = 0; i < RowLength; i++)
                        {
                            Solution = CheckCol(Solution, i);
                        }
                    }
                }
                sec--;
            } while (!IsSolved() && sec != 0);

            return Transpose(Solution);
        }

        #region private methods

        private void SetBitValues()
        {
            BitValues = new Dictionary<int, int>();
            int v = 1;
            for (int k = 1; k <= RowLength; k++)
            {
                BitValues.Add(k, v);
                v *= 2;
            }

            foreach (int val in BitValues.Values)
            {
                MaxValue += val;
            }
        }

        // Transforms the numeric values array into a "bitwise" array
        private void Transpose()
        {
            for (int x = 0; x < RowLength; x++)
            {
                for (int y = 0; y < RowLength; y++)
                {
                    if (Sudoku[x, y] != 0) Solution[x, y] = BitValues[Sudoku[x, y]];
                    else Solution[x, y] = MaxValue;
                }
            }
        }

        //Setting by row: Set the current cell possible numbers according to the rest of the row.
        //Returns the matrix with the cell rewritten (There's not a better way to return this??)
        private int[,] SetCellByRow(int[,] mat, int x, int y)
        {
            for (int c = 0; c < RowLength; c++)
            {
                if (c != x) ChangeCell(ref mat, x, y, c, null);
            }
            return mat;
        }

        // TODO!
        private void ChangeCell(ref int[,] mat, int x, int y, int? c, int? i)
        {
            c = c.HasValue ? c : x;
            i = i.HasValue ? i : y;
            //int checker = GetCell(mat, (int)c, (int)i);
            //if (checker > 0) mat[x, y, checker - 1] = 0;
        }

        //Cell Checking --
        //If a cell has only one possible, returns that number
        //If not, returns 0
        //If it has 0 possibles (error) returns -1
        private int GetCell(int bitValue)
        {
            if (bitValue == 0) return -1;
            if (BitValues.Values.Contains(bitValue))
                return BitValues.FirstOrDefault(x => x.Value == bitValue).Key;
            else return 0;
        }

        #endregion
    }
}
