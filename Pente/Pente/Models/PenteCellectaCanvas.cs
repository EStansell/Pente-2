using Pente.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pente.Models
{

	/// <summary>
	/// This class represents the main game board which provides basic
	/// functionality of the Pente game.
	/// 
	/// Its responsibility is to navigate communication between the WPF front-end
	/// and the Pente Controller back-end.	
	/// </summary>
	[Serializable]
	public class PenteCellectaCanvas : Canvas
    {
		public static int CELL_HEIGHT { get; set; }
		public static int CELL_WIDTH { get; set; }
		public int XPos { get; }
        public int YPos { get; }
		public static bool IsWhiteColor { get; set; } = true;

        private PenteController penteController;

        private Shape shape;

        private bool? isWhitePlayer;

        public bool? IsWhitePlayer
        {
            get { return isWhitePlayer;}
            set {
                Background = Brushes.Transparent;
                Opacity = 100;
                switch (value)
                {
                    case true:
                        // place/show a white piece
                        shape.Fill = Brushes.White;
                        break;
                    case false:
                        shape.Fill = Brushes.Black;
                        break;
                    default:
                        shape.Fill = Brushes.Transparent;
                        break;
                }
                isWhitePlayer = value;
            }
        }


        public PenteCellectaCanvas(int xPos, int yPos, PenteController penteController, int cellHeight, int cellWidth)
		{
            CreatePiece();

			CELL_HEIGHT = cellHeight;
			CELL_WIDTH = cellWidth;

			XPos = xPos;
            YPos = yPos;
            this.penteController = penteController;
            this.penteController.PutCanvas(XPos, YPos, this);
			// subscribe function to the mouse down event 
			PreviewMouseDown += ProcessCanvas_Click;
			MouseEnter += ProcessCanvas_Hover;
			MouseLeave += ProcessCanvas_Hover;

		}

        /// <summary>
        /// The function which controls the interaction with a gameboard Canvas
        /// </summary>
        private void ProcessCanvas_Click(object sender, MouseButtonEventArgs e)
        {
            // cast our object back into a Canvas that we can manipulate
            Canvas canvas = (Canvas)sender;

            if(penteController.IsValidOption(XPos, YPos))
            {
                penteController.AttemptPlacement(XPos, YPos);
            }
		}

        public bool CanvaseMouseOverTest(bool aName)
        {
            return aName;
        }

		/// <summary>
		/// NEEDS SUMMARY
		/// </summary>
		private void ProcessCanvas_Hover(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// get our canvas to manipulate
			Canvas canvas = (Canvas)sender;
            if(penteController.IsValidOption(XPos, YPos))
            {
                // hover exit statements
                if (canvas.IsMouseOver)
                {
                    // change opacity to see underlying grid
                    canvas.Opacity = 0.2;
                    canvas.Background = Brushes.LightGreen;
                    CanvaseMouseOverTest(true);
                }
                else
                {
                    canvas.Opacity = 1.0;
                    canvas.Background = Brushes.Transparent;
                    CanvaseMouseOverTest(false);
                }
            }

			
		}

        private void CreatePiece()
        {
            Point point = new Point((this.Width / 2), (this.Height / 2));
            shape = new Ellipse()
            {
                Width = CELL_WIDTH,
                Height = CELL_HEIGHT,
                Fill = Brushes.Transparent
            };

            SetLeft(shape, point.X);
            SetTop(shape, point.Y);
            Children.Add(shape);
        }
    }
}
