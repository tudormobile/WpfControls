using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a simple bar chart control.
/// </summary>
public class BarChart : Chart
{
    private FrameworkElement? _rootElement;

    /// <summary>
    /// Gets or sets the data series to be displayed in the chart.
    /// </summary>
    public ChartSeries Series
    {
        get { return (ChartSeries)GetValue(SeriesProperty); }
        set { SetValue(SeriesProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Series"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SeriesProperty = DependencyProperty
        .Register(nameof(Series),
        typeof(ChartSeries),
        typeof(BarChart),
        new PropertyMetadata(null, OnSeriesChanged));

    static BarChart()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BarChart), new FrameworkPropertyMetadata(typeof(BarChart)));
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
        var minWidth = 10;
        var spacing = 4;
        var top = Math.Max(this.ActualHeight - root.ActualHeight, 0) / 2;
        var left = Math.Max(this.ActualWidth - root.ActualWidth, 0) / 2;
        var allowedPoints = root.ActualWidth / (minWidth + spacing);
        if (values != null && allowedPoints > 0)
        {
            var skipCount = values.Count > allowedPoints
                ? (int)(values.Count / allowedPoints)
                : 0;
            var maxValue = values.Max();

            foreach (var (index, value) in values.Index().Where(x => x.Index % (skipCount) == 0).Index())
            {
                var xPos = left + index * (minWidth + spacing);
                if (xPos + minWidth > left + root.ActualWidth)
                {
                    break;
                }
                var height = (value.Item / maxValue) * root.ActualHeight;
                var rect = new Rect(
                    xPos,
                    top + root.ActualHeight - height,
                    minWidth,
                    height);
                drawingContext.DrawRoundedRectangle(
                    Brushes.Blue,
                    null,
                    rect, 4, 4);
            }
        }
    }

    private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BarChart chart) return;

        //// Unsubscribe from old series
        //if (e.OldValue is ChartSeries oldSeries && oldSeries.DataPoints is INotifyCollectionChanged oldCollection)
        //{
        //    oldCollection.CollectionChanged -= chart.OnDataPointsChanged;
        //}

        //// Subscribe to new series
        //if (e.NewValue is ChartSeries newSeries && newSeries.DataPoints is INotifyCollectionChanged newCollection)
        //{
        //    newCollection.CollectionChanged += chart.OnDataPointsChanged;
        //}

        // Trigger re-render
        chart.InvalidateVisual();
    }

}


