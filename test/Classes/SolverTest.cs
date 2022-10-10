using NUnit.Framework;
using sudoku.Classes;
using sudoku.Utils;

namespace test.Classes
{
    [TestFixture]
    public class SolverTest
    {
        private Solver solver;

        [SetUp]
        public void SetUp()
        {
            solver = new Solver();
        }

        [Test]
        public void SolveSudoku()
        {
            solver.SetMatrixFrom2D(MatrixHelper.GetSampleSudoku());
            int[,] solution = solver.SolveSudoku();
            Assert.AreEqual(MatrixHelper.GetSampleSolution(), solution);
        }
    }
}
