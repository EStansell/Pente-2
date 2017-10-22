using Pente.Controllers;
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
       private const int CELL_WIDTH = 20;
       private const int CELL_HEIGHT = 20;
        public int GameBoardColumnCount { get; set; } = 9;
        public int GameBoardRowCount { get; set; } = 9;
        
        // Create controller
        private static PenteController penteController;

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

			// Menu options for boardsize

			//overlayGrid.Visibility = Visibility.Collapsed;
			//mainGrid.Visibility = Visibility.Collapsed;
           
        }


		/// <summary>
		/// Creates the base grid for the gameboard, which will be interacted with during gameplay.
		/// Holds pieces and data relevent to game decisions.
		/// </summary>
		public void CreateGrid()
		{
			// Pente gameboard size is 19x19 cells by standard, make our grid have this size rows/columns
			mainGrid.Columns = (int)WidthSlider.Value;
			mainGrid.Rows = (int)HeightSlider.Value;

			// for each row in the grid
			for (int row = 0; row < (int)HeightSlider.Value; row++)
			{
				// for each column in this row
				for (int col = 0; col < (int)WidthSlider.Value; col++)
				{
					// create the base label object that will interact with the players
					PenteCellectaCanvas canvas = new PenteCellectaCanvas(col, row, penteController, CELL_HEIGHT, CELL_WIDTH) {
						//BorderBrush = Brushes.DarkGray,
						//BorderThickness = new Thickness(0.5),
						Background = Brushes.Transparent								
					};
					// add the label to the grid
					mainGrid.Children.Add(canvas);					
				} // end inner for loop
			} // end outer for loop 


			// set adequate grid size 
			mainGrid.Width = (CELL_WIDTH) * GameBoardColumnCount;
			mainGrid.Height = (CELL_HEIGHT) * GameBoardRowCount;			

			// set adequate window size to fit our label grid
			mainWindow.MinWidth = mainGrid.Width + (CELL_WIDTH * 2);
			mainWindow.MinHeight = mainGrid.Height + (CELL_HEIGHT * 2);
		

		}

        public void CreateOverlay()
        {
			for (int i = 0; i < (int)WidthSlider.Value - 1; i++)
			{
				overlayGrid.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < (int)HeightSlider.Value - 1; i++)
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
			//overlayGrid.MinWidth = ((int)WidthSlider.Value - 1) * CELL_WIDTH;
			//overlayGrid.MinHeight = ((int)HeightSlider.Value - 1) * CELL_HEIGHT;

		}

		private void StartGameButtonClick(object sender, RoutedEventArgs e)
		{
			GameBoardColumnCount = (int)WidthSlider.Value;
			GameBoardRowCount = (int)HeightSlider.Value;
			penteController = new PenteController((int)WidthSlider.Value, (int)HeightSlider.Value, PlayerOneNameBox.Text, PlayerTwoNameBox.Text);
			
			CreateGrid();
			CreateOverlay();

			
			StartingMenuGrid.Visibility = Visibility.Collapsed;
			overlayGrid.Visibility = Visibility.Visible;
			mainGrid.Visibility = Visibility.Visible;
			penteController.PlaceFirstPiece();
		}

		private void Close_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{

		}

		private void Open_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{

		}

		private void Save_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{

		}

		private void SaveAs_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{

		}

		private void HeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			HeightValueLabel.Content = e.NewValue;
		}
		private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			WidthValueLabel.Content = e.NewValue;
		}

	} // end main class
}
