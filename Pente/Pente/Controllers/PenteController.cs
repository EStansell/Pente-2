using Pente.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Pente.Controllers
{
	[Serializable]
    public class PenteController : INotifyPropertyChanged
    {
		[field:NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		public PenteCellectaCanvas[,] board;
        public Player whitePlayer = null;
        public Player notWhitePlayer = null;
        public int[] boardCenter;
        private int currentRound = 1;
		public int notWhiteCaptureCount = 0;
		public int whiteCaptureCount = 0;
		public string currentPlayerName;
        public bool isWhitePlayersTurn = false;
        public int xTest;
        public int yTest;
        public int xIsRightSize;
        public int yIsRightSize;
         
		public int WhiteCaptureCount
		{
			get { return whiteCaptureCount; }
			set
			{
				whiteCaptureCount = value;
				FieldChanged();
			}
		}

		public int NotWhiteCaptureCount
		{
			get { return notWhiteCaptureCount; }
			set
			{
				notWhiteCaptureCount = value;
				FieldChanged();
			}
		}

		public string CurrentPlayerName
		{
			get { return currentPlayerName; }
			set
			{
				currentPlayerName = value;
				FieldChanged();
			}
		}

		public int CurrentRound
        {
            get { return currentRound; }
        }

		protected void FieldChanged([CallerMemberName] string field = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(field));
			}
		}


		public PenteController(int xSize, int ySize, String player1Name, String player2Name)
        {
            if (xSize < 9 || xSize > 39)
            {
                throw new ArgumentException("X Board size is not allowed. Must be between 9 and 39. Input = " + xSize);
            }
            else
            {
                xIsRightSize = xSize;
            }
            if (ySize < 9 || ySize > 39)
            {
                throw new ArgumentException("Y Board size is not allowed. Must be between 9 and 39. Input = " + ySize);
            }
            else
            {
                yIsRightSize = ySize;
            }
            board = new PenteCellectaCanvas[xSize,ySize];  

            boardCenter = new int[] { xSize/2, ySize/2 };

            notWhitePlayer = new Player(player1Name, false);
            whitePlayer = new Player(player2Name, true);

        }

        public void PlaceFirstPiece()
        {
            AttemptPlacement(boardCenter[0], boardCenter[1]);
        }

        public bool AttemptPlacement(int x, int y)
        {
            bool placedPeice = false;
            if(IsValidOption(x, y))
            {
                placedPeice = true;

                board[x, y].IsWhitePlayer = isWhitePlayersTurn;

                checkCapture(x, y, isWhitePlayersTurn);
                int highestInARow = checkWin(x, y, isWhitePlayersTurn);


                // if HighestInARow > 4 : win
                Console.WriteLine(highestInARow);

                if (isWhitePlayersTurn)
                {
                    currentRound++;
                }
                isWhitePlayersTurn = isWhitePlayersTurn ? false : true;
				CurrentPlayerName = isWhitePlayersTurn ? whitePlayer.Name : notWhitePlayer.Name;
            }
            return placedPeice;
        }

        public void checkCapture(int x, int y, bool whitePlayer)
        {
            for (int xTran = -1; xTran < 2; xTran++)
            {
                for (int yTran = -1; yTran < 2; yTran++)
                {
                    if (!(xTran == 0 && yTran == 0))
                    {
                        if (testColor(x + xTran, y + yTran, !whitePlayer) &&
                            testColor(x + (xTran * 2), y + (yTran * 2), !whitePlayer) &&
                            testColor(x + (xTran * 3), y + (yTran * 3), whitePlayer))
                        {
                            board[x + xTran, y + yTran].IsWhitePlayer = null;
                            board[x + (xTran * 2), y + (yTran * 2)].IsWhitePlayer = null;
                            if (whitePlayer)
                            {
                                WhiteCaptureCount++;
                                if(WhiteCaptureCount == 5)
                                {
                                    determineWinner(true);
                                }
                            }
                            else
                            {
                                NotWhiteCaptureCount++;
                                if(notWhiteCaptureCount == 5)
                                {
                                    determineWinner(false);

                                }
                            }
                        }
                    }
                }
            }
        }
        public void determineWinner(bool isWhitePlayer)
        {

            if (isWhitePlayer)
            {
                MessageBox.Show($"{whitePlayer.Name} Has Won The Game!");
                App.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show($"{notWhitePlayer.Name} Has Won The Game!");
                App.Current.MainWindow.Close();

            }

        }
        public void Yell(bool isWhitePlayer, int count)
        {
            if (isWhitePlayer)
            {
                if(count == 3)
                {
                    MessageBox.Show($"{whitePlayer.Name} Got a Tria!");
                }
                else if(count == 4)
                {
                    MessageBox.Show($"{whitePlayer.Name} Got a Tessera!");
                }
            }
            else if(!isWhitePlayer)
            {
                if (count == 3)
                {
                    MessageBox.Show($"{notWhitePlayer.Name} Got a Tria!");
                }
                else if (count == 4)
                {
                    MessageBox.Show($"{notWhitePlayer.Name} Got a Tessera!");
                }

            }

        }

        public int checkWin(int x, int y, bool whitePlayer)
        {
            int highestCount = checkRow(x - 4   , y     , 1, 0, whitePlayer);


            int rowCount = checkRow(x + -4, y - 4, 1, 1, whitePlayer);
            highestCount = rowCount > highestCount ? rowCount : highestCount;

            rowCount = checkRow(x, y - 4, 0, 1, whitePlayer);
            highestCount = rowCount > highestCount ? rowCount : highestCount;

            rowCount = checkRow(x + 4, y - 4, -1, 1, whitePlayer);
            highestCount = rowCount > highestCount ? rowCount : highestCount;
            
            if(highestCount == 5)
            {
                determineWinner(isWhitePlayersTurn);
            }
            else if(highestCount == 4 || highestCount == 3)
            {
                Yell(isWhitePlayersTurn, highestCount);
            }
            return highestCount;
        }

        private bool testColor(int x, int y, bool player)
        {
            bool isColor = false;
            if(x >= 0 && x < board.GetLength(0) && 
                    y >= 0 && y < board.GetLength(1))
            {
                isColor = board[x, y].IsWhitePlayer == player;
            }
            return isColor;
        }

        private int checkRow(int x, int y, int xTran, int yTran, bool playerColor)
        {
            int highestCount = 0;
            int inARowCount = 0;
            for(int distance = 0; distance < 9; distance++)
            {
                int newX = x + (xTran * distance);
                int newY = y + (yTran * distance);
                if (testColor(newX, newY, playerColor))
                {
                    inARowCount++;
                    highestCount = highestCount > inARowCount ? highestCount : inARowCount;
                }
                else
                {
                    inARowCount = 0;
                }
            }
            return highestCount;
        }

        public bool IsValidOption(int x, int y)
        {
			//Console.WriteLine(x + " " + y);            
			bool validOption = (board[x, y].IsWhitePlayer == null);
            if (!isWhitePlayersTurn && currentRound == 2)
            {
                int xCenter = boardCenter[0];
                int yCenter = boardCenter[1];
                if ((x >= xCenter + 3 || x <= xCenter - 3)
                    || (y >= yCenter + 3 || y <= yCenter - 3))
                {
                    return validOption;
                }
                else
                {
                    return false;
                }
            }
            return validOption;
        }

        public void PutCanvas(int x,int y, PenteCellectaCanvas canvas)
        {
            //Console.WriteLine(x + " " + y);
            board[x, y] = canvas;
        }


		public PenteCellectaCanvas[,] GetGameBoard()
		{
			return board;
		}

		public void MoveTimeElapsed()
		{
			currentRound++;
			isWhitePlayersTurn = isWhitePlayersTurn? false : true;
			CurrentPlayerName = isWhitePlayersTurn? whitePlayer.Name : notWhitePlayer.Name;
		}

	}
}