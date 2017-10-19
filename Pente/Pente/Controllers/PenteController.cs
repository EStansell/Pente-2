using Pente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Controllers
{
    public class PenteController
    {
        private PenteCellectaCanvas[,] board;
        private int[] boardCenter;
        private Player whitePlayer = null;
        private Player notWhitePlayer = null;
        public bool isWhitePlayersTurn = false;
        private int currentRound = 1;

        public int CurrentRound
        {
            get { return currentRound; }
        }

        public PenteController(int xSize, int ySize, String player1Name, String player2Name)
        {
            if (xSize < 8 || xSize > 40)
            {
                throw new ArgumentException("X Board size is not allowed. Must be between 8 and 40. Input = " + xSize);
            }
            if (ySize < 8 || ySize > 40)
            {
                throw new ArgumentException("Y Board size is not allowed. Must be between 8 and 40. Input = " + ySize);
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
            if(isValidOption(x, y))
            {
                placedPeice = true;
                board[x, y].IsWhitePlayer = isWhitePlayersTurn;

                if (isWhitePlayersTurn)
                {
                    currentRound++;
                }

                isWhitePlayersTurn = isWhitePlayersTurn ? false : true;
            }
            return placedPeice;
        }

        public bool isValidOption(int x, int y)
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

        public void putCanvas(int x,int y, PenteCellectaCanvas canvas)
        {
            Console.WriteLine(x + " " + y);
            this.board[x, y] = canvas;
        }

    }
}