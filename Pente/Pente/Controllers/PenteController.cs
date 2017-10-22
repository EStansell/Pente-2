using Pente.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Controllers
{
	[Serializable]
    public class PenteController : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;
		private PenteCellectaCanvas[,] board;
        private Player whitePlayer = null;
        private Player notWhitePlayer = null;
        private int[] boardCenter;
        private int currentRound = 1;
		private int notWhiteCaptureCount;
		private int whiteCaptureCount;
		private string currentPlayerName;
        public bool isWhitePlayersTurn = false;
        public int xTest;
        public int yTest;


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
            if (ySize < 9 || ySize > 39)
            {
                throw new ArgumentException("Y Board size is not allowed. Must be between 9 and 39. Input = " + ySize);
            }
            board = new PenteCellectaCanvas[xSize, ySize];

            boardCenter = new int[] { xSize/2, ySize/2 };

            whitePlayer = new Player(player1Name, false);
            notWhitePlayer = new Player(player2Name, true);

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
                if (isWhitePlayersTurn)
                {
                    currentRound++;
                }

                isWhitePlayersTurn = isWhitePlayersTurn ? false : true;
				CurrentPlayerName = isWhitePlayersTurn ? whitePlayer.Name : notWhitePlayer.Name;
            }
            return placedPeice;
        }

        public bool IsValidOption(int x, int y)
        {
			Console.WriteLine(x + " " + y);
            bool validOption = (board[x, y].IsWhitePlayer == null);
            if (!isWhitePlayersTurn && currentRound == 2)
            {
                Console.WriteLine("In method");
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
            Console.WriteLine(x + " " + y);
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