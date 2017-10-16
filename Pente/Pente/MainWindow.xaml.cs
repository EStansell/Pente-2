using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			CreateGrid(); 
        }

		public void CreateGrid()
		{
			mainGrid.Columns = 19;
			mainGrid.Rows = 19;

			for (int row = 0; row < 19; row++)
			{
				//grid1.ColumnDefinitions.Add(new ColumnDefinition());

				for (int col = 0; col < 19; col++)
				{
					//grid1.RowDefinitions.Add(new RowDefinition());

					Label label = new Label() {
						BorderBrush = Brushes.DarkGray,
						BorderThickness = new Thickness(1.0),
						Background = Brushes.Green,
						Width = 30,
						Height = 30
					};

					label.PreviewMouseDown += ProcessLabel_Click;

					mainGrid.Width = (label.Width) * 19;
					mainGrid.Height = (label.Height) * 19;
					mainWindow.Width = label.Width * 19.5;
					mainWindow.Height = label.Height * 20.3;

					mainGrid.Children.Add(label);					
				}
			}

			Border border = new Border();
			border.Width = mainGrid.Width;
			border.Height = mainGrid.Height;
		}

		private void ProcessLabel_Click(object sender, RoutedEventArgs e)
		{
			Label label = (Label)sender;

			if (label.Background == Brushes.Green)
			{
				label.Background = Brushes.Azure;
				return;
			}

			label.Background = Brushes.Green;
		}
	}
}
