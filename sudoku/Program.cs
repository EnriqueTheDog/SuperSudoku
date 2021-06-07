using System;

namespace sudoku
{
    class Program
    {

        static void Main(string[] args)
        {
            bool stop = false;
            do
            {
                PrintTitle();
                Console.WriteLine();
                Console.Write("Select a sudoku level (standard = 3) ");
                int max = PrintGrid(Convert.ToInt32(Console.ReadLine()), true);

                //Create the input matrix based on user's input
                int[,] input = ReadSudoku(max);

                //Now we need to translate the 2d input matrix into the 3d blueprint matrix
                int[,,] blueprint = Transpose(input);

                //Once we have the transposed matrix, we can solve it.
                int[,,] solved = SolveSudoku(blueprint);

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            } while (!stop);

        }

        static int[,,] SolveSudoku(int[,,] mat)
        {
            int cont = 0;
            int rowLength = Convert.ToInt32(Math.Cbrt(mat.Length));
            //What is the maximum number we could need to solve it?
            int sec = mat.Length;
            do
            {
                for (int y = 0; y < rowLength; y++)
                {
                    for (int x = 0; x < rowLength; x++)
                    {
                        mat = SetCellByRow(mat, x, y);
                        mat = SetCellByCol(mat, x, y);
                        mat = SetCellByQuad(mat, x, y);
                    }
                    cont++;
                    mat = CheckRow(mat, y);
                    if (cont % Convert.ToInt32(Math.Sqrt(rowLength)) == 0)
                    {
                        for (int q = 0; q < Math.Sqrt(rowLength); q++)
                        {
                            mat = CheckQuad(mat, q * Convert.ToInt32(Math.Sqrt(rowLength)), y);
                        }
                    }
                    if (y == rowLength - 1)
                    {
                        for (int i = 0; i < rowLength; i++)
                        {
                            mat = CheckCol(mat, i);
                        }
                    }
                }
                sec--;
            } while (!IsSolved(mat) && sec != 0);

            int[,] printable = ReadSudoku(mat);
            Console.Clear();
            PrintSudoku(printable);
            BoardDown(rowLength);
            if (IsSolved(mat)) { Console.WriteLine("Sudoku completed!"); }
            else { Console.WriteLine("This sudoku is impossible or has multiple solutions"); }

            return mat;
        }

        //Translate matrix cursor position to sreen positions
        static int CToGrid(int cp, bool x)
        {
            cp = cp + cp + 1;
            if (x)
            {
                return cp * 2;
            }
            else
            {
                return cp;
            }
        }

        //Moves the cursor to the end of the board. Useful to print things
        static void BoardDown(int max)
        {
            Console.SetCursorPosition(0, CToGrid(max, false));
        }

        //This one helps you to jump between cells. Goes with ReadSudoku
        static int Move(bool add, int c, int max)
        {
            int n = c;
            if (add)
            {
                n++;
            }
            else
            {
                n--;
            }

            if (n < 0 || n >= max)
            {
                return c;
            }
            else
            {
                return n;
            }
        }

        //This guy should be able to read all the inputs and return them in a 2d matrix when 'Enter' is pressed
        static int[,] ReadSudoku(int max)
        {
            //Matrix positions
            int x = 0;
            int y = 0;

            int[,] blue = new int[max, max];

            bool stop = false;
            ConsoleKeyInfo key;

            Console.WriteLine("Give me a Sudoku and I'll solve it\n\nArrow keys to move\nNumber keys to write\nBackspace to erase\nEnter to solve");

            while (!stop)
            {
                Console.SetCursorPosition(CToGrid(x, true), CToGrid(y, false));

                key = Console.ReadKey(true);

                switch (Convert.ToString(key.Key))
                {
                    case "UpArrow":
                        y = Move(false, y, max);
                        break;
                    case "DownArrow":
                        y = Move(true, y, max);
                        break;
                    case "LeftArrow":
                        x = Move(false, x, max);
                        break;
                    case "RightArrow":
                        x = Move(true, x, max);
                        break;
                    case "Enter":
                        stop = true;
                        break;
                    case "Backspace":
                        Console.Write("  ");
                        blue[x, y] = 0;
                        break;
                    default:
                        if (Char.IsNumber(Convert.ToString(key.Key)[1]))
                        {
                            int num = Convert.ToString(key.Key)[1] - 48;
                            if (blue[x, y] == 0)
                            {
                                if (num <= 0)
                                {
                                    Console.WriteLine("  ");
                                    blue[x, y] = 0;
                                }
                                else
                                {
                                    Console.Write(num);
                                    blue[x, y] = num;
                                }

                            }
                            else
                            {
                                int val = Convert.ToInt32(Convert.ToString(blue[x, y]) + num);
                                if (val <= max)
                                {
                                    Console.Write(val);
                                    blue[x, y] = val;
                                }

                            }
                        }
                        break;
                }
            }
            return blue;
        }

