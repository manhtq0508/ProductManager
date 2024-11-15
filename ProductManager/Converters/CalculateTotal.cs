using ProductManager.Entities;
using System.Globalization;

namespace ProductManager.Converters;

public class CalculateTotal : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Product product)
        {
            return product.Price * product.Amount;
        }

        return 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Do nothing because we don't need to convert back
        return null;
    }
}
