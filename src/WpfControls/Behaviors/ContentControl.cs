using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tudormobile.Wpf.Behaviors;

/// <summary>
/// Content control behaviors (attached properties).
/// <para>
/// Transition property will apply a storyboard animation when the content property changes.
/// </para>
/// </summary>
public static class ContentControl
{
    /// <summary>
    /// Gets the Transition attached property.
    /// </summary>
    /// <param name="obj">Target object for the property.</param>
    /// <returns>Transition storyboard.</returns>
    public static Storyboard GetTransition(DependencyObject obj)
    {
        return (Storyboard)obj.GetValue(TransitionProperty);
    }

    /// <summary>
    /// Sets the Transition attached property.
    /// </summary>
    /// <param name="obj">Target object for the property.</param>
    /// <param name="value">Transition storyboard.</param>
    public static void SetTransition(DependencyObject obj, Storyboard value)
    {
        obj.SetValue(TransitionProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty TransitionProperty = DependencyProperty
        .RegisterAttached("Transition",
        typeof(Storyboard),
        typeof(ContentControl),
        new PropertyMetadata(null, transitionPropertyChanged));

    private static void transitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is System.Windows.Controls.ContentControl cc)
        {
            var descriptor = DependencyPropertyDescriptor
                .FromProperty(System.Windows.Controls.ContentControl.ContentProperty,
                 typeof(System.Windows.Controls.ContentControl));

            descriptor.RemoveValueChanged(cc, contentPropertyChanged);
            descriptor.AddValueChanged(cc, contentPropertyChanged);

        }
    }

    private static void contentPropertyChanged(object? sender, EventArgs e)
    {
        if (sender is System.Windows.Controls.ContentControl cc)
        {
            var presenter = cc.Template?.FindName("PART_ContentPresenter", cc) as System.Windows.Controls.ContentPresenter;
            if (presenter == null) presenter = EnumerateChildren(cc).OfType<System.Windows.Controls.ContentPresenter>().FirstOrDefault();
            var storyboard = GetTransition(cc);
            storyboard?.Begin(cc);
        }
    }

    private static IEnumerable<DependencyObject> EnumerateChildren(DependencyObject root)
    {
        for (int index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
        {
            var child = VisualTreeHelper.GetChild(root, index);
            yield return child;
            foreach (var c in EnumerateChildren(child)) yield return c;
        }
    }

}
