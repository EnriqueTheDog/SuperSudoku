using sudoku.Classes.SolvingTools;
using sudoku.Enums;
using sudoku.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using static sudoku.Enums.ToolEnum;

namespace sudoku.Classes
{
    internal class Solver
    {
        ISuperTool Tool { get; set; }
        Grid Grid { get; set; }
        Stopwatch Watch { get; set; }
        List<SolutionData> SolutionDataLog { get; set; }

        public Solver ()
        {
            Watch = new Stopwatch();
            SolutionDataLog = new List<SolutionData>();
        }

        #region start function

        public void Start()
        {
            ShowMenu();
        }

        #endregion

        #region private functions

        private void ShowMenu()
        {
            Screen.PrintTitle();
            Console.WriteLine("1. Solve Sudoku");
            Console.WriteLine("2. Settings");
            Console.WriteLine("3. Exit");
            if (Settings.devMode)
            {
                Console.WriteLine("4. Solution Log");
                Console.WriteLine("5. Erase Log");
            } 

            switch (Console.ReadLine())
            {
                case "1":
                    Solve();
                    break;
                case "2":
                    ConfigureSettings();
                    break;
                case "3":
                    Console.WriteLine("\nPress any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                case "4":
                    if (Settings.devMode) ShowSolutionLog();
                    else Console.WriteLine("Please press a valid key");
                    break;
                case "5":
                    if (Settings.devMode)
                    {
                        SolutionDataLog.Clear();
                        Console.WriteLine("Solution log cleared");
                    }
                    else Console.WriteLine("Please press a valid key");
                    break;
                default:
                    Console.WriteLine("Please press a valid key");
                    break;
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Start();            
        }

        private void Solve()
        {
            int lvl = ReadNum("\n Select a sudoku level \n Sudoku size will be n^3 \n (standard sudoku is n = 3)");
            SetGrid(lvl);
            SetTool();

            Watch.Restart();
            Grid.Matrix = Tool.SolveSudoku();
            Watch.Stop();
            SolutionDataLog.Add(new SolutionData(Watch.ElapsedTicks, Settings.Tool));

            Grid.Print();
            if (Tool.IsSolved()) Console.WriteLine("Sudoku completed!");
            else Console.WriteLine("This sudoku is impossible or has multiple solutions");
            if (Settings.devMode) Console.WriteLine("this sudoku took " + Watch.ElapsedTicks + "ticks to be solved");
        }

        private void ConfigureSettings()
        {
            Screen.PrintTitle();
            Console.WriteLine("SETTINGS");
            Console.WriteLine("1. Solving Tool - using " + Settings.Tool.ToString());
            Console.WriteLine("2. Developer mode - " + (Settings.devMode ? "ON" : "OFF"));
            Console.WriteLine("3. Back to Menu");

            switch (Console.ReadLine())
            {
                case "1":
                    ToolSelection();
                    break;
                case "2":
                    Settings.devMode = !Settings.devMode;
                    Console.WriteLine("2. Developer mode " + (Settings.devMode ? "ON" : "OFF"));
                    break;
                case "3":
                    Console.WriteLine("No changes");
                    break;
                default:
                    Console.WriteLine("Please press a valid key");
                    break;
            }
        }

        private void ToolSelection()
        {
            Screen.PrintTitle();
            Console.WriteLine("Please select a tool:");
            Console.WriteLine("1. Matrix Tool");
            Console.WriteLine("2. Bitwise Tool");

            switch (Console.ReadLine())
            {
                case "1":
                    Settings.Tool = ToolEnum.Tools.MatrixTool;
                    Console.WriteLine("Tool changed");
                    break;
                case "2":
                    Settings.Tool = ToolEnum.Tools.BitwiseTool;
                    Console.WriteLine("Tool changed");
                    break;
                default:
                    Console.WriteLine("Please press a valid key");
                    break;
            }
        }

        private void ShowSolutionLog()
        {
            Screen.PrintTitle();
            if (SolutionDataLog.Count() == 0)
            {
                Console.WriteLine("There is no solution data");
                return;
            }
            Console.WriteLine("Tool Used \t Elapsed time in ticks");
            int counter = 0;
            foreach (SolutionData data in SolutionDataLog)
                Console.WriteLine(counter++ + ". " + data.ToolUsed.ToString() + "\t" + data.ExecutionTimeInTicks.ToString() + " ticks");


            SolutionData matrixAvrg = new SolutionData(AverageTimeForTool(ToolEnum.Tools.MatrixTool), ToolEnum.Tools.MatrixTool);
            SolutionData bitwiseAvrg = new SolutionData(AverageTimeForTool(ToolEnum.Tools.BitwiseTool), ToolEnum.Tools.BitwiseTool);

            Console.WriteLine("Tool Used \t Average time in ticks");
            Console.WriteLine(ToolEnum.Tools.MatrixTool.ToString() + "\t" + matrixAvrg.ExecutionTimeInTicks);
            Console.WriteLine(ToolEnum.Tools.BitwiseTool.ToString() + "\t" + bitwiseAvrg.ExecutionTimeInTicks);
            Console.WriteLine();
            Console.WriteLine("-- Comparative --");
            CompareSolutions(matrixAvrg, bitwiseAvrg);
        }

        private double AverageTimeForTool(ToolEnum.Tools tool)
        {
            int count = SolutionDataLog.Where(x => x.ToolUsed == tool).Count();
            return SolutionDataLog.Where(x => x.ToolUsed == tool).Sum(x => x.ExecutionTimeInTicks) / (count == 0 ? 1 : count);
        }

        private void CompareSolutions(SolutionData that, SolutionData other)
        {
            double val = (that.ExecutionTimeInTicks - other.ExecutionTimeInTicks) / ((that.ExecutionTimeInTicks + other.ExecutionTimeInTicks) / 2) * 100;
            string bestTool = val <= 0 ? that.ToolUsed.ToString() : other .ToolUsed.ToString();
            string worseTool = val > 0 ? that.ToolUsed.ToString() : other .ToolUsed.ToString();
            Console.WriteLine(bestTool + " is " + Math.Round(Math.Abs(val), 2) + "% more efficient than " + worseTool);
        }

        private int ReadNum(string message)
        {
            Console.Clear();
            Screen.PrintTitle();
            try
            {
                Console.WriteLine(message);
                int lvl = int.Parse(Console.ReadLine());
                return lvl;
            }
            catch
            {
                Console.WriteLine("Please try a valid number");
                return ReadNum(message);
            }
        }

        private void SetGrid(int size)
        {
            Grid = new Grid(size, true);
            Grid.ReadInput();
        }

        private void SetTool()
        {
            switch (Settings.Tool)
            {
                case ToolEnum.Tools.MatrixTool:
                    Tool = new MatrixTool(Grid.Matrix);
                    break;
                case ToolEnum.Tools.BitwiseTool:
                    Tool = new BitwiseTool(Grid.Matrix);
                    break;
            }
        }

        #endregion
    }
}
