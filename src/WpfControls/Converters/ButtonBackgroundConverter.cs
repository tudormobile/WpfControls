using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tudormobile.Wpf.Converters;

/// <summary>
/// Converts a background to a proportional shade. The parameter (0.0-1.0) indicates the proportion
/// to modify the shade. Intended for use with 'Hover' and 'Pressed' effects on buttons. The created
/// brush is cached.
/// </summary>
internal class ButtonBackgroundConverter : IValueConverter
{
    private readonly Dictionary<int, Brush> _cache = [];
    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is Brush b)
        {
            var hash = b.GetHashCode() + parameter?.GetHashCode() ?? 0;
            _cache.TryGetValue(hash, out Brush? brush);
            if (brush != null) return brush;

            var r = b.Clone();
            r.Opacity = Math.Clamp((double)System.Convert.ChangeType(parameter ?? 0, typeof(double)), 0.0, 1.0);
            _cache[hash] = r;
            return r;
        }
        return value;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
