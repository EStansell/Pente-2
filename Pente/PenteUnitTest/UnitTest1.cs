using Pente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls.Primitives;
using Pente.Models;
using Pente.Controllers;

namespace PenteUnitTest
{
	[TestClass]
	public class UnitTest1
	{
		private MainWindow main = new MainWindow();

		[TestMethod]
		public void GridCreationTest()
		{
			UniformGrid grid = main.GetMainGrid;
			int rows = grid.Rows;
			int cols = grid.Columns;
			int expected = rows * cols;
			int childCount = grid.Children.Count;

			Assert.AreEqual(expected, childCount, "Amount of children did not match what was expected!");
			
		}


        //things to test
        /*
         * Start Game test works
         *
         * on Click Places correct color piece
         * on Click again Places opposite Color Piece
         * 
         * 
         * 
        */
        [TestMethod]
        public void GridPiecePlacementTest()
        {
            PenteController pc = new PenteController(19, 19, "tester1", "tester2");
            Assert.AreEqual(true, pc.AttemptPlacement(4, 7), "The Piece placed was not placed");
        }

        public void Test3()
        {
            //PenteCellectaCanvas pcc = new PenteCellectaCanvas();


        }
	}
}
