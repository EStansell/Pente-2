using Pente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls.Primitives;
using Pente.Models;
using Pente.Controllers;

namespace PenteUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public MainWindow main = new MainWindow();

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

        [TestMethod]
        public void GridPiecePlacementTest()
        {
            PenteController pc = new PenteController(19, 19, "Tester1", "Tester2");

            pc.isTesting = true;
            for (int row = 0; row < 19; row++)
            {//start of first loo0p

                for (int col = 0; col < 19; col++)
                {//start of second loop

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 20, 20);

                }//end of second loop

            }//end of first loop
            pc.PlaceFirstPiece();
            pc.AttemptPlacement(4, 7);
            pc.AttemptPlacement(0, 8);
            Assert.AreEqual(true, pc.board[4, 7].IsWhitePlayer, "The Piece Was not Places");
            Assert.AreEqual(true, !pc.board[0, 8].IsWhitePlayer, "Piece was not Placed");
        }
        [TestMethod]
        public void FirstPieceCenter()
        {
            //Checking for Center piece of Grid size 9x9
            PenteController pc = new PenteController(9, 9, "Tester1", "Tester2");
            pc.isTesting = true;

            Assert.AreEqual(4, pc.boardCenter[0], $"X2 Not Middle Board Center is { pc.boardCenter[0]}");
            Assert.AreEqual(4, pc.boardCenter[1], $"Y2 Not Middle Board Center is {pc.boardCenter[1]}");

            //Checking for Center Piece of Grid Size 9x21
            PenteController pcc = new PenteController(9, 21, "Tester1", "Tester2");
            Assert.AreEqual(4, pcc.boardCenter[0], $"X2 Not Middle Board Center is {pcc.boardCenter[0]}");
            Assert.AreEqual(10, pcc.boardCenter[1], $"Y2 Not Middle Board Center is {pcc.boardCenter[1]}");

            //Checking to see if Black piece is first
            Assert.AreEqual(false, pcc.isWhitePlayersTurn, "Black Piece is Not First");
        }

        [TestMethod]
        public void ValidGridSize()
        {
            PenteController pc = new PenteController(9, 39, "Tester1", "Tester2");
            pc.isTesting = true;

            Assert.AreEqual(true, pc.xIsRightSize % 2 == 1, "xSize is not an Odd Number");
            Assert.AreEqual(true, pc.yIsRightSize % 2 == 1, "ySize is not an Odd Number");
            Assert.AreEqual(true, pc.xIsRightSize >= 9 && pc.xIsRightSize <= 39, "xSize does not fit between 9-39");
            Assert.AreEqual(true, pc.yIsRightSize >= 9 && pc.yIsRightSize <= 39, "ySize does not fit between 9-39");

        }


        [TestMethod]
        public void checkHorizontalWinTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {//start of first loo0p

                for (int col = 0; col < 9; col++)
                {//start of second loop

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);

                }//end of second loop

            }//end of first loop

            //Place 5 Pieces in a Row to see if check win method equals 5 
            //Horizontally
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(0, 7);//white
            pc.AttemptPlacement(0, 1);//black
            pc.AttemptPlacement(1, 7);//white
            pc.AttemptPlacement(0, 2);//black
            pc.AttemptPlacement(2, 7);//white
            pc.AttemptPlacement(0, 3);//black
            pc.AttemptPlacement(3, 7);//white
            pc.AttemptPlacement(8, 1);//black
            pc.AttemptPlacement(4, 7);//white
            Assert.AreEqual(5, pc.CheckWin(3, 7, true), "Vertical Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(0, 7, true), "Vertical Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(1, 7, true), "Vertical Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(2, 7, true), "Vertical Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(4, 7, true), "Vertical Win Does Not Work");

        }

        [TestMethod]
        public void checkVerticalWinTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");

            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            //Place 5 Pieces in a Row to see if check win method equals 5 
            //Vertically
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(7, 0);//white
            pc.AttemptPlacement(0, 1);//black
            pc.AttemptPlacement(7, 1);//white
            pc.AttemptPlacement(0, 2);//black
            pc.AttemptPlacement(7, 2);//white
            pc.AttemptPlacement(0, 3);//black
            pc.AttemptPlacement(7, 3);//white
            pc.AttemptPlacement(8, 1);//black
            pc.AttemptPlacement(7, 4);//white
            Assert.AreEqual(5, pc.CheckWin(7, 0, true), "Horizontal Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(7, 1, true), "Horizontal Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(7, 2, true), "Horizontal Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(7, 3, true), "Horizontal Win Does Not Work");
            Assert.AreEqual(5, pc.CheckWin(7, 4, true), "Horizontal Win Does Not Work");
        }

        [TestMethod]
        public void checkDiagonalWinTest()
        {
            PenteController pc = new PenteController(9, 9, "tester1", "tester2");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            //Place 5 Pieces in a Row to see if check win method equals 5 
            //Diagonal

            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(7, 4);//white
            pc.AttemptPlacement(1, 1);//black
            pc.AttemptPlacement(6, 5);//white
            pc.AttemptPlacement(8, 0);//black
            pc.AttemptPlacement(5, 6);//white
            pc.AttemptPlacement(4, 0);//black
            pc.AttemptPlacement(4, 7);//white
            pc.AttemptPlacement(0, 4);//black
            pc.AttemptPlacement(3, 8);//white 

            Assert.AreEqual(5, pc.CheckWin(7, 4, true), $"Diagonal Win Does Not Work, Highest Count Is: {pc.CheckWin(0, 1, true)}");
            Assert.AreEqual(5, pc.CheckWin(3, 8, true), $"Diagonal Win Does Not Work, Highest Count Is: {pc.CheckWin(3, 8, true)}");

        }

        [TestMethod]
        public void CaptureVerticalStonesTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);

                }
            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(0, 5);//white
            pc.AttemptPlacement(0, 6);//black
            pc.AttemptPlacement(0, 4);//white
            pc.AttemptPlacement(0, 3);//black
            pc.checkCapture(0, 6, false);

            Assert.AreEqual(null, pc.board[0, 5].IsWhitePlayer, "Vertical Capture Stones Does Not Work");
            Assert.AreEqual(null, pc.board[0, 4].IsWhitePlayer, "Vertical Capture Stones Does Not Work");

        }
        [TestMethod]
        public void CaptureHorizontalStoneTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(2, 1);//white
            pc.AttemptPlacement(1, 1);//black
            pc.AttemptPlacement(3, 1);//white
            pc.AttemptPlacement(4, 1);//black
            pc.checkCapture(4, 1, false);

            Assert.AreEqual(null, pc.board[3, 1].IsWhitePlayer, "Horizontal Capture Stones Does Not Work");
            Assert.AreEqual(null, pc.board[2, 1].IsWhitePlayer, "Horizontal Capture Stones Does Not Work");
        }

        [TestMethod]
        public void CaptureDiagonalStoneTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);

                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(3, 3);//white
            pc.AttemptPlacement(0, 0);//black
            pc.AttemptPlacement(2, 2);//white
            pc.AttemptPlacement(1, 1);//black
            pc.checkCapture(3, 3, false);

            Assert.AreEqual(null, pc.board[2, 2].IsWhitePlayer, "Diagonal Capture Stones Does Not Work");
            Assert.AreEqual(null, pc.board[3, 3].IsWhitePlayer, "Diagonal Capture Stones Does Not Work");
        }

        [TestMethod]
        public void CaptureFiveWinsTest()
        {
            PenteController pc = new PenteController(9, 9, "TESTER1", "TESTER2");
            pc.isTesting = true;


            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {

                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(2, 1);//white
            pc.AttemptPlacement(1, 1);//black
            pc.AttemptPlacement(3, 1);//white
            pc.AttemptPlacement(4, 1);//black

            pc.AttemptPlacement(0, 5);//white
            pc.AttemptPlacement(0, 6);//black
            pc.AttemptPlacement(0, 4);//white
            pc.AttemptPlacement(0, 3);//black

            pc.AttemptPlacement(3, 7);//white
            pc.AttemptPlacement(2, 7);//black
            pc.AttemptPlacement(4, 7);//white
            pc.AttemptPlacement(5, 7);//black

            pc.AttemptPlacement(7, 3);//white
            pc.AttemptPlacement(7, 2);//black
            pc.AttemptPlacement(7, 4);//white
            pc.AttemptPlacement(7, 5);//black

            pc.AttemptPlacement(6, 0);//white
            pc.AttemptPlacement(5, 0);//black
            pc.AttemptPlacement(7, 0);//white
            pc.AttemptPlacement(8, 0);//black

            pc.checkCapture(4, 1, false);
            pc.checkCapture(0, 6, false);
            pc.checkCapture(6, 7, false);
            pc.checkCapture(7, 2, false);
            pc.checkCapture(8, 0, false);
            Assert.AreEqual(5, pc.NotWhiteCaptureCount, $"Capturing 5 Pairs Win Does Not Work Count Is: {pc.NotWhiteCaptureCount}");
        }

        [TestMethod]
        public void PlayerNameStoredTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            Assert.AreEqual(true, pc.notWhitePlayer.Name == "Boris", "Player One Name Not Stored");
            Assert.AreEqual(true, pc.whitePlayer.Name == "Kyle", "Player Two Name Not Stored");

        }
        [TestMethod]
        public void BlackSecondMoveOutOfBoundaryTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(1, 4);//white
            pc.AttemptPlacement(4, 3);//black
            pc.AttemptPlacement(3, 4);//white

            Assert.AreEqual(null, !(pc.board[4, 3].IsWhitePlayer), "Black Cannot Place Piece In Boundary");
        }
        [TestMethod]
        public void SwitchBlackToWhiteTurnsTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece(); //black
            pc.AttemptPlacement(2, 3);//white
            Assert.AreEqual(false, pc.isWhitePlayersTurn, "Switching Turns DOes Not Work");


        }
        [TestMethod]
        public void SwitchWhiteToBlackTurnsTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(2, 4);//white
            pc.AttemptPlacement(4, 8);//black
            Assert.AreEqual(false, !pc.isWhitePlayersTurn, "Switching Turns Does Not Work");
        }
        //[TestMethod]
        //public void SaveTest()
        //{
        //    PenteController pc = new PenteController(9, 9, "Boris", "Kyle");

        //}
        [TestMethod]
        public void TriaTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }

            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(0, 7);//white
            pc.AttemptPlacement(0, 1);//black
            pc.AttemptPlacement(1, 7);//white
            pc.AttemptPlacement(0, 2);//black
            pc.AttemptPlacement(2, 7);//white

            pc.CheckWin(0, 7, true);
            pc.Yell(true, 3);
            Assert.AreEqual(true, pc.isTria, $"Tria Yell Does Not Work its {pc.isTria} ");
        }
        [TestMethod]
        public void TesseraTest()
        {
            PenteController pc = new PenteController(9, 9, "Boris", "Kyle");
            pc.isTesting = true;

            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    PenteCellectaCanvas canvas = new PenteCellectaCanvas(row, col, pc, 10, 10);
                }

            }
            pc.PlaceFirstPiece();//black
            pc.AttemptPlacement(0, 7);//white
            pc.AttemptPlacement(0, 1);//black
            pc.AttemptPlacement(1, 7);//white
            pc.AttemptPlacement(0, 2);//black
            pc.AttemptPlacement(2, 7);//white
            pc.AttemptPlacement(0, 3);//black
            pc.AttemptPlacement(3, 7);//white

            pc.CheckWin(0, 7, true);
            pc.Yell(true, 4);
            Assert.AreEqual(true, pc.isTessera, $"Tria Yell Does Not Work its {pc.isTessera} ");

        }
        [TestMethod]
        public void SetTimerTest()
        {
            main.SetTimer();
            Assert.AreEqual(20, main.moveTime, "Timer Does Not Set");
        }

    }


}

