using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a donut chart control that displays data as segments of a ring with an optional center content area.
/// </summary>
/// <remarks>
/// The donut chart is similar to a pie chart but features a hollow center that can display additional content
/// such as totals, labels, or other contextual information. The chart supports customizable corner radius for
/// rounded segments and data binding through the Series property.
/// </remarks>
public class DonutChart : Chart
{
    private FrameworkElement? _presenter;

    /// <summary>
    /// Gets or sets the corner radius for the donut chart segments.
    /// </summary>
    /// <remarks>
    /// Setting a corner radius creates rounded edges on the donut segments. A value of 0 results in sharp edges.
    /// Uniform corner radii are typically used for consistent appearance across all segments.
    /// </remarks>
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty
        .Register(nameof(CornerRadius),
        typeof(CornerRadius), typeof(DonutChart), new PropertyMetadata(new CornerRadius(0)));

    /// <summary>
    /// Gets or sets the content to be displayed in the center of the donut chart.
    /// </summary>
    /// <remarks>
    /// The center content area is ideal for displaying summary information, totals, labels, or other
    /// contextual data. The content can be any object and will be rendered using the <see cref="ContentTemplate"/>
    /// if specified, or the default string representation.
    /// </remarks>
    [System.ComponentModel.Bindable(true)]
    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Content"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentProperty = DependencyProperty
        .Register(nameof(Content),
        typeof(object), typeof(DonutChart), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the data template used to display the center content.
    /// </summary>
    /// <remarks>
    /// The content template defines how the <see cref="Content"/> object is rendered in the center of the donut chart.
    /// If not specified, the content will be displayed using its default string representation or data template.
    /// </remarks>
    public DataTemplate ContentTemplate
    {
        get { return (DataTemplate)GetValue(ContentTemplateProperty); }
        set { SetValue(ContentTemplateProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="ContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty
        .Register(nameof(ContentTemplate),
        typeof(DataTemplate), typeof(DonutChart), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the format string used to display the center content.
    /// </summary>
    /// <remarks>
    /// This property allows you to specify a format string for the content display, such as "{0:N2}" for
    /// numeric formatting or "{0:P0}" for percentage formatting. This is applied when <see cref="ContentTemplate"/>
    /// is not specified.
    /// </remarks>
    [System.ComponentModel.Bindable(true)]
    public string ContentStringFormat
    {
        get { return (string)GetValue(ContentStringFormatProperty); }
        set { SetValue(ContentStringFormatProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="ContentStringFormat"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty
        .Register(nameof(ContentStringFormat),
        typeof(string), typeof(DonutChart), new PropertyMetadata(null));

    static DonutChart()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DonutChart), new FrameworkPropertyMetadata(typeof(DonutChart)));
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        // see if we can find the content presenter for the center content
        if (GetTemplateChild("PART_ContentPresenter") is ContentPresenter contentPresenter)
        {
            _presenter = contentPresenter;
        }
        base.OnApplyTemplate();
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        var root = this;
        var values = Series?.DataPoints;
        if (values != null)
        {
            var presenter = (_presenter ?? root);

            var colors = TintBrushes;

            var top = Math.Max(this.ActualHeight - root.ActualHeight, 0) / 2;
            var left = Math.Max(this.ActualWidth - root.ActualWidth, 0) / 2;
            var rMax = Math.Min(root.ActualWidth, root.ActualHeight) / 2;
            var rMin2 = Math.Max(presenter.ActualHeight / 2, presenter.ActualWidth / 2);
            var rMin = Math.Max(Math.Min(0.7 * rMax, rMin2), 0.5 * rMax);  // 70 percent inner radius for donut hole (or less)
            var p = new Point(left + root.ActualWidth / 2, top + root.ActualHeight / 2);

            var slices = this.Series switch
            {
                LabelledSeries ls => ls.DataPoints,
                ChartSeries cs => cs.DataPoints,
                _ => null
            };

            var startAngle = 0.0;

            if (slices != null)
            {
                var total = slices.Sum();
                if (total <= 0)
                {
                    return;
                }
                foreach (var (index, slice) in slices.Index())
                {
                    var sweepAngle = (double)(slice / total) * 360.0;
                    var startPointOnCircle = new Point(
                        p.X + rMax * Math.Cos(startAngle * Math.PI / 180.0),
                        p.Y + rMax * Math.Sin(startAngle * Math.PI / 180.0));
                    var endPointOnCircle = new Point(
                        p.X + rMax * Math.Cos((startAngle + sweepAngle) * Math.PI / 180.0),
                        p.Y + rMax * Math.Sin((startAngle + sweepAngle) * Math.PI / 180.0));
                    var startPointOnInnerCircle = new Point(
                        p.X + rMin * Math.Cos(startAngle * Math.PI / 180.0),
                        p.Y + rMin * Math.Sin(startAngle * Math.PI / 180.0));
                    var endPointOnInnerCircle = new Point(
                        p.X + rMin * Math.Cos((startAngle + sweepAngle) * Math.PI / 180.0),
                        p.Y + rMin * Math.Sin((startAngle + sweepAngle) * Math.PI / 180.0));

                    var pathGeometry = new PathGeometry();
                    var pathFigure = new PathFigure();

                    // 1. Set the start point (inner circle)
                    Point startPoint = startPointOnInnerCircle;
                    pathFigure.StartPoint = startPoint;

                    // 2. Add a line to the arc's start point (on outer circle)
                    Point arcStartPoint = startPointOnCircle;
                    LineSegment lineToArcStart = new LineSegment(arcStartPoint, true);
                    pathFigure.Segments.Add(lineToArcStart);

                    // 3. Add the arc segment (Clockwise, outer circle)
                    Point arcEndPoint = endPointOnCircle;
                    Size arcSize = new Size(rMax, rMax);
                    bool isLargeArc = sweepAngle > 180.0;  // Add this line
                    ArcSegment arc = new ArcSegment(arcEndPoint, arcSize, 0, isLargeArc, SweepDirection.Clockwise, true);
                    pathFigure.Segments.Add(arc);

                    // 4. Add a line segment back to the inner circle end point
                    LineSegment lineToInnerCircleEnd = new LineSegment(endPointOnInnerCircle, true);
                    pathFigure.Segments.Add(lineToInnerCircleEnd);

                    // 5. Add the inner arc segment (CounterClockwise, inner circle)
                    Point innerArcEndPoint = startPointOnInnerCircle;
                    Size innerArcSize = new Size(rMin, rMin);
                    bool isInnerLargeArc = sweepAngle > 180.0;  // Add this line
                    ArcSegment innerArc = new ArcSegment(innerArcEndPoint, innerArcSize, 0, isInnerLargeArc, SweepDirection.Counterclockwise, true);
                    pathFigure.Segments.Add(innerArc);

                    // 6. Add the figure to the geometry and set the path data
                    pathGeometry.Figures.Add(pathFigure);

                    // Create the Path element and set its properties
                    Path filledArc = new Path();
                    filledArc.Fill = colors[index % colors.Length];
                    filledArc.Stroke = TintBrush;
                    filledArc.StrokeThickness = 1;
                    filledArc.Data = pathGeometry;

                    drawingContext.DrawGeometry(filledArc.Fill, new Pen(filledArc.Stroke, filledArc.StrokeThickness), pathGeometry);

                    // draw the text label
                    var midAngle = startAngle + sweepAngle / 2;
                    var labelRadius = rMax * 0.8; // position label at 80% of radius
                    var labelPoint = new Point(
                        p.X + labelRadius * Math.Cos(midAngle * Math.PI / 180.0),
                        p.Y + labelRadius * Math.Sin(midAngle * Math.PI / 180.0));
                    var percentage = (slice / total) * 100;
                    var label = Series switch
                    {
                        LabelledSeries ls => (index < ls.Labels.Count) ? ls.Labels[index] : "",
                        _ => ""
                    };
                    var formattedText = new FormattedText(
                        $"{label}({percentage:F1}%)",
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        9,
                        Brushes.Black,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    // Draw the text label
                    drawingContext.DrawText(formattedText, labelPoint);

                    // Update the starting angle for the next slice
                    startAngle += sweepAngle;
                }
            }
        }
        base.OnRender(drawingContext);
    }
}
