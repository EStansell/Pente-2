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
        public Boolean?[,] board;
        public int[] boardCenter;
        Player[] players;
        
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
            board = new Boolean?[xSize, ySize];

            boardCenter = new int[] { xSize/2, ySize/2 };

            players = new Player[] {new Player(player1Name, false), new Player(player2Name, true)};

            AttemptPlacement(boardCenter[0], boardCenter[1]);
        }

        public bool AttemptPlacement(int x, int y)
        {

            return false;
        }

        public bool isValidOption(int x, int y)
        {
            return (board[x, y] == null);
        }


    }
}