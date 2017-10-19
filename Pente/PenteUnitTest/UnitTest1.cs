using Pente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls.Primitives;

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
	}
}
