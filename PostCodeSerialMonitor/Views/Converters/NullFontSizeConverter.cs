using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PostCodeSerialMonitor.Views.Converters;

public class NullFontSizeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? 0d : parameter;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}