        //This one re-translates 3d info to a 2d matrix
        static int[,] ReadSudoku(int[,,] mat)
        {
            int[,] blue = new int[Convert.ToInt32(Math.Cbrt(mat.Length)), Convert.ToInt32(Math.Cbrt(mat.Length))];
            for (int x = 0; x < Math.Cbrt(mat.Length); x++)
            {
                for (int y = 0; y < Math.Cbrt(mat.Length); y++)
                {
                    blue[x, y] = GetCell(mat, x, y);
                }
            }
            return blue;
        }

        //Given a number, returns te square of that number and creates a grid that size. The 'fancy' option also prints double lines in borders
        static int PrintGrid(int n, bool fancy)
        {
            int height = n * n * 2 + 1;
            int width = n * n + 1;
            Console.Clear();
            /*if (n > 5)
            {
                Console.WriteLine("Tampoco te pases.");
                return 0;
            }*/
            // ╣ ║ ╗ ╝ ╚ ╔ ╩ ╦ ╠ ═ ╬
            // │ ─ ┼ ┬ ┐ └ 
            for (int f = 1; f <= height; f++)
            {
                for (int c = 1; c <= width; c++)
                {
                    if (f % 2 != 0)
                    {
                        if (f == 1)
                        {
                            if (c == 1)
                            {
                                if (fancy) { Console.Write("╔"); }
                                else { Console.Write("┌"); }

                            }
                            else if (c == width)
                            {
                                if (fancy) { Console.Write("╗"); }
                                else { Console.Write("┐"); }

                            }
                            else
                            {
                                if (fancy)
                                {
                                    Console.Write("╦");
                                }
                                else
                                {
                                    Console.Write("┬");
                                }

                            }
                        }
                        else if (f == height)
                        {
                            if (c == 1)
                            {
                                if (fancy)
                                {
                                    Console.Write("╚");
                                }
                                else
                                {
                                    Console.Write("└");
                                }

                            }
                            else if (c == width)
                            {
                                if (fancy)
                                {
                                    Console.Write("╝");
                                }
                                else
                                {
                                    Console.Write("┘");
                                }
                            }
                            else
                            {
                                if (fancy)
                                {
                                    Console.Write("╩");
                                }
                                else
                                {
                                    Console.Write("┴");
                                }

                            }
                        }
                        else
                        {
                            if (c == 1)
                            {
                                if (fancy)
                                {
                                    Console.Write("╠");
                                }
                                else
                                {
                                    Console.Write("├");
                                }

                            }
                            else if (c == width)
                            {
                                if (fancy)
                                {
                                    Console.Write("╣");
                                }
                                else
                                {
                                    Console.Write("┤");
                                }
                            }
                            else
                            {
                                if (((f - 1) / 2) % n == 0 && fancy || (c - 1) % n == 0 && fancy)
                                {

                                    Console.Write("╬");


                                }
                                else
                                {
                                    Console.Write("┼");

                                }
                            }
                        }
                        // & one more
                        if (c != width)
                        {
                            if (((f - 1) / 2) % n == 0 && fancy)
                            {
                                Console.Write("═══");

                            }
                            else
                            {
                                Console.Write("───");

                            }
                        }
                    }
                    else
                    {
                        if ((c - 1) % n == 0 && fancy)
                        {
                            Console.Write("║");

                        }
                        else
                        {
                            Console.Write("│");

                        }
                        if (c != width)
                        {
                            Console.Write("   ");
                        }
                    }


                }
                Console.WriteLine();

            }
            return n * n;
        }

