using System;
using System.Collections.Generic;
using System.Text;
using sudoku.Utils;

namespace sudoku.Classes
{
    public class Grid
    {
        public int[,] Matrix { get; set; }
        private int Size { get; set; }
        private int Max { get; set; }
        public bool Fancy { get; set; }

        public Grid(int size, bool fancy)
        {
            Size = size;
            Max = size * size;
            Fancy = fancy;
            Matrix = new int[Max, Max];
        }

        #region public methods

        public void Print()
        {
            PrintEmpty();
            WriteGrid();
            Screen.BoardDown(Max);
        }

        public void ReadInput()
        {
            PrintEmpty();

            int x = 0;
            int y = 0;

            bool stop = false;
            ConsoleKeyInfo key;

            Console.WriteLine("Give me a Sudoku and I'll solve it\n\nArrow keys to move\nNumber keys to write\nBackspace to erase\nEnter to solve");
            if (Settings.devMode) Console.WriteLine("'d' to load default sudoku");

            while (!stop)
            {
                Console.SetCursorPosition(Screen.CToGrid(x, true), Screen.CToGrid(y, false));

                key = Console.ReadKey(true);

                switch (Convert.ToString(key.Key))
                {
                    case "UpArrow":
                        y = Move(false, y, Max);
                        break;
                    case "DownArrow":
                        y = Move(true, y, Max);
                        break;
                    case "LeftArrow":
                        x = Move(false, x, Max);
                        break;
                    case "RightArrow":
                        x = Move(true, x, Max);
                        break;
                    case "Enter":
                        stop = true;
                        break;
                    case "Backspace":
                        Console.Write("  ");
                        Matrix[x, y] = 0;
                        break;
                    case "D":
                        if (Settings.devMode && Max == 9)
                        {
                            Matrix = MatrixHelper.GetSampleSudoku();
                            WriteGrid();
                        }
                        break;
                    default:
                        if (char.IsNumber(Convert.ToString(key.Key)[1]))
                        {
                            int num = Convert.ToString(key.Key)[1] - 48;
                            if (Matrix[x, y] == 0)
                            {
                                if (num <= 0)
                                {
                                    Console.WriteLine("  ");
                                    Matrix[x, y] = 0;
                                }
                                else
                                {
                                    Console.Write(num);
                                    Matrix[x, y] = num;
                                }
                            }
                            else
                            {
                                int val = Convert.ToInt32(Convert.ToString(Matrix[x, y]) + num);
                                if (val <= Max)
                                {
                                    Console.Write(val);
                                    Matrix[x, y] = val;
                                }
                            }
                        }
                        break;
                }
            }
        }

        #endregion


        #region private methods

        //Given n, prints a n^2 grid. With 'fancy' true also prints double lines in borders
        private void PrintEmpty()
        {
            int height = Max * 2 + 1;
            int width = Max + 1;
            string line = "   ";
            Console.Clear();
            for (int row = 1; row <= height; row++)
            {
                for (int col = 1; col <= width; col++)
                {
                    if (row % 2 != 0)
                    {
                        if (row == 1)
                        {
                            if (col == 1) line = Fancy ? "╔" : "┌";
                            else if (col == width) line = Fancy ? "╗" : "┐";
                            else line = Fancy ? "╦" : "┬";
                        }
                        else if (row == height)
                        {
                            if (col == 1) line = Fancy ? "╚" : "└";
                            else if (col == width) line = Fancy ? "╝" : "┘";
                            else line = Fancy ? "╩" : "┴";
                        }
                        else
                        {
                            if (col == 1) line = Fancy ? "╠" : "├";
                            else if (col == width) line = Fancy ? "╣" : "┤";
                            else line = ((row - 1) / 2 % Size == 0 && Fancy || (col - 1) % Size == 0 && Fancy) ? "╬" : "┼";
                        }
                        if (col != width) line += ((row - 1) / 2 % Size == 0 && Fancy) ? "═══" : "───";
                    }
                    else
                    {
                        line = ((col - 1) % Size == 0 && Fancy) ? "║" : "│";
                        line += (col != width) ? "   " : "";
                    }
                    Console.Write(line);
                }
                Console.WriteLine();
            }
        }

        private void WriteGrid()
        {
            for (int x = 0; x < Max; x++)
            {
                for (int y = 0; y < Max; y++)
                {
                    Console.SetCursorPosition(Screen.CToGrid(x, true), Screen.CToGrid(y, false));
                    if (Matrix[x, y] == -1 || Matrix[x, y] == 0) { Console.Write("?"); }
                    else { Console.Write(Matrix[x, y]); }
                }
            }
        }

        static int Move(bool add, int c, int max)
        {
            int n = (add) ? c + 1 : c - 1;
            return (n < 0 || n >= max) ? c : n;
        }

        #endregion

    }
}
