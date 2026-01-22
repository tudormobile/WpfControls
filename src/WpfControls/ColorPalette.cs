using System.Windows.Media;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a themed color palette with utility methods for creating brushes and gradients.
/// </summary>
/// <remarks>
/// The ColorPalette class provides a convenient way to work with predefined color themes. Each palette
/// contains six harmonious colors ranging from light to dark, and offers factory methods for creating
/// solid brushes, linear gradients, and radial gradients. Static instances are provided for quick access
/// to common color themes.
/// </remarks>
public class ColorPalette
{
    private Brush[]? _brushes;
    private LinearGradientBrush? _horizontalBrush;
    private LinearGradientBrush? _verticalBrush;
    private RadialGradientBrush? _radialBrush;
    private RadialGradientBrush? _reversedRadialBrush;

    /// <summary>
    /// Defines the available color group themes.
    /// </summary>
    /// <remarks>
    /// Each color group corresponds to a predefined palette in <see cref="ColorDefinitions.ColorGroups"/>.
    /// The groups are designed to provide harmonious color schemes for various UI themes and data visualizations.
    /// </remarks>
    public enum ColorGroup
    {
        /// <summary>
        /// Green color group - Natural, growth-oriented colors.
        /// </summary>
        Green,

        /// <summary>
        /// Purple color group - Creative, sophisticated colors.
        /// </summary>
        Purple,

        /// <summary>
        /// Orange color group - Energetic, warm colors.
        /// </summary>
        Orange,

        /// <summary>
        /// Blue color group - Professional, trustworthy colors.
        /// </summary>
        Blue,

        /// <summary>
        /// Tan color group - Neutral, earthy colors.
        /// </summary>
        Tan,

        /// <summary>
        /// Aquamarine color group - Fresh, calming colors.
        /// </summary>
        Aquamarine,

        /// <summary>
        /// Rose color group - Soft, elegant colors.
        /// </summary>
        Rose,

        /// <summary>
        /// Turquoise color group - Vibrant, modern colors.
        /// </summary>
        Turquoise,
    }

    /// <summary>
    /// Gets a static instance of the Green color palette.
    /// </summary>
    public static ColorPalette Green { get; } = new(ColorGroup.Green);

    /// <summary>
    /// Gets a static instance of the Purple color palette.
    /// </summary>
    public static ColorPalette Purple { get; } = new(ColorGroup.Purple);

    /// <summary>
    /// Gets a static instance of the Orange color palette.
    /// </summary>
    public static ColorPalette Orange { get; } = new(ColorGroup.Orange);

    /// <summary>
    /// Gets a static instance of the Blue color palette.
    /// </summary>
    public static ColorPalette Blue { get; } = new(ColorGroup.Blue);

    /// <summary>
    /// Gets a static instance of the Tan color palette.
    /// </summary>
    public static ColorPalette Tan { get; } = new(ColorGroup.Tan);

    /// <summary>
    /// Gets a static instance of the Aquamarine color palette.
    /// </summary>
    public static ColorPalette Aquamarine { get; } = new(ColorGroup.Aquamarine);

    /// <summary>
    /// Gets a static instance of the Rose color palette.
    /// </summary>
    public static ColorPalette Rose { get; } = new(ColorGroup.Rose);

    /// <summary>
    /// Gets the color group theme associated with this palette.
    /// </summary>
    public ColorGroup Group { get; }

    /// <summary>
    /// Gets the array of colors in this palette.
    /// </summary>
    /// <remarks>
    /// Returns six colors ranging from the primary color through various shades to the darkest color.
    /// The color order is: primary, lightest, light-medium, medium-dark, dark, darkest.
    /// </remarks>
    public Color[] Colors => ColorDefinitions.ColorGroups[(int)Group];

    /// <summary>
    /// Gets an array of solid color brushes created from the palette colors.
    /// </summary>
    /// <remarks>
    /// The brushes are lazily initialized and cached for performance. Each brush corresponds to a color
    /// in the <see cref="Colors"/> array.
    /// </remarks>
    public Brush[] Brushes => _brushes ??= [.. Colors.Select(c => new SolidColorBrush(c))];

    /// <summary>
    /// Gets the primary solid brush for this palette (the first color in the group).
    /// </summary>
    /// <remarks>
    /// This is typically the most representative color of the theme and is suitable for primary UI elements.
    /// </remarks>
    public Brush PrimaryBrush => Brushes[0];

    /// <summary>
    /// Gets the recommended text brush for light theme backgrounds.
    /// </summary>
    /// <remarks>
    /// Returns the darkest color in the palette, which provides good contrast on light backgrounds.
    /// </remarks>
    public Brush LightThemeTextBrush => Brushes[^1];

