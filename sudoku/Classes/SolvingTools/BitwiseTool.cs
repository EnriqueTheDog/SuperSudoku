using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sudoku.Classes.SolvingTools
{
    internal class BitwiseTool : ISuperTool
    {
        private int[,] Sudoku { get; set; }
        private int[,] SudokuBits { get; set; }
        private int RowLength { get; set; }
        private Dictionary<int, int> BitValues { get; set; }
        private int MaxValue { get; set; }
        private int QuadSize { get; set; }

        public BitwiseTool(int[,] sudoku)
        {
            Sudoku = sudoku;
            RowLength = Convert.ToInt32(Math.Sqrt(Sudoku.Length));
            QuadSize = Convert.ToInt32(Math.Sqrt(RowLength));
            BitValues = GetBitValues();
            foreach (int val in BitValues.Values)
            {
                MaxValue += val;
            }
        }

        public bool IsSolved()
        {
            for (int y = 0; y < RowLength; y++)
            {
                for (int x = 0; x < RowLength; x++)
                {
                    if (GetCell(SudokuBits[x, y]) <= 0) return false;
                }
            }
            return true;
        }

        public int[,] SolveSudoku()
        {
            int cont = 0;
            int sec = Sudoku.Length;
            SudokuIntToBit();
            do
            {
                for (int y = 0; y < RowLength; y++)
                {
                    for (int x = 0; x < RowLength; x++)
                    {
                        SudokuBits = SetCellByRow(SudokuBits, x, y);
                        SudokuBits = SetCellByCol(SudokuBits, x, y);
                        SudokuBits = SetCellByQuad(SudokuBits, x, y);
                    }
                    cont++;
                    SudokuBits = CheckRow(SudokuBits, y);
                    if (cont % QuadSize == 0)
                    {
                        for (int q = 0; q < QuadSize; q++)
                        {
                            SudokuBits = CheckQuad(SudokuBits, q * QuadSize, y);
                        }
                    }
                    if (y == RowLength - 1)
                    {
                        for (int i = 0; i < RowLength; i++)
                        {
                            SudokuBits = CheckCol(SudokuBits, i);
                        }
                    }
                }
                sec--;
            } while (!IsSolved() && sec != 0);

            SudokuBitToInt();
            return Sudoku;
        }

        #region private methods

        private Dictionary<int, int> GetBitValues()
        {
            Dictionary<int, int>  bitValues = new Dictionary<int, int>();
            int v = 1;
            for (int k = 1; k <= RowLength; k++)
            {
                bitValues.Add(k, v);
                v *= 2;
            }

            return bitValues;
        }

        private void SudokuIntToBit()
        {
            SudokuBits = new int[RowLength, RowLength];
            for (int x = 0; x < RowLength; x++)
            {
                for (int y = 0; y < RowLength; y++)
                {
                    if (Sudoku[x, y] != 0) SudokuBits[x, y] = BitValues[Sudoku[x, y]];
                    else SudokuBits[x, y] = MaxValue;
                }
            }
        }

        private void SudokuBitToInt()
        {
            for (int x = 0; x < RowLength; x++)
            {
                for (int y = 0; y < RowLength; y++)
                {
                    Sudoku[x, y] = GetCell(SudokuBits[x, y]);
                }
            }
        }

        //Setting by row: Set the current cell possible numbers according to the rest of the row.
        //Returns the matrix with the cell rewritten (There's not a better way to return this??)
        private int[,] SetCellByRow(int[,] mat, int x, int y)
        {
            for (int c = 0; c < RowLength; c++)
            {
                if (c != x) ChangeCell(ref mat, x, y, c);
            }
            return mat;
        }

        private int[,] SetCellByCol(int[,] mat, int x, int y)
        {
            for (int i = 0; i < RowLength; i++)
            {
                if (i != y) ChangeCell(ref mat, x, y, null, i);
            }
            return mat;
        }
        private int[,] SetCellByQuad(int[,] mat, int x, int y)
        {
            int quadX = x / QuadSize * QuadSize;
            int quadY = y / QuadSize * QuadSize;

            for (int c = quadX; c < quadX + QuadSize; c++)
            {
                for (int i = quadY; i < quadY + QuadSize; i++)
                {
                    if (i != y || c != x) ChangeCell(ref mat, x, y, c, i);
                }
            }
            return mat;
        }

        // Can we get any better than this O(n^2) ? Rethink
        private int[,] CheckRow(int[,] mat, int y)
        {
            for (int n = 0; n < RowLength; n++)
            {
                int possibleCellsForN = 0;
                for (int c = 0; c < RowLength; c++)
                {
                    if (BitValues.Values.Contains(mat[c, y] & BitValues[n + 1])) possibleCellsForN |= BitValues[c + 1];
                }
                int result = GetCell(possibleCellsForN) - 1;
                if (result >= 0) mat[result, y] = BitValues[n + 1];
            }
            return mat;
        }

        private int[,] CheckCol(int[,] mat, int x)
        {
            for (int n = 0; n < RowLength; n++)
            {
                int possibleCellsForN = 0;
                for (int i = 0; i < RowLength; i++)
                {
                    if (BitValues.Values.Contains(mat[x, i] & BitValues[n + 1])) possibleCellsForN |= BitValues[i + 1];
                }
                int result = GetCell(possibleCellsForN) - 1;
                if (result >= 0) mat[x, result] = BitValues[n + 1];
            }
            return mat;
        }

        private int[,] CheckQuad(int[,] mat, int x, int y)
        {
            int quadX = x / QuadSize * QuadSize;
            int quadY = y / QuadSize * QuadSize;

            for (int n = 1; n <= RowLength; n++)
            {
                int singleX = 0;
                int singleY = 0;
                int count = 0;
                for (int c = quadX; c < quadX + QuadSize; c++)
                {
                    for (int i = quadY; i < quadY + QuadSize; i++)
                    {
                        if (BitValues.Values.Contains(mat[c, i] & BitValues[n])) // This could be != 0
                        {
                            singleX = c;
                            singleY = i;
                            count++;
                        }
                        if (count > 1) break;
                    }
                    if (count > 1) break;
                }
                if (count == 1) mat[singleX, singleY] = BitValues[n];
            }
            return mat;
        }

        private void ChangeCell(ref int[,] mat, int x, int y, int? c = null, int? i = null)
        {
            c = c.HasValue ? c : x;
            i = i.HasValue ? i : y;
            int targetVal = mat[(int)c, (int)i];
            if (BitValues.Values.Contains(targetVal) && BitValues.Values.Contains(mat[x, y] & targetVal))
                mat[x, y] ^= targetVal;
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
