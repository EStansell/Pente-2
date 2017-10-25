using System;
using System.Globalization;
using System.Windows.Data;

namespace Pente.Converters
{
	/// <summary>
	/// This is used to convert the current cell's IsWhitePlayer bool? value to the opacity value that the canvas parent should have
	/// </summary>
	[Serializable]
	public class BoolToOpacityConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? boolVal = (bool?)value;
			double opacity = 0.2;
			
			if (boolVal != null)
			{
				opacity = 100.0;
			}

			return opacity;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
