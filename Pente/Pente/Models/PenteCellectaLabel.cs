using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pente.Models
{

	/// <summary>
	/// This class represents the main game board which provides basic
	/// functionality of the Pente game.
	/// 
	/// Its responsibility is to navigate communication between the WPF front-end
	/// and the Pente Controller back-end.	
	/// </summary>
	public class PenteCellectaLabel : Label
	{
		


		public PenteCellectaLabel()
		{
			// subscribe function to the mouse down event 
			PreviewMouseDown += ProcessLabel_Click;

		}

		/// <summary>
		/// The function which controls the interaction with a gameboard label
		/// </summary>
		private void ProcessLabel_Click(object sender, RoutedEventArgs e)
		{
			// cast our object back into a label that we can manipulate
			Label label = (Label)sender;

			// change background color to show interactivity with mouse clicks
			if (label.Background == Brushes.Green)
			{
				// change from green color and leave function
				label.Background = Brushes.Azure;
				return;
			}

			// change from "azure" color and exit function
			label.Background = Brushes.Green;
		}
	}
}
