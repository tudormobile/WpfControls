using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tudormobile.Wpf.Behaviors;

/// <summary>
/// Progress bar behaviors (attached properties).
/// <para>
/// CornerRadius will sylize pogress bar elements with a corder radius value.
/// </para>
/// </summary>
public static class ProgressBar
{
    /// <summary>
    /// Retrieves the CornerRadius attached property.
    /// </summary>
    /// <param name="dependencyObject">The object to read the attached property from.</param>
    /// <returns>Value of the attached property.</returns>
    public static CornerRadius GetCornerRadius(DependencyObject dependencyObject)
    {
        return (CornerRadius)dependencyObject.GetValue(CornerRadiusProperty);
    }

    /// <summary>
    /// Sets the CornerRadius attached property.
    /// </summary>
    /// <param name="dependencyObject">The target object for the dependency property.</param>
    /// <param name="value">Value of the attached property.</param>
    public static void SetCornerRadius(DependencyObject dependencyObject, CornerRadius value)
    {
        dependencyObject.SetValue(CornerRadiusProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty
        .RegisterAttached("CornerRadius",
        typeof(CornerRadius),
        typeof(ProgressBar),
        new PropertyMetadata(new CornerRadius(0), cornerRadiusChanged));

    private static void cornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is System.Windows.Controls.ProgressBar p)
        {
            p.SnapsToDevicePixels = true;
            applyCornerRadius(p);
            p.Loaded += (s, e) => applyCornerRadius((System.Windows.Controls.ProgressBar)s);
        }
    }

    private static void applyCornerRadius(System.Windows.Controls.ProgressBar p)
    {
        var radius = ProgressBar.GetCornerRadius(p);
        foreach (var child in enumerateChildren(p))
        {
            if (child is Rectangle r)
            {
                r.RadiusX = radius.TopLeft;
                r.RadiusY = radius.TopRight;
            }
            else if (child is Border b)
            {
                b.CornerRadius = radius;
            }
        }
    }

    private static IEnumerable<DependencyObject> enumerateChildren(DependencyObject root)
    {
        for (int index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
        {
            var child = VisualTreeHelper.GetChild(root, index);
            yield return child;
            foreach (var c in enumerateChildren(child)) yield return c;
        }
    }
}
