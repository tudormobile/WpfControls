using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tudormobile.Wpf.Adorners;

/// <summary>
/// Provides a custom adorner that displays a glyph (icon or symbol) alongside a control, such as a <see
/// cref="TextBoxBase"/>. The Glyphs must be frmom the "Segoe Fluent Icons" font. The adorner adjusts the padding of the
/// adorned control to accommodate the glyph, ensuring it does not overlap with the control's content.
/// </summary>
/// <remarks>The <see cref="GlyphAdorner"/> is designed to adorn a control with a glyph, typically for decorative
/// or functional purposes. The glyph is rendered using the "Segoe Fluent Icons" font and is positioned relative to the
/// adorned element's padding. This adorner is not hit-test visible and does not interfere with user interactions on the
/// adorned control.
/// <para>
/// Currently , the adorner is primarily intended for use with <see cref="TextBoxBase"/> controls, however it may be
/// extended in the future to support other control types as needed.
/// </para>
/// </remarks>
public class GlyphAdorner : Adorner
{
    private double? _fontSize = null;
    private Thickness? _padding = null;
    private string? _glyph;
    private FormattedText? _formattedText;
    private Size? _glyphSize;

    /// <summary>
    /// Font size for the glyph
    /// </summary>
    public double FontSize
    {
        get => _fontSize ?? ((AdornedElement as Control)?.FontSize ?? 12) * .75;
        set { _fontSize = value; }
    }

    /// <summary>
    /// Original padding value
    /// </summary>
    public Thickness Padding
    {
        get => _padding ?? new Thickness(0);
        set { _padding = value; }
    }

    /// <summary>
    /// Gets or sets the glyph associated with this instance. The Glyph is a string
    /// representing a character from the "Segoe Fluent Icons" font.
    /// </summary>
    public string? Glyph
    {
        get => _glyph;
        set
        {
            _glyph = value;
            _formattedText = null;
            _glyphSize = null;
        }
    }

    /// <summary>
    /// Gets the size of the glyph represented by the adorned element.
    /// </summary>
    /// <remarks>The size is calculated based on the glyph's text, font size, and other visual properties of
    /// the adorned element. If the adorned element is a <see cref="Control"/>, its font size and foreground brush are
    /// used in the calculation; otherwise 12pt font with Black text is used.</remarks>
    public Size GlyphSize => _glyphSize ??= new Size((_formattedText ??= new FormattedText(_glyph ?? "",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Segoe Fluent Icons"),
                    FontSize,
                    (AdornedElement as Control)?.Foreground ?? Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip)).Width, _formattedText.Height);

    /// <summary>
    /// Initializes a new instance of the <see cref="GlyphAdorner"/> class, which provides a visual adornment for the
    /// specified control.
    /// </summary>
    /// <remarks>The <see cref="GlyphAdorner"/> is a non-interactive visual element, as indicated by its IsHitTestVisible
    /// property being set to <see langword="false"/>.</remarks>
    /// <param name="adornedElement">The <see cref="Control"/> to which the adornment is applied. 
    /// This adorned UIElement cannot be <see langword="null"/>.</param>
    public GlyphAdorner(UIElement adornedElement) : base(adornedElement)
    {
        if (adornedElement is not FrameworkElement)
        {
            throw new ArgumentException("The adorned element must be a FrameworkElement.", nameof(adornedElement));
        }
        IsHitTestVisible = false;
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        var element = (FrameworkElement)AdornedElement;
        var xPos = element.ActualWidth - GlyphSize.Width;
        var yPos = element.ActualHeight / 2 - GlyphSize.Height / 2;

        if (AdornedElement is TextBoxBase tb && !string.IsNullOrEmpty(_glyph))
        {
            xPos = Math.Max(6, tb.Padding.Left - GlyphSize.Width);
            yPos = tb.ActualHeight / 2 - GlyphSize.Height / 2;
        }
        drawingContext.DrawText(_formattedText, new Point(xPos - 4, yPos));
    }

    /// <inheritdoc/>
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        _formattedText = null;
        base.OnRenderSizeChanged(sizeInfo);
        if (AdornedElement is TextBoxBase tb && _padding != null)
        {
            tb.Padding = new Thickness(GlyphSize.Width + Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);
        }
    }
}

