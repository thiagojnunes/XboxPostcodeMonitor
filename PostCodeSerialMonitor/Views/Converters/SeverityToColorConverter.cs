using Avalonia.Data.Converters;
using Avalonia.Media;
using PostCodeSerialMonitor.Models;
using System;
using System.Globalization;

namespace PostCodeSerialMonitor.Views.Converters;

public class SeverityToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CodeSeverity severity)
        {
            return severity switch
            {
                CodeSeverity.Error => new SolidColorBrush(Color.Parse("#FF0000")), // Red
                CodeSeverity.Warning => new SolidColorBrush(Color.Parse("#FFA500")), // Orange
                _ => new SolidColorBrush(Color.Parse("#FFFFFF")) // White
            };
        }
        return new SolidColorBrush(Color.Parse("#FFFFFF")); // White
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}