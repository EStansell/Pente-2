using Pente.Controllers;
using Pente.Converters;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

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
	public class PenteCellectaCanvas : INotifyPropertyChanged
    {
		[field:NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		public static int CELL_HEIGHT { get; set; }
		public static int CELL_WIDTH { get; set; }
		public int XPos { get; }
        public int YPos { get; }
        [field:NonSerialized]
		public DispatcherTimer timer = null;
		private PenteController penteController;		
		private bool? isWhitePlayer;

		public bool? IsWhitePlayer
        {
            get { return isWhitePlayer; }
            set {
                isWhitePlayer = value;
				FieldChanged();
            }
        }
		

        public PenteCellectaCanvas(int xPos, int yPos, PenteController penteController, int cellHeight, int cellWidth)
		{
			CELL_HEIGHT = cellHeight;
			CELL_WIDTH = cellWidth;
			
			XPos = xPos;
            YPos = yPos;
            this.penteController = penteController;
            this.penteController.PutCanvas(XPos, YPos, this);
			// subscribe function to the mouse down event 
		}


		protected void FieldChanged([CallerMemberName] string field = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(field));
			}
		}


		/// <summary>
		/// The function which controls the interaction with a gameboard Canvas
		/// </summary>
		public void ProcessCanvas_Click(object sender, MouseButtonEventArgs e)
        {
            // cast our object back into a Canvas that we can manipulate
            Canvas canvas = (Canvas)sender;

			if (penteController.IsValidOption(XPos, YPos))
			{
				canvas.Opacity = 1.0;
				canvas.Background = Brushes.Transparent;
				penteController.AttemptPlacement(XPos, YPos);
				UpdateChildShape(canvas);
			}
		}

        
		private void UpdateChildShape(Canvas canvas)
		{
			Ellipse child = (Ellipse)canvas.Children[0];
			if (isWhitePlayer == null)
			{
				child.Fill = Brushes.Transparent;
			}
			else
			{
				child.Fill = IsWhitePlayer == true ? Brushes.White : Brushes.Black;
				child.Opacity = 100.0;
			}
		}


		/// <summary>
		/// NEEDS SUMMARY
		/// </summary>
		public void ProcessCanvas_Hover(object sender, System.Windows.Input.MouseEventArgs e)
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
		public bool CanvaseMouseOverTest(bool aName)
		{
			return aName;
		}
		
    }
}