        //Used to display all values of a matrix visually. Only for debbuging
        static void PrintMatrix(int[,] mat)
        {
            for (int i = 0; i < Math.Sqrt(mat.Length); i++)
            {
                for (int x = 0; x < Math.Sqrt(mat.Length); x++)
                {
                    Console.Write(mat[x, i] + "\t");
                }
                Console.WriteLine();
            }
        }
        static void PrintMatrix(int[,,] mat)
        {
            //WARNING---->When working with 3d matrix, we use cubiq root (Cbrt) instead of square root to find the length of the row
            for (int i = 0; i < Math.Cbrt(mat.Length); i++)
            {
                for (int x = 0; x < Math.Cbrt(mat.Length); x++)
                {
                    Console.Write($"{x},{i}: ");
                    for (int z = 0; z < Math.Cbrt(mat.Length); z++)
                    {
                        Console.Write($"{mat[x, i, z]} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        //Prints a 2d matrix into a new Grid
        static void PrintSudoku(int[,] mat)
        {
            PrintGrid(Convert.ToInt32(Math.Sqrt(Math.Sqrt(mat.Length))), true);

            for (int x = 0; x < Math.Sqrt(mat.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(mat.Length); y++)
                {
                    Console.SetCursorPosition(CToGrid(x, true), CToGrid(y, false));
                    if (mat[x, y] == -1 || mat[x, y] == 0) { Console.Write("?"); }
                    else { Console.Write(mat[x, y]); }
                }

            }

        }

        //Translates a 2d matrix to a 3d matrix
        // (1,4 = 7) = (1,4,6 = 1)
        // (1,4 = 0) = (1,4,all = 1)
        static int[,,] Transpose(int[,] mat)
        {
            int[,,] blue = new int[Convert.ToInt32(Math.Sqrt(mat.Length)), Convert.ToInt32(Math.Sqrt(mat.Length)), Convert.ToInt32(Math.Sqrt(mat.Length))];
            for (int y = 0; y < Math.Sqrt(mat.Length); y++)
            {
                for (int x = 0; x < Math.Sqrt(mat.Length); x++)
                {
                    //WARNING----> Each position stores the number it's labelled with minus 1
                    if (mat[x, y] != 0)
                    {
                        blue[x, y, (mat[x, y] - 1)] = 1;
                    }
                    //This clausule 
                    else
                    {
                        for (int z = 0; z < Math.Sqrt(mat.Length); z++)
                        {
                            blue[x, y, z] = 1;
                        }
                    }
                }
            }
            return blue;
        }


        //SOLVING METHODS -- They need a matrix and the 2d coordinates of the cell they're supposed to check

        //Setting by row: Set the current cell possible numbers according to the rest of the row. Returns the matrix with the cell rewritten (There's not a better way to return this??)
        static int[,,] SetCellByRow(int[,,] mat, int x, int y)
        {
            int[,,] blue = mat;
            //Cell level (x)
            for (int c = 0; c < Math.Cbrt(mat.Length); c++)
            {
                //IMPORTANT: Prevent the current cell to be checked. This would result in a collapse of the whole sudoku
                if (c != x)
                {
                    int checker = GetCell(mat, c, y);

                    if (checker > 0)
                    {
                        blue[x, y, checker - 1] = 0;
                    }
                }
            }
            return blue;
        }

        //Setting by column. Works like the row setting but with current column
        static int[,,] SetCellByCol(int[,,] mat, int x, int y)
        {
            int[,,] blue = mat;
            //Column level (y)
            for (int i = 0; i < Math.Cbrt(mat.Length); i++)
            {
                //IMPORTANT: Prevent the current cell to be checked. This would result in a collapse of the whole sudoku
                if (i != y)
                {
                    int checker = GetCell(mat, x, i);

                    if (checker > 0)
                    {
                        blue[x, y, checker - 1] = 0;
                    }
                }
            }
            return blue;
        }

        //Setting by quadrant
        static int[,,] SetCellByQuad(int[,,] mat, int x, int y)
        {
            int[,,] blue = mat;
            int quadLength = LocateQuad(mat, -1);
            int quadX = LocateQuad(mat, x);
            int quadY = LocateQuad(mat, y);

            //let's check from here the whole quadrant

            //x
            for (int c = quadX; c < quadX + quadLength; c++)
            {
                //y
                for (int i = quadY; i < quadY + quadLength; i++)
                {
                    //Prevents the current cell to be checked
                    if (i != y || c != x)
                    {
                        int checker = GetCell(mat, c, i);
                        //Console.WriteLine($"{c},{i}: {checker}");

                        if (checker > 0)
                        {
                            blue[x, y, checker - 1] = 0;
                        }
                    }
                }
            }

            return blue;

        }

        //Checking -- With the same logic as Set, Check looks for possibles that aren't repeated in a whole row, column or quadrant
        //This is done separatedly from Sets cuz while Sets go once per cell, Checks go only once for row, column or quadrant, and affect every cell in them. It also must be placed after Sets

        //Checking row
        static int[,,] CheckRow(int[,,] mat, int y)
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
                if (count == 1)
                {
                    mat = SetNumber(mat, single, y, n);
                }
            }
            return mat;
        }

        static int[,,] CheckCol(int[,,] mat, int x)
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
                if (count == 1)
                {
                    mat = SetNumber(mat, x, single, n);
                }
            }
            return mat;
        }

        static int[,,] CheckQuad(int[,,] mat, int x, int y)
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
                if (count == 1)
                {
                    mat = SetNumber(mat, singleX, singleY, n);
                }

            }
            return mat;
        }

