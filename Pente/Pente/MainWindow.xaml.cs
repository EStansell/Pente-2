using Microsoft.Win32;
using Pente.Controllers;
using Pente.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pente
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private BinaryFormatter formatter = new BinaryFormatter();
		private SaveFileDialog fileSaver = new SaveFileDialog();
		private OpenFileDialog fileOpener = new OpenFileDialog();
		private string fileDialogFilter = "Pente Game (*.pente)|*.pente*";
		private string filePath = null;
		private static int moveTime = 20;
		private static DispatcherTimer countdownTimer = new DispatcherTimer();
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
			fileOpener.Filter = fileDialogFilter;
			fileSaver.Filter = fileDialogFilter;
		}


		/// <summary>
		/// Creates the base grid for the gameboard, which will be interacted with during gameplay.
		/// Holds pieces and data relevent to game decisions.
		/// </summary>
		public void CreateGrid()
		{			
			// for each row in the grid
			for (int row = 0; row < GameBoardRowCount; row++)
			{
				// for each column in this row
				for (int col = 0; col < GameBoardColumnCount; col++)
				{
					// create the base label object that will interact with the players
					PenteCellectaCanvas canvas = new PenteCellectaCanvas(col, row, penteController, CELL_HEIGHT, CELL_WIDTH) {						
						Background = Brushes.Transparent								
					};
					// add the label to the grid
					mainGrid.Children.Add(canvas);					
				} // end inner for loop
			} // end outer for loop 
			lblCurrentPlayer.Text = $"Current Player: {penteController.CurrentPlayerName}";
		}


		private void SetGridSize()
		{
			// Pente gameboard size is 19x19 cells by standard, make our grid have this size rows/columns
			mainGrid.Columns = GameBoardColumnCount;
			mainGrid.Rows = GameBoardRowCount;

			// set adequate grid size 
			mainGrid.Width = (CELL_WIDTH) * GameBoardColumnCount;
			mainGrid.Height = (CELL_HEIGHT) * GameBoardRowCount;

			// set adequate window size to fit our label grid
			mainWindow.MinWidth = mainGrid.Width + (CELL_WIDTH * 2);
			mainWindow.MinHeight = mainGrid.Height + (CELL_HEIGHT * 2);

			// set adequate overlayGrid size
			overlayGrid.Width = (GameBoardColumnCount - 1) * CELL_WIDTH;
			overlayGrid.Height = (GameBoardRowCount - 1) * CELL_HEIGHT;
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
		}


		private void StartGameButtonClick(object sender, RoutedEventArgs e)
		{
			GameBoardColumnCount = (int)WidthSlider.Value;
			GameBoardRowCount = (int)HeightSlider.Value;
			penteController = new PenteController((int)WidthSlider.Value, (int)HeightSlider.Value, PlayerOneNameBox.Text, PlayerTwoNameBox.Text);
			SetGridSize();
			CreateGame();
		}


		private void CreateGame()
		{
			// if at this point the main grid has no cavases, create them. otherwise, skip it
			if (mainGrid.Children.Count == 0)
			{
				CreateGrid();
			}

			CreateOverlay();

			StartingMenuGrid.Visibility = Visibility.Collapsed;
			overlayGrid.Visibility = Visibility.Visible;
			mainGrid.Visibility = Visibility.Visible;
			penteController.PlaceFirstPiece();
		}


		private void Close_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			Close();
			Application.Current.Shutdown();
		}


		private void Open_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			bool? result = fileOpener.ShowDialog();

			if (result != null)
			{
				bool bResult = (bool)result;

				if (bResult)
				{
					filePath = fileOpener.FileName;

					using (FileStream dataFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
					{
						// deserialize and re-assign objects here
						penteController = (PenteController)formatter.Deserialize(dataFileStream);
						LoadSavedGame(penteController.GetGameBoard());
						lblCurrentPlayer.Text = $"Current Player: {penteController.CurrentPlayerName}";
					}
				}
			}
		}


		private void Save_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			// if we already have a path to save at
			if (filePath != null)
			{
				using (FileStream dataFileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
				{
					// serialize object to file
					formatter.Serialize(dataFileStream, penteController);
				}
			}
			else
			{
				// if we don't have the path, re-direct to SaveAs in order to prompt for save location
				SaveAs_Click(sender, e);
			}
		}


		private void SaveAs_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			// open save dialog for user to navigate to save location
			bool? result = fileSaver.ShowDialog();
			if (result != null)
			{
				// !null means nothing broke
				bool bResult = (bool)result;

				// if they gave us a location
				if (bResult)
				{
					// set path
					filePath = fileSaver.FileName;

					using (FileStream dataFileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
					{
						// serialize object to file 
						formatter.Serialize(dataFileStream, penteController);
					}
				}
			}
		}


		public void LoadSavedGame(PenteCellectaCanvas[,] board)
		{
			// if the board is not null (the game has been started)
			if (board != null)
			{
				// re-set grid/column counts
				GameBoardColumnCount = board.GetLength(0);
				GameBoardRowCount = board.GetLength(1);
				
				// re-set sizes now that we have the dimensions
				SetGridSize();

				// re-populate the game grid
				int size = 0;
				
				foreach (PenteCellectaCanvas canvas in board)
				{
					mainGrid.Children.Add(canvas);
					size++;
				}

				if (size != board.Length)
					throw new Exception("SIZES DON'T MATCH");

				CreateGame();

				// start the timer?
			}

			// if the game board is null, start the app as if it is the first run			
		}


		private void HeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			HeightValueLabel.Content = e.NewValue;
		}


		private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			WidthValueLabel.Content = e.NewValue;
		}


		private void SetTimer()
		{
			countdownTimer.Tick += new EventHandler(CountdownTimer_Tick);
			countdownTimer.Interval = new TimeSpan(0, 0, 1); // 1 second
			moveTime = 20;
			lblCountdown.Foreground = Brushes.Black;
			lblCountdown.Content = $" {moveTime.ToString()}";
			countdownTimer.Start();
		}

		private void CountdownTimer_Tick(object sender, EventArgs e)
		{
			moveTime--;
			if (moveTime == 0)
			{
				// time is up! change turns
				countdownTimer.Stop();
				penteController.MoveTimeElapsed();
				// show popup when the time is up?

			}

			if (moveTime == 5)
			{
				lblCountdown.Foreground = Brushes.Red;
			}

			lblCountdown.Content = $" {moveTime.ToString()}";
		}

		private void Instructions_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{

		}
	} // end main class
}
