using Pente.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Pente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private const int CELL_WIDTH = 30;
       private const int CELL_HEIGHT = 30;
        public int GameBoardColumnCount { get; set; } = 19;
        public int GameBoardRowCount { get; set; } = 19;


        public UniformGrid GetMainGrid
        {
            get
            {
                return mainGrid;
            }
        }

		public Grid GetOverlayGrid
		{
			get
			{
				return overlayGrid;
			}
		}


        public MainWindow()
        {
            InitializeComponent();	
			
			// create our base gameboard
			CreateGrid();

            CreateOverlay();
        }


		/// <summary>
		/// Creates the base grid for the gameboard, which will be interacted with during gameplay.
		/// Holds pieces and data relevent to game decisions.
		/// </summary>
		public void CreateGrid()
		{
			// Pente gameboard size is 19x19 cells, make our grid have this size rows/columns
			mainGrid.Columns = GameBoardColumnCount;
			mainGrid.Rows = GameBoardRowCount;

			// for each row in the grid
			for (int row = 0; row < GameBoardColumnCount; row++)
			{
				// for each column in this row
				for (int col = 0; col < GameBoardRowCount; col++)
				{
					// create the base label object that will interact with the players
					PenteCellectaLabel label = new PenteCellectaLabel(row, col) {
						BorderBrush = Brushes.DarkGray,
						BorderThickness = new Thickness(0.5),
						Background = Brushes.Green								
					};
					// add the label to the grid
					mainGrid.Children.Add(label);					
				} // end inner for loop
			} // end outer for loop 


			// set adequate grid size 
			mainGrid.Width = (CELL_WIDTH) * GameBoardColumnCount;
			mainGrid.Height = (CELL_HEIGHT) * GameBoardRowCount;
			
			// set adequate window size to fit our label grid
			mainWindow.Width = CELL_WIDTH * 19.5;
			mainWindow.Height = CELL_HEIGHT * 20.3;

		}

        public void CreateOverlay()
        {
			for (int i = 0; i < GameBoardColumnCount - 1; i++)
			{
				overlayGrid.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < GameBoardRowCount - 1; i++)
			{
				overlayGrid.RowDefinitions.Add(new RowDefinition());
			}

			for (int col = 0; col < overlayGrid.ColumnDefinitions.Count; col++)
            {
				for (int row = 0; row < overlayGrid.RowDefinitions.Count; row++)
                {
					Border border = new Border()
					{
						BorderBrush = Brushes.White,
						BorderThickness = new Thickness(1),				
                    };

					Grid.SetColumn(border, col);
					Grid.SetRow(border, row);
                    overlayGrid.Children.Add(border);                   
                      
                }
            }
			
            overlayGrid.Width = (GameBoardColumnCount - 1) * CELL_WIDTH;
            overlayGrid.Height = (GameBoardRowCount - 1) * CELL_HEIGHT; 

        }

		
	} // end main class
}
