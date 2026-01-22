using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a control that displays a chart with a configurable title.
/// </summary>
/// <remarks>The Chart control can be used to visualize data in various formats. The Title property supports data
/// binding, styling, and animation through its dependency property implementation.</remarks>
public class Chart : Control
{
    private ColorPalette? _colorPalette;

    /// <summary>
    /// Gets a brush that applies the current tint color.
    /// </summary>
    /// <remarks>The returned brush is created with the current value of the Tint property. Each access
    /// returns a new SolidColorBrush instance.</remarks>
    public Brush TintBrush => (_colorPalette ??= new ColorPalette(Tint)).PrimaryBrush;

    /// <summary>
    /// Collection of brushes from the current color palette.
    /// </summary>
    protected Brush[] TintBrushes => (_colorPalette ??= new ColorPalette(Tint)).Brushes;

    /// <summary>
    /// Gets or sets the brush used to apply a color tint to the control's content.
    /// </summary>
    public ColorPalette.ColorGroup Tint
    {
        get { return (ColorPalette.ColorGroup)GetValue(TintProperty); }
        set { SetValue(TintProperty, value); _colorPalette = null; }
    }

    /// <summary>
    /// Identifies the Tint dependency property.
    /// </summary>
    public static readonly DependencyProperty TintProperty = DependencyProperty
        .Register(nameof(Tint),
        typeof(ColorPalette.ColorGroup),
        typeof(Chart), new PropertyMetadata(default));

    /// <summary>
    /// Gets or sets the data series to be displayed in the pie chart.
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
        typeof(Chart),
        new PropertyMetadata(null, SeriesChanged));

    /// <summary>
    /// Gets or sets the title of the chart.
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    /// <remarks>This enables animation, styling, binding, etc.</remarks>
    public static readonly DependencyProperty TitleProperty = DependencyProperty
        .Register(nameof(Title),
        typeof(string),
        typeof(Chart),
        new PropertyMetadata(null));

    /// <summary>
    /// The data series dependency property has changed.
    /// </summary>
    /// <param name="args">An object that contains the event data for the dependency property change.</param>
    /// <remarks>
    /// Override this method to perform custom actions when the data series changes. You do not need to call
    /// the base implementation.
    /// </remarks>
    protected virtual void OnSeriesChanged(DependencyPropertyChangedEventArgs args) { }

    /// <summary>
    /// Handles the event that occurs when the collection of data points changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the collection whose contents have changed. This value can be null.</param>
    /// <param name="e">An object that contains information about the collection change event.</param>
    protected virtual void OnDataPointsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.InvalidateVisual();
    }

    private static void SeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Chart chart) return;
        // Unsubscribe from old series
        if (e.OldValue is ChartSeries oldSeries && oldSeries.DataPoints is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= chart.OnDataPointsChanged;
        }

        // Subscribe to new series
        if (e.NewValue is ChartSeries newSeries && newSeries.DataPoints is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += chart.OnDataPointsChanged;
        }

        // Trigger re-render
        chart.InvalidateVisual();
        chart.OnSeriesChanged(e);
    }
}

/// <summary>
/// Represents a data series containing a collection of numeric data points for chart visualization.
/// </summary>
/// <remarks>
/// This is the base class for chart data series. It contains a named collection of double values
/// that can be bound to chart controls for rendering.
/// </remarks>
public class ChartSeries
{
    /// <summary>
    /// Gets or sets the name of the data series.
    /// </summary>
    /// <remarks>
    /// The name is typically used for legend labels and series identification in the chart.
    /// </remarks>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of numeric data points to be displayed in the chart.
    /// </summary>
    /// <remarks>
    /// The data points are rendered as bars, lines, or other visual elements depending on the chart type.
    /// Changes to this collection will automatically update the chart if it supports change notifications.
    /// </remarks>
    public ObservableCollection<double> DataPoints { get; set; } = [];
}