    /// <summary>
    /// Gets the recommended text brush for dark theme backgrounds.
    /// </summary>
    /// <remarks>
    /// Returns the lightest color in the palette, which provides good contrast on dark backgrounds.
    /// </remarks>
    public Brush DarkThemeTextBrush => Brushes[1];

    /// <summary>
    /// Gets the primary color for this palette (the first color in the group).
    /// </summary>
    public Color PrimaryColor => Colors[0];

    /// <summary>
    /// Gets the darkest color in the palette.
    /// </summary>
    /// <remarks>
    /// Useful for shadows, text on light backgrounds, or creating depth in UI elements.
    /// </remarks>
    public Color DarkestColor => Colors.Last();

    /// <summary>
    /// Gets the lightest color in the palette.
    /// </summary>
    /// <remarks>
    /// Useful for highlights, text on dark backgrounds, or creating subtle backgrounds.
    /// </remarks>
    public Color LightestColor => Colors[1];

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPalette"/> class with the specified color group.
    /// </summary>
    /// <param name="group">The color group theme for this palette.</param>
    public ColorPalette(ColorGroup group)
    {
        Group = group;
    }

    /// <summary>
    /// Creates a linear gradient brush with the specified angle.
    /// </summary>
    /// <param name="angle">The angle of the gradient in degrees. Default is 0 (left to right).</param>
    /// <returns>A <see cref="LinearGradientBrush"/> transitioning from the lightest to darkest color.</returns>
    /// <remarks>
    /// The gradient transitions from <see cref="LightestColor"/> to <see cref="DarkestColor"/>.
    /// Common angles: 0 (horizontal left-right), 90 (vertical top-bottom), 45 (diagonal).
    /// </remarks>
    public LinearGradientBrush CreateGradientBrush(double angle = 0)
        => new(LightestColor, DarkestColor, angle);

    /// <summary>
    /// Creates a horizontal linear gradient brush (left to right).
    /// </summary>
    /// <returns>A <see cref="LinearGradientBrush"/> with 0-degree angle.</returns>
    public LinearGradientBrush CreateHorizontalGradientBrush()
        => CreateGradientBrush(0);

    /// <summary>
    /// Creates a vertical linear gradient brush (top to bottom).
    /// </summary>
    /// <returns>A <see cref="LinearGradientBrush"/> with 90-degree angle.</returns>
    public LinearGradientBrush CreateVerticalGradientBrush()
        => CreateGradientBrush(90);

    /// <summary>
    /// Creates a radial gradient brush.
    /// </summary>
    /// <param name="isReversed">If true, gradient goes from dark center to light edges; otherwise light center to dark edges.</param>
    /// <returns>A <see cref="RadialGradientBrush"/> with the specified direction.</returns>
    /// <remarks>
    /// By default, the gradient radiates from a light center to dark edges. Set <paramref name="isReversed"/> to true
    /// for a dark center radiating to light edges.
    /// </remarks>
    public RadialGradientBrush CreateRadialGradientBrush(bool isReversed = false)
        => isReversed ? new(DarkestColor, LightestColor) : new(LightestColor, DarkestColor);

    /// <summary>
    /// Gets a cached horizontal linear gradient brush.
    /// </summary>
    /// <remarks>
    /// The brush is lazily initialized and reused for performance. Transitions from light (left) to dark (right).
    /// </remarks>
    public LinearGradientBrush HorizontalBrush => _horizontalBrush ??= CreateHorizontalGradientBrush();

    /// <summary>
    /// Gets a cached vertical linear gradient brush.
    /// </summary>
    /// <remarks>
    /// The brush is lazily initialized and reused for performance. Transitions from light (top) to dark (bottom).
    /// </remarks>
    public LinearGradientBrush VerticalBrush => _verticalBrush ??= CreateVerticalGradientBrush();

    /// <summary>
    /// Gets a cached radial gradient brush with light center to dark edges.
    /// </summary>
    /// <remarks>
    /// The brush is lazily initialized and reused for performance.
    /// </remarks>
    public RadialGradientBrush RadialBrush => _radialBrush ??= CreateRadialGradientBrush();

    /// <summary>
    /// Gets a cached radial gradient brush with dark center to light edges.
    /// </summary>
    /// <remarks>
    /// The brush is lazily initialized and reused for performance. Useful for spotlight or vignette effects.
    /// </remarks>
    public RadialGradientBrush ReversedRadialBrush => _reversedRadialBrush ??= CreateRadialGradientBrush(isReversed: true);
}

