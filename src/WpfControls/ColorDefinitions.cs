using System.Globalization;
using System.Windows.Media;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Provides static color definitions and utilities for parsing HTML color strings.
/// </summary>
/// <remarks>
/// This class contains predefined color palettes organized into harmonious groups and utility methods
/// for converting HTML/CSS-style color strings to WPF <see cref="Color"/> objects. The color groups
/// are designed to provide aesthetically pleasing color combinations for data visualization.
/// </remarks>
public static class ColorDefinitions
{
    /// <summary>
    /// Converts an HTML color string to a WPF <see cref="Color"/> object.
    /// </summary>
    /// <param name="htmlColor">The HTML color string in RGB or ARGB format (e.g., "#FF0000" or "#80FF0000").</param>
    /// <returns>A <see cref="Color"/> object representing the parsed color.</returns>
    /// <remarks>
    /// Supports both 6-character RGB format (#RRGGBB) and 8-character ARGB format (#AARRGGBB).
    /// The leading '#' character is optional. If the alpha channel is not specified, it defaults to 255 (fully opaque).
    /// </remarks>
    /// <exception cref="FormatException">Thrown when the input string is not a valid hexadecimal color format.</exception>
    /// <example>
    /// <code>
    /// var red = ColorDefinitions.FromHtml("#FF0000");
    /// var semiTransparentBlue = ColorDefinitions.FromHtml("#8000FF00");
    /// </code>
    /// </example>
    public static Color FromHtml(string htmlColor)
    {
        ReadOnlySpan<char> span = htmlColor.AsSpan();

        // Skip '#' if present
        if (span.Length > 0 && span[0] == '#')
        {
            span = span[1..];
        }

        byte a = 255;
        byte r = 0, g = 0, b = 0;

        if (span.Length == 8)
        {
            // ARGB format
            a = byte.Parse(span[..2], NumberStyles.HexNumber);
            r = byte.Parse(span[2..4], NumberStyles.HexNumber);
            g = byte.Parse(span[4..6], NumberStyles.HexNumber);
            b = byte.Parse(span[6..8], NumberStyles.HexNumber);
        }
        else if (span.Length == 6)
        {
            // RGB format
            r = byte.Parse(span[..2], NumberStyles.HexNumber);
            g = byte.Parse(span[2..4], NumberStyles.HexNumber);
            b = byte.Parse(span[4..6], NumberStyles.HexNumber);
        }

        return Color.FromArgb(a, r, g, b);
    }

    /// <summary>
    /// Gets a collection of predefined color groups, each containing harmonious color shades.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each group contains six related color shades ranging from light to dark, designed to work well together
    /// in charts, graphs, and other data visualizations. The groups include:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Green Shades - Natural, growth-oriented colors</description></item>
    /// <item><description>Purple Shades - Creative, sophisticated colors</description></item>
    /// <item><description>Orange Shades - Energetic, warm colors</description></item>
    /// <item><description>Blue Shades - Professional, trustworthy colors</description></item>
    /// <item><description>Tan Shades - Neutral, earthy colors</description></item>
    /// <item><description>Aquamarine Shades - Fresh, calming colors</description></item>
    /// <item><description>Rose Shades - Soft, elegant colors</description></item>
    /// <item><description>Turquoise Shades - Vibrant, modern colors</description></item>
    /// </list>
    /// <para>
    /// Each color array contains six shades: a base color, followed by progressively lighter and darker variations
    /// to provide visual hierarchy and contrast in data presentations.
    /// </para>
    /// </remarks>
    public static readonly Color[][] ColorGroups = [
        [
                // Green Shades
                FromHtml("#4ea72e"),
                FromHtml("#daf2d0"),
                FromHtml("#b5e6a2"),
                FromHtml("#8ed973"),
                FromHtml("#3c7d22"),
                FromHtml("#275317"),
            ],
            [
                // Purple Shades
                FromHtml("#a02b93"),
                FromHtml("#f2ceef"),
                FromHtml("#e49edd"),
                FromHtml("#d86dcd"),
                FromHtml("#782170"),
                FromHtml("#51154a"),
            ],
            [
                // Orange Shades
                FromHtml("#e97132"),
                FromHtml("#fbe2d5"),
                FromHtml("#f7c7ac"),
                FromHtml("#f1a983"),
                FromHtml("#be5014"),
                FromHtml("#7e350e"),
            ],
            [
                // Blue Shades
                FromHtml("#0e2841"),
                FromHtml("#dae9f8"),
                FromHtml("#a6c9ec"),
                FromHtml("#4d93d9"),
                FromHtml("#215c98"),
                FromHtml("#153d64"),
            ],
            [
                // Tan Shades
                FromHtml("#c9a875"),
                FromHtml("#eee3d2"),
                FromHtml("#d7be99"),
                FromHtml("#a67d40"),
                FromHtml("#694f29"),
                FromHtml("#372915"),
            ],
            [
                // Aquamarine Shades
                FromHtml("#4c8665"),
                FromHtml("#d1e5da"),
                FromHtml("#96c4aa"),
                FromHtml("#62a67f"),
                FromHtml("#417356"),
                FromHtml("#20382a"),
            ],
            [
                // Rose Shades
                FromHtml("#b17572"),
                FromHtml("#eadbda"),
                FromHtml("#d6b6b4"),
                FromHtml("#bf8d8b"),
                FromHtml("#8e5350"),
                FromHtml("#512f2d"),
            ],
            [
                // Turquoise Shades
                FromHtml("#0f9ed5"),
                FromHtml("#caedfb"),
                FromHtml("#94dcf8"),
                FromHtml("#61cbf3"),
                FromHtml("#0c769e"),
                FromHtml("#074f69"),
            ],
        ];
}
