using sudoku.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Classes
{
    public class SolutionData
    {
        public double ExecutionTimeInTicks { get; set; }
        public ToolEnum.Tools ToolUsed { get; set; }

        public SolutionData(double executionTimeInTicks, ToolEnum.Tools toolUsed)
        {
            ExecutionTimeInTicks = executionTimeInTicks;
            ToolUsed = toolUsed;
        }
    }
}
