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

		public PenteCellectaCanvas(int xPos, int yPos)
		{
            this.XPos = xPos;
            this.YPos = yPos;
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

			// if a shape already exists in the canvas, clear it
			if (canvas.Children.Count > 0)
			{
				canvas.Children.Clear();
				return;
			}

			Point point = new Point((canvas.Width / 2), (canvas.Height / 2));			
			Shape shape;
			shape = new Ellipse()
			{
				Width = 30,
				Height = 30,			
			};

			shape.Opacity = 1.0;
			shape.Fill = IsWhiteColor ? Brushes.White : Brushes.Black;
			IsWhiteColor = !IsWhiteColor;

			SetLeft(shape, point.X);
			SetTop(shape, point.Y);
			canvas.Children.Add(shape);
		}

		/// <summary>
		/// NEEDS SUMMARY
		/// </summary>
		private void ProcessCanvas_Hover(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// get our canvas to manipulate
			Canvas canvas = (Canvas)sender;

			// hover exit statements
			if (canvas.Background == Brushes.LightGreen)
			{
				// return opacity to normal
				canvas.Opacity = 1.0;
				canvas.Background = Brushes.Transparent;
				return;
			}

			// change opacity to see underlying grid
			canvas.Opacity = 0.2;
			canvas.Background = Brushes.LightGreen;

		}
	}
}
