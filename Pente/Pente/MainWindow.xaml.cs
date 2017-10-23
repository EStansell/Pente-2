using Microsoft.Win32;
using Pente.Controllers;
using Pente.Converters;
using Pente.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pente
{
<<<<<<< HEAD
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        private SaveFileDialog fileSaver = new SaveFileDialog();
        private OpenFileDialog fileOpener = new OpenFileDialog();
        private string fileDialogFilter = "Pente Game (*.pente)|*.pente*";
        public string filePath = null;
        [field: NonSerialized]
        private static DispatcherTimer countdownTimer = new DispatcherTimer();

        public int moveTime = 20;
        private const int CELL_WIDTH = 20;
        private const int CELL_HEIGHT = 20;
        private string name1 = null;
        private string name2 = null;


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
                    PenteCellectaCanvas cell = new PenteCellectaCanvas(col, row, penteController, CELL_HEIGHT, CELL_WIDTH);
                    cell.timer = countdownTimer;
                    Canvas canvas = CreateCanvas(cell);

                    // add the label to the grid
                    mainGrid.Children.Add(canvas);
                } // end inner for loop
            } // end outer for loop 
            Binding binding1 = new Binding("CurrentPlayerName")
            {
                Source = penteController
            };
            lblCurrentPlayer.SetBinding(ContentProperty, binding1);
        }


        private void SetElementSizes()
        {
            // Pente gameboard size is 19x19 cells by standard, make our grid have this size rows/columns
            mainGrid.Columns = GameBoardColumnCount;
            mainGrid.Rows = GameBoardRowCount;

            // set adequate grid size 
            mainGrid.Width = (CELL_WIDTH) * GameBoardColumnCount;
            mainGrid.Height = (CELL_HEIGHT) * GameBoardRowCount;

            // set adequate window size to fit our label grid
            //mainWindow.Width = (mainGrid.Width + (CELL_WIDTH * 2)) + 150;
            //mainWindow.Height = (mainGrid.Height + (CELL_HEIGHT * 2)) + 150;

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
            SetElementSizes();
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

            Binding binding1 = new Binding("WhiteCaptureCount")
            {
                Source = penteController,
                StringFormat = $"{penteController.whitePlayer.Name} Captured: {0}"
            };
            lblWhiteCaptures.SetBinding(ContentProperty, binding1);

            Binding binding2 = new Binding("NotWhiteCaptureCount")
            {
                Source = penteController,
                StringFormat = $"{penteController.notWhitePlayer.Name} Captured: {0}"
            };
            lblNotWhiteCaptures.SetBinding(ContentProperty, binding2);

            name1 = penteController.CurrentPlayerName;

            SetTimer();
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

                        Binding binding1 = new Binding("CurrentPlayerName")
                        {
                            Source = penteController,
                            StringFormat = $"Current Player: {0}"
                        };
                        lblCurrentPlayer.SetBinding(ContentProperty, binding1);

                    }
                }
            }
        }

        public void Save_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
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

        public void SaveAs_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
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
            mainGrid.Children.Clear();
            overlayGrid.Children.Clear();
            overlayGrid.ColumnDefinitions.Clear();
            overlayGrid.RowDefinitions.Clear();

            // if the board is not null (the game has been started)
            if (board != null)
            {
                // re-set grid/column counts
                PlayerOneNameBox.Text = penteController.whitePlayer.Name;
                PlayerTwoNameBox.Text = penteController.notWhitePlayer.Name;
                GameBoardColumnCount = board.GetLength(0);//changed
                GameBoardRowCount = board.GetLength(1); //changed

                // re-set sizes now that we have the dimensions
                SetElementSizes();

                for (int x = 0; x < board.GetLength(1); x++)
                {
                    for (int y = 0; y < board.GetLength(0); y++)
                    {
                        Canvas canvas = CreateCanvas(board[y,x]);
                        mainGrid.Children.Add(canvas);
                    }
                }

                //foreach (PenteCellectaCanvas cell in board)
                //{
                //    Canvas canvas = CreateCanvas(cell);

                //    mainGrid.Children.Add(canvas);
                // }


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
         
        public void SetTimer()
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
                lblUpdateUser.Text = $"{penteController.CurrentPlayerName}'s time is up!\nNext player's turn starts now.";
                penteController.MoveTimeElapsed();
                moveTime = 20;
                countdownTimer.Start();
                 lblCountdown.Foreground = Brushes.Black;
            }

            if (moveTime == 5)
            {
                lblCountdown.Foreground = Brushes.Red;
            }
             
            if (name1 != penteController.CurrentPlayerName && name2 != penteController.CurrentPlayerName)
            {
                name2 = penteController.CurrentPlayerName;
                moveTime = 20;
                countdownTimer.Start();
                lblCountdown.Foreground = Brushes.Black;
            }
            else if (name2 == penteController.CurrentPlayerName && name1 != penteController.CurrentPlayerName)
            {
                name1 = penteController.CurrentPlayerName;
                moveTime = 20;
                countdownTimer.Start();
                lblCountdown.Foreground = Brushes.Black;
            }

            lblCountdown.Content = $" {moveTime.ToString()}";
        }

        private Ellipse CreatePiece()
        {
            Point point = new Point(0, 0);
            Ellipse shape = new Ellipse()
            {
                Width = CELL_WIDTH,
                Height = CELL_HEIGHT,
                Fill = Brushes.Transparent
            };

            Canvas.SetLeft(shape, point.X);
            Canvas.SetTop(shape, point.Y);

            return shape;
        }


        private Canvas CreateCanvas(PenteCellectaCanvas cell)
        {
            // set a binding converter to get opacity based on an empty or filled cell
            Binding opacityBinding = new Binding("IsWhitePlayer")
            {
                Source = cell,
                Converter = new BoolToOpacityConverter()
            };

            // set a binding converter to get piece color based on an empty or filled cell
            Binding colorBinding = new Binding("IsWhitePlayer")
            {
                Source = cell,
                Converter = new BoolToBrushConverter()
            };

            // create the canvas for the grid
            Canvas canvas = new Canvas()
            {
                DataContext = cell,
                Background = Brushes.Transparent
            };

            // create the shape to be a child of the canvas, representing a game board piece
            Ellipse s = CreatePiece();
            colorBinding.ConverterParameter = s;
            // set shape color based on current status of the piece (open, player1, or player2)
            s.SetBinding(ForegroundProperty, colorBinding);

            // set opacity binding for the canvas and piece based on who controls it
            canvas.SetBinding(OpacityProperty, opacityBinding);
            s.SetBinding(OpacityProperty, opacityBinding);

            // add shape
            canvas.Children.Add(s);

            // subscribe to action handlers
            canvas.PreviewMouseDown += cell.ProcessCanvas_Click;
            canvas.MouseEnter += cell.ProcessCanvas_Hover;
            canvas.MouseLeave += cell.ProcessCanvas_Hover;

            return canvas;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string message = "Win by placing five (or more) of your stones in a row, vertically,\n " +
                            "horizontally, or diagonally, with no empty points between them.\n " +
                            "Or, win by capturing five (or more) pairs of your opponent's stones.";
            MessageBox.Show(message);
        }
    } // end main class
