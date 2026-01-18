using System.Windows.Media;

namespace Tudormobile.Wpf.Controls.Helpers;

internal static class ColorHelpers
{
    /// <summary>
    /// Generates a harmonious color palette based on the tint color using HSL color space.
    /// </summary>
    /// <param name="baseTint">The base color to derive the palette from.</param>
    /// <param name="count">The number of colors to generate.</param>
    /// <returns>An array of brushes with evenly distributed hues.</returns>
    internal static Brush[] GenerateColorPalette(Color baseTint, int count)
    {
        if (count <= 0)
            return [];

        if (count == 1)
            return [new SolidColorBrush(baseTint)];

        // Convert base color to HSL
        ColorToHsl(baseTint, out double h, out double s, out double l);

        var brushes = new Brush[count];

        // Define the range for lightness variation (subtle shades)
        var lightnessMin = Math.Max(0.25, l - 0.25);  // Don't go too dark
        var lightnessMax = Math.Min(0.85, l + 0.25);  // Don't go too light
        var lightnessStep = (lightnessMax - lightnessMin) / (count - 1);

        for (int i = 0; i < count; i++)
        {
            // Keep the same hue, vary lightness for subtle shades
            var lightness = lightnessMin + (i * lightnessStep);

            // Slightly adjust saturation for depth (optional)
            var saturation = s * (0.85 + (i * 0.15 / count)); // Subtle saturation variance
            saturation = Math.Clamp(saturation, 0.4, 1.0);

            var color = HslToColor(h, saturation, lightness);
            brushes[i] = new SolidColorBrush(color);
        }

        return brushes;
    }

    /// <summary>
    /// Converts an RGB color to HSL color space.
    /// </summary>
    /// <param name="color">The RGB color.</param>
    /// <param name="h">Hue (0-360).</param>
    /// <param name="s">Saturation (0-1).</param>
    /// <param name="l">Lightness (0-1).</param>
    private static void ColorToHsl(Color color, out double h, out double s, out double l)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        // Lightness
        l = (max + min) / 2.0;

        if (delta == 0)
        {
            h = 0;
            s = 0;
        }
        else
        {
            // Saturation
            s = l < 0.5 ? delta / (max + min) : delta / (2.0 - max - min);

            // Hue
            if (max == r)
                h = ((g - b) / delta + (g < b ? 6 : 0)) / 6.0;
            else if (max == g)
                h = ((b - r) / delta + 2) / 6.0;
            else
                h = ((r - g) / delta + 4) / 6.0;

            h *= 360.0;
        }
    }

    /// <summary>
    /// Converts HSL color values to an RGB color.
    /// </summary>
    /// <param name="h">Hue (0-360).</param>
    /// <param name="s">Saturation (0-1).</param>
    /// <param name="l">Lightness (0-1).</param>
    /// <returns>The RGB color.</returns>
    private static Color HslToColor(double h, double s, double l)
    {
        h = h / 360.0;

        double r, g, b;

        if (s == 0)
        {
            r = g = b = l; // achromatic
        }
        else
        {
            double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            double p = 2 * l - q;
            r = HueToRgb(p, q, h + 1.0 / 3.0);
            g = HueToRgb(p, q, h);
            b = HueToRgb(p, q, h - 1.0 / 3.0);
        }

        return Color.FromRgb(
            (byte)Math.Round(r * 255),
            (byte)Math.Round(g * 255),
            (byte)Math.Round(b * 255));
    }

    /// <summary>
    /// Helper function to convert hue component to RGB.
    /// </summary>
    /// <param name="p">The adjusted lightness value.</param>
    /// <param name="q">The calculated lightness value.</param>
    /// <param name="t">The hue component.</param>
    /// <returns>The RGB component value (0-1).</returns>
    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2.0) return q;
        if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;
        return p;
    }
}
