using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a simple pie chart control.
/// </summary>
public class PieChart : Chart
{
    private FrameworkElement? _rootElement;

    static PieChart()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PieChart), new FrameworkPropertyMetadata(typeof(PieChart)));
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        _rootElement = Template.FindName("PART_Canvas", this) is Canvas c
            ? c
            : this;
        InvalidateVisual();
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        var root = _rootElement ?? this;
        var values = Series?.DataPoints;
        if (values != null)
        {
            var colors = TintBrushes;

            var top = Math.Max(this.ActualHeight - root.ActualHeight, 0) / 2;
            var left = Math.Max(this.ActualWidth - root.ActualWidth, 0) / 2;
            var pen = new Pen(TintBrush, 1.0);
            var r = Math.Min(root.ActualWidth, root.ActualHeight) / 2;
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
                var labels = new List<(FormattedText text, Point position)>();
                foreach (var (index, slice) in slices.Index())
                {
                    var sweepAngle = (double)(slice / total) * 360.0;
                    var startPointOnCircle = new Point(
                        p.X + r * Math.Cos(startAngle * Math.PI / 180.0),
                        p.Y + r * Math.Sin(startAngle * Math.PI / 180.0));
                    var endPointOnCircle = new Point(
                        p.X + r * Math.Cos((startAngle + sweepAngle) * Math.PI / 180.0),
                        p.Y + r * Math.Sin((startAngle + sweepAngle) * Math.PI / 180.0));

                    var pathGeometry = new PathGeometry();
                    var pathFigure = new PathFigure();

                    // 1. Set the start point (center of the pie)
                    Point startPoint = p;
                    pathFigure.StartPoint = startPoint;

                    // 2. Add a line to the arc's start point
                    Point arcStartPoint = startPointOnCircle;
                    LineSegment lineToArcStart = new LineSegment(arcStartPoint, true);
                    pathFigure.Segments.Add(lineToArcStart);

                    // 3. Add the arc segment
                    Point arcEndPoint = endPointOnCircle;
                    Size arcSize = new Size(r, r);
                    bool isLargeArc = sweepAngle > 180.0;  // Add this line
                    ArcSegment arc = new ArcSegment(arcEndPoint, arcSize, 0, isLargeArc, SweepDirection.Clockwise, true);
                    pathFigure.Segments.Add(arc);

                    // 4. Add a line segment back to the origin (center point) to close the shape
                    LineSegment lineToOrigin = new LineSegment(startPoint, true);
                    pathFigure.Segments.Add(lineToOrigin);

                    // 5. Add the figure to the geometry and set the path data
                    pathGeometry.Figures.Add(pathFigure);

                    // Create the Path element and set its properties
                    Path filledArc = new Path();
                    filledArc.Fill = colors[index % colors.Length];
                    filledArc.Stroke = TintBrush;
                    filledArc.StrokeThickness = 1;
                    filledArc.Data = pathGeometry;

                    drawingContext.DrawGeometry(filledArc.Fill, new Pen(filledArc.Stroke, filledArc.StrokeThickness), pathGeometry);

                    // Compute the text label
                    var label = Series switch
                    {
                        LabelledSeries ls => (index < ls.Labels.Count) ? ls.Labels[index] : "",
                        _ => ""
                    };
                    if (label.Trim().Length > 0)
                    {
                        // Calculate the position for the label
                        var midAngle = startAngle + sweepAngle / 2;
                        var labelRadius = r * 0.7; // position label at 70% of radius
                        var labelPoint = new Point(
                            p.X + labelRadius * Math.Cos(midAngle * Math.PI / 180.0),
                            p.Y + labelRadius * Math.Sin(midAngle * Math.PI / 180.0));
                        var percentage = (slice / total) * 100;
                        var formattedText = new FormattedText(
                            $"{label:F0} ({percentage:F1}%)",
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            new Typeface("Arial"),
                            8,
                            Brushes.Black,
                            VisualTreeHelper.GetDpi(this).PixelsPerDip);
                        labels.Add((formattedText, labelPoint));
                    }
                    // Update the starting angle for the next slice
                    startAngle += sweepAngle;
                }
                // Draw all labels
                foreach (var (text, position) in labels)
                {
                    drawingContext.DrawText(text, position);
                }
            }
        }
    }
}