=======
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// A go-between for app functionality
	/// Responsibilities are:
	///		Creation of the game board and all aspects therein
	///		Controls the timer
	///		Controls saving and loading game instances
	///		Helps make the UI interactive
	/// </summary>
	public partial class MainWindow : Window
	{
		[field: NonSerialized]
		private static DispatcherTimer countdownTimer = new DispatcherTimer();
		private static PenteController penteController = new PenteController(9, 9, null, null);
		private static int moveTime = 20;
		private const int CELL_WIDTH = 20;
		private const int CELL_HEIGHT = 20;
		private BinaryFormatter formatter = new BinaryFormatter();
		private SaveFileDialog fileSaver = new SaveFileDialog();
		private OpenFileDialog fileOpener = new OpenFileDialog();
		private string fileDialogFilter = "Pente Game (*.pente)|*.pente* |All Files (*.*)|(*.*)";
		private string filePath = null;
		private string name1 = null;
		private string name2 = null;
		public int GameBoardColumnCount { get; set; } = 9;
		public int GameBoardRowCount { get; set; } = 9;

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


		/// <summary>
		/// This contructor
		/// </summary>
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
					PenteCellectaCanvas cell = new PenteCellectaCanvas(col, row, penteController, CELL_HEIGHT, CELL_WIDTH);
					cell.timer = countdownTimer;
					Canvas canvas = CreateCanvas(cell);

					// add the label to the grid
					mainGrid.Children.Add(canvas);
				} // end inner for loop
			} // end outer for loop 
			Binding binding1 = new Binding("CurrentPlayerName")
			{
				Source = penteController
			};
			lblCurrentPlayer.SetBinding(ContentProperty, binding1);
		}


		/// <summary>
		/// Sets the dimensions of the grids
		/// </summary>
		private void SetElementSizes()
		{
			// Pente gameboard size is 19x19 cells by standard, make our grid have this size rows/columns
			mainGrid.Columns = GameBoardColumnCount;
			mainGrid.Rows = GameBoardRowCount;

			// set adequate grid size 
			mainGrid.Width = (CELL_WIDTH) * GameBoardColumnCount;
			mainGrid.Height = (CELL_HEIGHT) * GameBoardRowCount;

			// set adequate window size to fit our label grid
			//mainWindow.Width = (mainGrid.Width + (CELL_WIDTH * 2)) + 150;
			//mainWindow.Height = (mainGrid.Height + (CELL_HEIGHT * 2)) + 150;

			// set adequate overlayGrid size
			overlayGrid.Width = (GameBoardColumnCount - 1) * CELL_WIDTH;
			overlayGrid.Height = (GameBoardRowCount - 1) * CELL_HEIGHT;
		}


		/// <summary>
		/// Creates the grid overlay that will help "point" the user to where their piece will land
		/// </summary>
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
		

		/// <summary>
		/// From the startup window, takes the values the user entered from the UIElements
		/// and uses them to help create key game parameters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StartGameButtonClick(object sender, RoutedEventArgs e)
		{
			GameBoardColumnCount = (int)WidthSlider.Value;
			GameBoardRowCount = (int)HeightSlider.Value;
			penteController = new PenteController(GameBoardColumnCount, GameBoardRowCount, PlayerOneNameBox.Text, PlayerTwoNameBox.Text);
			SetElementSizes();
			CreateGame();
		}


		/// <summary>
		/// Stands up the main gaming window
		/// Including label binding, grid creation, timer setting, and visibility modifications
		/// </summary>
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

			Binding binding1 = new Binding("WhiteCaptureCount")
			{
				Source = penteController,
				StringFormat = $"{penteController.whitePlayer.Name} Captured: {0}"
			};
			lblWhiteCaptures.SetBinding(ContentProperty, binding1);

			Binding binding2 = new Binding("NotWhiteCaptureCount")
			{
				Source = penteController,
				StringFormat = $"{penteController.notWhitePlayer.Name} Captured: {0}"
			};
			lblNotWhiteCaptures.SetBinding(ContentProperty, binding2);

			name1 = penteController.CurrentPlayerName;

			SetTimer();
		}

		
		/// <summary>
		/// Closes the application without warning or prompt
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Close_Click(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			Close();
			Application.Current.Shutdown();
		}


		/// <summary>
		/// Opens a file dialog for the user to navigate to their saved game file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

						Binding binding1 = new Binding("CurrentPlayerName")
						{
							Source = penteController,
							StringFormat = $"Current Player: {0}"
						};
						lblCurrentPlayer.SetBinding(ContentProperty, binding1);

					}
				}
			}
		}


		/// <summary>
		/// Saves the current state of the game in the already established location if it exists.
		/// If the path is not yet set, the event is re-directed to the SaveAs_Click method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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


		/// <summary>
		/// Opens a save dialog for the user to navigate to their desired save location
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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


		/// <summary>
		/// Takes the deserialized controller and sets the cells back into the game board
		/// Re-sets the game objects to their previous state
		/// </summary>
		/// <param name="board"></param>
		public void LoadSavedGame(PenteCellectaCanvas[,] board)
		{
			countdownTimer.Tick -= CountdownTimer_Tick;
			mainGrid.Children.Clear();
			overlayGrid.Children.Clear();
			overlayGrid.ColumnDefinitions.Clear();
			overlayGrid.RowDefinitions.Clear();

			// if the board is not null (the game has been started)
			if (!penteController.whitePlayer.Name.Equals("[Unknown]"))
			{
				// re-set grid/column counts
				PlayerOneNameBox.Text = penteController.whitePlayer.Name;
				PlayerTwoNameBox.Text = penteController.notWhitePlayer.Name;
				GameBoardRowCount = board.GetLength(1);

				// re-set sizes now that we have the dimensions
				SetElementSizes();

				// re-populate the game grid
				for (int x = 0; x < board.GetLength(1); x++)
				{
					for (int y = 0; y < board.GetLength(0); y++)
					{
						Canvas canvas = CreateCanvas(board[y, x]);
						mainGrid.Children.Add(canvas);
					}
				}

				CreateGame();
			} 

			// if the game board is null, start the app as if it is the first run			
		}


		/// <summary>
		/// Helper method to handle instant label updates for enhanced UX when choosing grid size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			HeightValueLabel.Content = e.NewValue;
		}


		/// <summary>
		/// Helper method to handle instant label updates for enhanced UX when choosing grid size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			WidthValueLabel.Content = e.NewValue;
		}


		/// <summary>
		/// Set the "shot clock" so the client's friends don't take up too much time
		/// </summary>
		private void SetTimer()
		{
			countdownTimer.Tick += new EventHandler(CountdownTimer_Tick);
			countdownTimer.Interval = new TimeSpan(0, 0, 1); // 1 second
			moveTime = 20;
			lblCountdown.Foreground = Brushes.Black;
			lblCountdown.Content = $" {moveTime.ToString()}";
			countdownTimer.Start();
		}


		/// <summary>
		/// Handler which takes care of the "shot clock" tick events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CountdownTimer_Tick(object sender, EventArgs e)
		{
			moveTime--;
			if (moveTime == 0)
			{
				// time is up! change turns
				countdownTimer.Stop();
				lblUpdateUser.Text = $"{penteController.CurrentPlayerName}'s time is up!\nNext player's turn starts now.";
				penteController.MoveTimeElapsed();
				moveTime = 20;
				countdownTimer.Start();
				lblCountdown.Foreground = Brushes.Black;
			}

			if (moveTime == 5)
			{
				lblCountdown.Foreground = Brushes.Red;
			}



			if (name1 != penteController.CurrentPlayerName && name2 != penteController.CurrentPlayerName)
			{
				name2 = penteController.CurrentPlayerName;
				moveTime = 20;
				countdownTimer.Start();
				lblCountdown.Foreground = Brushes.Black;
			}
			else if (name2 == penteController.CurrentPlayerName && name1 != penteController.CurrentPlayerName)
			{
				name1 = penteController.CurrentPlayerName;
				moveTime = 20;
				countdownTimer.Start();
				lblCountdown.Foreground = Brushes.Black;
			}

			lblCountdown.Content = $" {moveTime.ToString()}";
		}


		/// <summary>
		/// Create the Canvas child object that will represent a game board piece
		/// </summary>
		/// <returns>The Ellipse object</returns>
		private Ellipse CreatePiece()
		{
			Point point = new Point(0, 0);
			Ellipse shape = new Ellipse()
			{
				Width = CELL_WIDTH,
				Height = CELL_HEIGHT,
				Fill = Brushes.Transparent
			};

			Canvas.SetLeft(shape, point.X);
			Canvas.SetTop(shape, point.Y);

			return shape;
		}


		/// <summary>
		/// Create canvas object with the "cell" argument as its datacontext
		/// </summary>
		/// <param name="cell">The PenteCellecaCanvas data to be bound to the canvas</param>
		/// <returns>A Canvas object to be used elsewhere</returns>
		private Canvas CreateCanvas(PenteCellectaCanvas cell)
		{
			// set a binding converter to get opacity based on an empty or filled cell
			Binding opacityBinding = new Binding("IsWhitePlayer")
			{
				Source = cell,
				Converter = new BoolToOpacityConverter()
			};

			// set a binding converter to get piece color based on an empty or filled cell
			Binding colorBinding = new Binding("IsWhitePlayer")
			{
				Source = cell,
				Converter = new BoolToBrushConverter()
			};

			// create the canvas for the grid
			Canvas canvas = new Canvas()
			{
				DataContext = cell,
				Background = Brushes.Transparent
			};

			// create the shape to be a child of the canvas, representing a game board piece
			Ellipse s = CreatePiece();
			colorBinding.ConverterParameter = s;
			// set shape color based on current status of the piece (open, player1, or player2)
			s.SetBinding(ForegroundProperty, colorBinding);

			// set opacity binding for the canvas and piece based on who controls it
			canvas.SetBinding(OpacityProperty, opacityBinding);
			s.SetBinding(OpacityProperty, opacityBinding);

			// add shape
			canvas.Children.Add(s);

			// subscribe to action handlers
			canvas.PreviewMouseDown += cell.ProcessCanvas_Click;
			canvas.MouseEnter += cell.ProcessCanvas_Hover;
			canvas.MouseLeave += cell.ProcessCanvas_Hover;

			return canvas;
		}


		/// <summary>
		/// Show the Instructions by way of pop-up window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowInstructions_Click(object sender, RoutedEventArgs e)
		{
			string message = "Win by placing five (or more) of your stones in a row, vertically,\n " +
							"horizontally, or diagonally, with no empty points between them.\n " +
							"Or, win by capturing five (or more) pairs of your opponent's stones.";
			MessageBox.Show(message);
		}
	}
>>>>>>> 54b0f89fa7da7488d8e126b97e6bcd81e9f39ee8
}
