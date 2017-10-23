using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pente.Converters
{	
	/// <summary>
	/// This is used to convert the current cell's IsWhitePlayer bool? value to the brush color that the ellipse child should have
	/// </summary>
	[Serializable]
	public class BoolToBrushConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? boolVal = (bool?)value;
			Ellipse e = (Ellipse)parameter;
			Brush b;

			if (boolVal == null)
			{
				b = Brushes.Transparent;
			}
			else if (boolVal == true)
			{
				b = Brushes.White;
			}
			else // boolVal == false
			{
				b = Brushes.Black;
			}

			e.Fill = b;
			return b;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
