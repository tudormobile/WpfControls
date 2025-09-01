using System.Globalization;
using System.Windows.Data;

namespace Tudormobile.Wpf.Converters;

/// <summary>
/// Converts a null value to a <see cref="System.Windows.Visibility"/> value.
/// </summary>
/// <remarks>This converter is typically used in data binding scenarios to control the visibility of UI elements 
/// based on whether a bound value is null. If the input value is <see langword="null"/>, the method  returns <see
/// cref="System.Windows.Visibility.Collapsed"/>; otherwise, it returns  <see
/// cref="System.Windows.Visibility.Visible"/>.</remarks>
public class NullToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to a <see cref="System.Windows.Visibility"/> enumeration based on its nullability.
    /// </summary>
    /// <param name="value">The value to evaluate. If <see langword="null"/>, the method returns <see
    /// cref="System.Windows.Visibility.Collapsed"/>; otherwise, <see cref="System.Windows.Visibility.Visible"/>.</param>
    /// <param name="targetType">The type to convert to. This parameter is not used in the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion. This parameter is not used in the conversion.</param>
    /// <param name="culture">The culture to use in the conversion. This parameter is not used in the conversion.</param>
    /// <returns>A <see cref="System.Windows.Visibility"/> value: <see cref="System.Windows.Visibility.Collapsed"/> if <paramref
    /// name="value"/> is <see langword="null"/>; otherwise, <see cref="System.Windows.Visibility.Visible"/>.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        return value == null ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }

    /// <summary>
    /// Converts a value back to its source type. This method is not implemented and will throw an exception if called.
    /// </summary>
    /// <param name="value">The value produced by the binding target.</param>
    /// <param name="targetType">The type to convert the value back to.</param>
    /// <param name="parameter">An optional parameter to use during the conversion.</param>
    /// <param name="culture">The culture to use during the conversion.</param>
    /// <returns>This method does not return a value as it is not implemented.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
