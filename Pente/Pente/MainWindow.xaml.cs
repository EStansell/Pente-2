using Pente.Models;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Pente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UniformGrid GetMainGrid
        {
            get
            {
                return mainGrid;
            }
        }

        public MainWindow()
        {
            InitializeComponent();	
			
			// create our base gameboard
			CreateGrid(); 
        }


		/// <summary>
		/// Creates the base grid for the gameboard, which will be interacted with during gameplay.
		/// Holds pieces and data relevent to game decisions.
		/// </summary>
		public void CreateGrid()
		{
			// Pente gameboard size is 19x19 cells, make our grid have this size rows/columns
			mainGrid.Columns = 19;
			mainGrid.Rows = 19;
			const int CELL_WIDTH = 30;
			const int CELL_HEIGHT = 30;

			// for each row in the grid
			for (int row = 0; row < 19; row++)
			{
				// for each column in this row
				for (int col = 0; col < 19; col++)
				{
					// create the base label object that will interact with the players
					PenteCellectaLabel label = new PenteCellectaLabel(row, col) {
						BorderBrush = Brushes.DarkGray,
						BorderThickness = new Thickness(1.0),
						Background = Brushes.Green,
						Width = CELL_WIDTH,
						Height = CELL_HEIGHT					
					};
					// add the label to the grid
					mainGrid.Children.Add(label);					
				} // end inner for loop
			} // end outer for loop 


			// set adequate grid size 
			mainGrid.Width = (CELL_WIDTH) * 19;
			mainGrid.Height = (CELL_HEIGHT) * 19;
			
			// set adequate window size to fit our label grid
			mainWindow.Width = CELL_WIDTH * 19.5;
			mainWindow.Height = CELL_HEIGHT * 20.3;

		}
		
	} // end main class
}
