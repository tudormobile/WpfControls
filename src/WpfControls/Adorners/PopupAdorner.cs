using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tudormobile.Wpf.Behaviors;

namespace Tudormobile.Wpf.Adorners;

/// <summary>
/// A custom adorner that displays a popup glyph (down arrow or right arrow) on the adorned element. The glyph indicates responds
/// to MouseOver and MouseDown events to provide visual feedback (consistent with the adorned element's foreground color) and displays
/// the PopupContent (via attached property) of the adorned element as either a ContextMenu, a Popup control, or as an arbitrary framework
/// element hosted in a Popup control.
/// </summary>
public class PopupAdorner : GlyphAdorner
{
    private const string DownArrow = "\uF08e";
    private const string RightArrow = "\uF08f";
    private const double PressedOpacity = 0.15;
    private const double HoverOpacity = 0.05;
    private Brush? _backgroundBrush;

    /// <summary>
    /// Creates a new instance of the <see cref="PopupAdorner"/> class, which adorns the specified UI element with an interactive popup glyph.
    /// </summary>
    /// <param name="adornedElement">
    /// The UI element to be adorned with the popup glyph. This element will receive the visual overlay and interactive
    /// behavior provided by the <see cref="PopupAdorner"/>. Must not be <c>null</c> and must be a FrameworkElement.
    /// </param>
    public PopupAdorner(UIElement adornedElement) : base(adornedElement)
    {
        this.FontSize = 8;
        this.IsHitTestVisible = true;
        this.Glyph = Popup.GetPopupContent(adornedElement) switch
        {
            ContextMenu menu => menu.Placement == System.Windows.Controls.Primitives.PlacementMode.Right ? RightArrow : DownArrow,
            System.Windows.Controls.Primitives.Popup popup => popup.Placement == System.Windows.Controls.Primitives.PlacementMode.Right ? RightArrow : DownArrow,
            _ => DownArrow,
        };
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        // Provide visual indication (rectangle) of MouseOver and MouseDown
        Brush b = _backgroundBrush ??= AdornedElement switch
        {
            Control c => c.Foreground.Clone(),
            _ => new SolidColorBrush(Color.FromArgb(1, 0, 0, 0))
        };
        b.Opacity = Mouse.LeftButton == MouseButtonState.Pressed ? PressedOpacity : HoverOpacity;
        if (!this.IsMouseOver)
        {
            b = Brushes.Transparent;
        }
        var element = (FrameworkElement)AdornedElement;
        var xPos = element.ActualWidth - GlyphSize.Width;

        drawingContext.DrawRectangle(b, new Pen(Brushes.Transparent, 0), new Rect(xPos - 8, 0, GlyphSize.Width + 8, element.ActualHeight));

        // add the glyph
        base.OnRender(drawingContext);
        //(
        //    b,
        //    // Brushes.Pink,
        //    //new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)),
        //    new Pen(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), 1),
        //    new Rect(xPos - 8, 0, text.Width + 8, element.ActualHeight)
        //);

        //var text = new FormattedText(s,
        //    CultureInfo.CurrentCulture,
        //    FlowDirection.LeftToRight,
        //    new Typeface("Segoe Fluent Icons"),
        //    8,
        //    (AdornedElement as Control)?.Foreground ?? Brushes.Gray,
        //    VisualTreeHelper.GetDpi(this).PixelsPerDip);



        //var xPos = element.ActualWidth - text.Width;
        //var yPos = element.ActualHeight / 2 - text.Height / 2;


        //drawingContext.DrawText(text, new Point(xPos - 4, yPos));
        //}
    }

    /// <inheritdoc/>
    protected override void OnMouseEnter(MouseEventArgs e) => InvalidateVisual();
    /// <inheritdoc/>
    protected override void OnMouseLeave(MouseEventArgs e) => InvalidateVisual();
    /// <inheritdoc/>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => InvalidateVisual();
    /// <inheritdoc/>
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) => InvalidateVisual();

    //private Brush createBrush(Brush brush, double opacity)
    //{
    //    var result = brush.Clone();
    //    result.Opacity = opacity;
    //    return result;
    //}
}