        //Cell Checking --
        //If a cell has only one possible, returns that number
        //If not, returns 0
        //If it has 0 possibles (error) returns -1
        static int GetCell(int[,,] mat, int x, int y)
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
            if (count == 0)
            {
                return -1;
            }
            else if (count == 1)
            {
                return result + 1;
            }
            else
            {
                return 0;
            }
        }

        //Set all possibles of the given cell to 0 except from the given number
        //Remember z must be the matrix number ('true' number minus 1)
        static int[,,] SetNumber(int[,,] mat, int x, int y, int z)
        {
            for (int i = 0; i < Math.Cbrt(mat.Length); i++)
            {
                if (i != z)
                {
                    mat[x, y, i] = 0;
                }
            }
            return mat;
        }

        //Locates the quadrant 0 x or y coordinates corresponding to the current cell x or y position
        //If given a negative number, returns the length of the Quadrant
        static int LocateQuad(int[,,] mat, int c)
        {
            //is there an easier way to do this??
            int quadLength = Convert.ToInt32(Math.Sqrt(Math.Cbrt(mat.Length)));
            if (c >= 0)
            {
                return c / quadLength * quadLength;
            }
            else
            {
                return quadLength;
            }
        }

        //Returns true if the given Sudoku is solved (if every cell has a number)
        static bool IsSolved(int[,,] mat)
        {
            for (int y = 0; y < Math.Cbrt(mat.Length); y++)
            {
                for (int x = 0; x < Math.Cbrt(mat.Length); x++)
                {
                    if (GetCell(mat, x, y) <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void PrintTitle()
        {
            Console.WriteLine(" ________                          ________      _________     ______         ");
            Console.WriteLine(" __  ___/___  _______________________  ___/___  _______  /________  /_____  __");
            Console.WriteLine(" _____ \\_  / / /__  __ \\  _ \\_  ___/____ \\_  / / /  __  /_  __ \\_  //_/  / / /");
            Console.WriteLine(" ____/ // /_/ /__  /_/ /  __/  /   ____/ // /_/ // /_/ / / /_/ /  ,<  / /_/ / ");
            Console.WriteLine(" /____/ \\__,_/ _  .___/\\___//_/    /____/ \\__,_/ \\__,_/  \\____//_/|_| \\__,_/  ");
            Console.WriteLine("               /_/                                                            ");
        }
    }



}
