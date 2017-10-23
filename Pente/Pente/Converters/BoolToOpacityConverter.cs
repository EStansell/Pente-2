using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pente.Converters
{
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
