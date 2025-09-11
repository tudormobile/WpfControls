using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tudormobile.Wpf.Adorners;

/// <summary>
/// Represents an adorner that displays placeholder text for a <see cref="TextBoxBase"/> control when it is empty.
/// </summary>
/// <remarks>This adorner is designed to provide a visual cue to users by displaying placeholder text over a <see
/// cref="TextBoxBase"/> control, such as a <see cref="System.Windows.Controls.TextBox"/>, when the control has no user
/// input. The placeholder text is rendered with reduced opacity to distinguish it from actual user input. The <see
/// cref="PlaceholderText"/> property specifies the text to display as the placeholder. The adorner is not hit-test
/// visible, ensuring it does not interfere with user interactions with the adorned control.</remarks>
public class PlaceholderAdorner : Adorner
{
    private string? _placeholderText;
    Brush? _clone;

    /// <summary>
    /// Gets or sets the placeholder text displayed in the input field when it is empty.
    /// </summary>
    public string? PlaceholderText
    {
        get => _placeholderText;
        set => _placeholderText = value;
    }

    /// <summary>
    /// Represents an adorner that provides a visual placeholder for a UI element.
    /// </summary>
    /// <remarks>This adorner is not hit-testable, meaning it does not respond to user input or mouse
    /// events.</remarks>
    /// <param name="adornedElement">The UI element to which the adorner is attached. Cannot be <see langword="null"/>.</param>
    public PlaceholderAdorner(UIElement adornedElement) : base(adornedElement)
    {
        IsHitTestVisible = false;
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        if (AdornedElement is TextBoxBase tb && !string.IsNullOrEmpty(_placeholderText))
        {
            var b = _clone ??= createClone(tb.Foreground);
            if (tb is System.Windows.Controls.TextBox textBox && !string.IsNullOrEmpty(textBox.Text)) return;
            var txt = new FormattedText(_placeholderText,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                tb.FontSize,
                b,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);
            var xPos = 4 + tb.Padding.Left;
            var yPos = tb.ActualHeight / 2 - txt.Height / 2;
            drawingContext.DrawText(txt, new Point(xPos, yPos));
        }
    }

    private static Brush? createClone(Brush foreground, double opacity = 0.5)
    {
        var b = foreground.Clone();
        b.Opacity = opacity;
        return b;
    }

}