/// <summary>
/// Represents a data series containing a collection of numeric data points and corresponding labels for chart visualization.
/// </summary>
/// <remarks>
/// This class extends <see cref="ChartSeries"/> by adding a collection of labels that correspond to each data point.
/// Each label represents the category or identifier for the corresponding value in the <see cref="ChartSeries.DataPoints"/> collection.
/// Ensure the <see cref="Labels"/> collection has the same count as <see cref="ChartSeries.DataPoints"/> for proper chart rendering.
/// </remarks>
public class LabelledSeries : ChartSeries
{
    /// <summary>
    /// Gets or sets the collection of labels corresponding to each data point.
    /// </summary>
    /// <remarks>
    /// Each label represents the category or identifier for the corresponding value in the <see cref="ChartSeries.DataPoints"/> collection.
    /// Ensure this collection has the same count as <see cref="ChartSeries.DataPoints"/> for proper chart rendering.
    /// </remarks>
    public ObservableCollection<string> Labels { get; set; } = [];
}

/// <summary>
/// Represents a time-series data set that associates data points with specific time values.
/// </summary>
/// <remarks>
/// This class extends <see cref="ChartSeries"/> by adding a parallel collection of <see cref="DateTime"/> values
/// that correspond to each data point. This is useful for time-based charts where the X-axis represents time.
/// The <see cref="TimePoints"/> collection should have the same number of elements as <see cref="ChartSeries.DataPoints"/>.
/// </remarks>
public class TimeSeries : ChartSeries
{
    /// <summary>
    /// Gets or sets the collection of time points that correspond to the data points.
    /// </summary>
    /// <remarks>
    /// Each time point represents the timestamp for the corresponding value in the <see cref="ChartSeries.DataPoints"/> collection.
    /// Ensure this collection has the same count as <see cref="ChartSeries.DataPoints"/> for proper chart rendering.
    /// </remarks>
    public ObservableCollection<DateTime> TimePoints { get; set; } = [];
}

/// <summary>
/// Represents a date-series data set that associates data points with specific calendar dates.
/// </summary>
/// <remarks>
/// This class extends <see cref="ChartSeries"/> by adding a parallel collection of <see cref="DateOnly"/> values
/// that correspond to each data point. This is useful for date-based charts where the X-axis represents calendar dates
/// without time components. The <see cref="DatePoints"/> collection should have the same number of elements as
/// <see cref="ChartSeries.DataPoints"/>.
/// </remarks>
public class DateSeries : ChartSeries
{
    /// <summary>
    /// Gets or sets the collection of date points that correspond to the data points.
    /// </summary>
    /// <remarks>
    /// Each date point represents the calendar date for the corresponding value in the <see cref="ChartSeries.DataPoints"/> collection.
    /// Ensure this collection has the same count as <see cref="ChartSeries.DataPoints"/> for proper chart rendering.
    /// </remarks>
    public ObservableCollection<DateOnly> DatePoints { get; set; } = [];
}

/// <summary>
/// Represents an XY-series data set with explicit X and Y coordinate pairs.
/// </summary>
/// <remarks>
/// This class extends <see cref="ChartSeries"/> by adding a parallel collection of X-coordinate values.
/// This is useful for scatter plots, XY line charts, or any chart type that requires explicit positioning
/// on both axes. The <see cref="XPoints"/> collection should have the same number of elements as
/// <see cref="ChartSeries.DataPoints"/> (which represent the Y values).
/// </remarks>
public class XYSeries : ChartSeries
{
    /// <summary>
    /// Gets or sets the collection of X-coordinate values that correspond to the data points.
    /// </summary>
    /// <remarks>
    /// Each X point represents the horizontal position for the corresponding Y value in the
    /// <see cref="ChartSeries.DataPoints"/> collection. Ensure this collection has the same count
    /// as <see cref="ChartSeries.DataPoints"/> for proper chart rendering.
    /// </remarks>
    public ObservableCollection<double> XPoints { get; set; } = [];
}