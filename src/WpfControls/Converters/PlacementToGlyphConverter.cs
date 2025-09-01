using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Tudormobile.Wpf.Converters;

/// <summary>
/// Converts a <see cref="PlacementMode"/> value to a corresponding glyph representation.
/// </summary>
/// <remarks>This converter is typically used in UI scenarios where a <see cref="PlacementMode"/> value  needs to
/// be visually represented as a glyph. For example, <see cref="PlacementMode.Right"/>  is converted to a right-pointing
/// caret glyph, while other values default to a down-pointing caret glyph.</remarks>
public class PlacementToGlyphConverter : IValueConverter
{
    /// <summary>
    /// Converts a <see cref="PlacementMode"/> value to its corresponding symbol representation.
    /// </summary>
    /// <param name="value">The value to convert, expected to be of type <see cref="PlacementMode"/>.</param>
    /// <param name="targetType">The type to convert to. This parameter is not used in the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion. This parameter is not used in the conversion.</param>
    /// <param name="culture">The culture to use in the conversion. This parameter is not used in the conversion.</param>
    /// <returns>A string representing the symbol for the specified <see cref="PlacementMode"/>.  Returns <c>"\uF08F"</c> if the
    /// value is <see cref="PlacementMode.Right"/>; otherwise, returns <c>"\uF08E"</c>.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is PlacementMode mode && mode == PlacementMode.Right) return "\uF08F"; // CaretSolidRight
        return "\uF08E"; // CaretSolidDown
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
