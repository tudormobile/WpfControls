using System.Globalization;
using System.Windows.Data;

namespace Tudormobile.Wpf.Controls.Converters;

/// <summary>
/// Converts a string value to a <see cref="System.Windows.Visibility"/> value based on its content. 
/// If the input value is Null or String.Empty, the converter returns Visibility.Collapsed; otherwise, 
/// it returns Visibility.Visible
/// </summary>
/// <remarks>This converter is typically used in data binding scenarios to control the visibility of UI elements
/// based on the presence or absence of a string value. By default, a non-empty string is converted to  <see
/// cref="System.Windows.Visibility.Visible"/>, and a null or empty string is converted to  <see
/// cref="System.Windows.Visibility.Collapsed"/>. The behavior can be customized using the  <see cref="IsInverted"/> and
/// <see cref="UseHidden"/> properties.</remarks>
public class StringToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation is inverted.
    /// </summary>
    public bool IsInverted { get; set; } = false;

    /// <summary>
    /// Gets or sets a value to indicate whether to use Hidden instead of Collapsed when the string is null or empty.
    /// </summary>
    public bool UseHidden { get; set; } = false;

    /// <summary>
    /// Converts a string value to a <see cref="System.Windows.Visibility"/> value based on its content.
    /// </summary>
    /// <param name="value">The input value to convert. Typically a string whose content determines the resulting <see cref="System.Windows.Visibility"/> value.</param>
    /// <param name="targetType">The type of the binding target property. Usually <see cref="System.Windows.Visibility"/>.</param>
    /// <param name="parameter">An optional parameter to influence the conversion logic. Not used in this implementation.</param>
    /// <param name="culture">The culture to use in the converter. Not used in this implementation.</param>
    /// <returns>A <see cref="System.Windows.Visibility"/> value based on the input string and converter settings.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return IsInverted ? System.Windows.Visibility.Visible : (UseHidden ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Collapsed);
        }
        else
        {
            return IsInverted ? (UseHidden ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Collapsed) : System.Windows.Visibility.Visible;
        }
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
