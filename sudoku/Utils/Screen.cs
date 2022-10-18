using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Utils
{
    public static class Screen
    {

        //Moves the cursor to the end of the board. Useful to print things
        public static void BoardDown(int max)
        {
            Console.SetCursorPosition(0, CToGrid(max, false));
        }

        //Translates matrix cursor position to screen positions
        public static int CToGrid(int cp, bool x)
        {
            cp = cp + cp + 1;
            return (x) ? cp * 2 : cp;
        }
        public static void PrintTitle()
        {
            Console.Clear();
            Console.WriteLine(" ________                          ________      _________     ______         ");
            Console.WriteLine(" __  ___/___  _______________________  ___/___  _______  /________  /_____  __");
            Console.WriteLine(" _____ \\_  / / /__  __ \\  _ \\_  ___/____ \\_  / / /  __  /_  __ \\_  //_/  / / /");
            Console.WriteLine(" ____/ // /_/ /__  /_/ /  __/  /   ____/ // /_/ // /_/ / / /_/ /  ,<  / /_/ / ");
            Console.WriteLine(" /____/ \\__,_/ _  .___/\\___//_/    /____/ \\__,_/ \\__,_/  \\____//_/|_| \\__,_/  ");
            Console.WriteLine("               /_/                                                            ");
        }
    }
}
