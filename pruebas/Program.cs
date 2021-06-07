using System;

namespace pruebas
{
    class Program
    {
        static void Main(string[] args)
        {

            while (1 < 1000000) { 
            ConsoleKeyInfo key = Console.ReadKey(true);

            Console.WriteLine(Convert.ToString(key.Key));
            }


        }


        //THIS IS A SAFETY COPY OF THE FIRST UNFINISHED VERSION OF SUPERSUDOKUU
        //static void Main(string[] args)
        //{
        //    bool stop = false;

        //    //Cursor positions
        //    //"Real" position
        //    int cpx = 0;
        //    int cpy = 0;
        //    //Grid Positions
        //    int cpxg = CToGrid(cpx, true);
        //    int cpyg = CToGrid(cpy, false);
        //    //Refers to the maximum position in the sudoku (the max number. Standard: 9)
        //    int max = 0;

        //    int inputNum = 0;

        //    //Matrix
        //    //Blueprint Matrix - Receives inputs and then send them to the main matrix
        //    int[,] blue = new int[1, 1];
        //    //Main matrix
        //    int[,] matrix = new int[1, 1];

        //    Console.TreatControlCAsInput = true;
        //    ConsoleKeyInfo key;

        //    int size = Convert.ToInt32(Console.ReadLine());
        //    max = PrintGrid(size, true);
        //    //Now let's create sized matrixes
        //    blue = new int[max, max];
        //    matrix = new int[max, max];

        //    Console.SetCursorPosition(cpxg, cpyg);

        //    while (!stop)
        //    {
        //        inputNum = 0;


        //        key = Console.ReadKey(true);

        //        switch (Convert.ToString(key.Key))
        //        {
        //            case "UpArrow":
        //                cpy = Move(false, cpy, max);
        //                break;
        //            case "DownArrow":
        //                cpy = Move(true, cpy, max);
        //                break;
        //            case "LeftArrow":
        //                cpx = Move(false, cpx, max);
        //                break;
        //            case "RightArrow":
        //                cpx = Move(true, cpx, max);
        //                break;
        //            case "Enter":
        //                stop = true;
        //                break;
        //            default:
        //                if (Convert.ToChar(key.Key) > 0)
        //                {
        //                    Console.Write(Convert.ToChar(key.Key));
        //                    blue[cpx, cpy] = Convert.ToInt32(key.Key);
        //                    //cpx = Move(true, cpx, max);
        //                }
        //                break;
        //        }
        //        cpyg = CToGrid(cpy, false);
        //        cpxg = CToGrid(cpx, true);
        //        Console.SetCursorPosition(cpxg, cpyg);





        //    }

        //    matrix = MatrixCheck(blue, max);
        //    PrintSolution(matrix);

        //}

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

        static int[,] MatrixCheck(int[,] mat, int max)
        {
            //Takes the blueprint matrix and returns it resolved
            //If irresoluble, returns the matrix as it was
            //We'll may need to duplicate the function in order to return false if the sudoku is irresoluble ???

            int[,] check = mat;
            int[,] bloo = mat;
            bool goin = true;
            bool hasSolution = true;

            //This guy will loop it til no cells are soluble
            while (goin)
            {
                //y - columns
                for (int y = 0; y < max; y++)
                {
                    //x - rows
                    for (int x = 0; x < max; x++)
                    {
                        //Set the cursor in the current position of the board <------
                        //IS THIS NECESSARY??? NO.
                        Console.SetCursorPosition(CToGrid(x, true), CToGrid(y, false));
                        //Check if the current position is empty (0). If not, jumps to the next
                        if (check[x, y] == 0)
                        {
                            //Function that creates an array with numbers from 1 to max
                            //We're going to quit numbers one by one until we end up with only one number
                            int[] may = Maybes(max);

                            //let's start checking number by number
                            //numbers in this row (we don't need to exclude current number cuz it's 0)
                            for (int col = 0; col < max; col++)
                            {
                                may = RemoveFromArr(may, check[x, col]);
                            }
                            //numbers in this column
                            for (int row = 0; row < max; row++)
                            {
                                may = RemoveFromArr(may, check[row, y]);
                            }
                            //AND numbers in this 1/max square
                            //For this one we need another two fors in order to create a 'sub-matrix'

                            //We need to acquire the current 'minisquare'
                            //(for x the 0 point is ---> x / srqt of max * sqrt of max :S
                            int miniX = x / Convert.ToInt32(Math.Sqrt(max));
                            int miniY = y / Convert.ToInt32(Math.Sqrt(max));
                            if (miniX != 0) { miniX = miniX * Convert.ToInt32(Math.Sqrt(max)); }
                            if (miniY != 0) { miniY = miniY * Convert.ToInt32(Math.Sqrt(max)); }

                            //In a standard sudoku, only possible results are 0, 1 or 2

                            for (int miniRow = miniY; miniRow < (miniY + (Convert.ToInt32(Math.Sqrt(max)) - 1)); miniRow++)
                            {
                                for (int miniCol = miniX; miniCol < (miniX + (Convert.ToInt32(Math.Sqrt(max)) - 1)); miniCol++)
                                {
                                    //At this point, we are at 0,0 of the mini matrix
                                    //And this should loop it till 2,2
                                    may = RemoveFromArr(may, check[miniRow, miniCol]);
                                }
                            }

                            //And now we've checked the current position x, y
                            //Before we jump to the next position, we check if we have a valid number for this one and apply
                            check[x, y] = CheckMaybe(may);
                        }
                    }


                }

                //Now we check if we've solved something. If not, we abort
                //Wait a minute. We're first going to check if there's any 0's. If not, it's solved!
                if (IsSolved(check))
                {
                    bloo = check;
                    return bloo;
                }
                else if (bloo == check)
                {
                    hasSolution = false;
                    return mat;
                }

            }
            return mat;

        }

        static int[] Maybes(int max)
        {
            int[] maybes = new int[max - 1];
            for (int i = 0; i < (max - 1); i++)
            {
                maybes[i] = i + 1;
            }
            return maybes;
        }

        static bool IsSolved(int[,] mat)
        {
            bool solved = true;
            for (int x = 0; x < Math.Sqrt(mat.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(mat.Length); y++)
                {
                    if (mat[x, y] == 0) { solved = false; break; }
                }
            }

            return solved;
        }

        static int[] RemoveFromArr(int[] arr, int n)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == n)
                {
                    arr[i] = 0;
                }
            }
            return arr;
        }

        //This function checks if there's only one num in the 'maybes' array
        //if there were more than one option, returns 0
        static int CheckMaybe(int[] maybe)
        {
            int count = 0;
            int res = 0;

            for (int i = 0; i < maybe.Length; i++)
            {
                if (maybe[i] != 0)
                {
                    count++;
                    res = maybe[i];
                }

                if (count > 1)
                {
                    break;
                }
            }

            if (count <= 1) { return res; }
            else { return 0; }
        }

        /*static string Arrow(ConsoleKeyInfo k)
        {
            switch (Convert.ToString(k.Key)) {
                case "UpArrow":
                    return "x";
                    break;
            }
            


        }*/
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

        //This guy prints a given matrix into the board
        static void PrintSolution(int[,] mat)
        {
            for (int x = 0; x < Math.Sqrt(mat.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(mat.Length); y++)
                {
                    //Let's put the cursor here
                    Console.SetCursorPosition(CToGrid(x, true), CToGrid(y, false));
                    //Now let's print
                    Console.Write(mat[x, y]);
                }
            }
        }

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
    }
}
