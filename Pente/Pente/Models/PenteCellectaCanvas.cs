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
	public class PenteCellectaCanvas : Canvas
    {
        public int XPos { get; }
        public int YPos { get; }
		public static bool IsWhiteColor { get; set; } = true;

        private PenteController penteController;



        public PenteCellectaCanvas(int xPos, int yPos, PenteController penteController)
		{
            this.XPos = xPos;
            this.YPos = yPos;
            this.penteController = penteController;
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

            if(penteController.isValidOption(XPos, YPos))
            { 

                Point point = new Point((canvas.Width / 2), (canvas.Height / 2));
                Shape shape;
                shape = new Ellipse()
                {
                    Width = 30,
                    Height = 30,
                };

                shape.Opacity = 1.0;
                canvas.Background = Brushes.Transparent;

                shape.Fill = penteController.isWhitePlayersTurn ? Brushes.White : Brushes.Black;
                penteController.AttemptPlacement(XPos, YPos);

                SetLeft(shape, point.X);
                SetTop(shape, point.Y);
                canvas.Children.Add(shape);
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
            if(penteController.isValidOption(XPos, YPos))
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
	}
}
