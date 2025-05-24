using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PostCodeSerialMonitor.Views.Converters;
public class BooleanToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string textOptions)
        {
            var options = textOptions.Split('|');
            if (options.Length == 2)
            {
                return boolValue ? options[1] : options[0];
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}