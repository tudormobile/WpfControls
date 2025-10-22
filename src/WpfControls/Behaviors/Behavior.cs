using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Tudormobile.Wpf.Behaviors;

/// <summary>
/// Provides utility methods for managing attached properties and adorners for elements of type <typeparamref
/// name="T"/>.
/// </summary>
/// <remarks>This class includes methods to handle changes to attached properties, manage adorners, and ensure
/// proper updates to the visual state of elements. It is designed to simplify the creation and management of behaviors
/// that rely on attached properties and adorners.</remarks>
/// <typeparam name="T">The type of the element to which the behavior applies. Must derive from <see cref="FrameworkElement"/>.</typeparam>
public class Behavior<T> where T : FrameworkElement
{
    /// <summary>
    /// Handles changes to an attached property on a <see cref="DependencyObject"/> and invokes a specified action when
    /// the property value changes.
    /// </summary>
    /// <remarks>If the <paramref name="d"/> object is of type <typeparamref name="T"/> and is already loaded,
    /// the <paramref name="valueChanged"/> action is invoked immediately. Otherwise, the action is deferred until the
    /// element's <see cref="FrameworkElement.Loaded"/> event occurs.</remarks>
    /// <param name="d">The <see cref="DependencyObject"/> on which the attached property has changed.</param>
    /// <param name="e">The event data containing information about the property change, including the old and new values.</param>
    /// <param name="valueChanged">An action to invoke when the property value changes. The action receives the following parameters: the element
    /// of type <typeparamref name="T"/>, the changed <see cref="DependencyProperty"/>, and a boolean indicating whether
    /// the action is triggered after the element is loaded (<see langword="true"/>) or immediately (<see
    /// langword="false"/>).</param>
    protected static void AttachedPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e,
        Action<T, DependencyProperty, bool> valueChanged
        )
    {
        if (d is T element)
        {
            if (element.IsLoaded)
            {
                valueChanged(element, e.Property, false);
            }
            else
            {
                element.Loaded += (sender, args) =>
                {
                    valueChanged((T)sender, e.Property, true);
                };
            }
        }
    }

    /// <summary>
    /// Handles changes to an attached property and updates or creates an adorner for the associated element.
    /// </summary>
    /// <remarks>This method ensures that an adorner of type <typeparamref name="TAdorner"/> is created or
    /// updated when the attached property changes. If the element is already loaded, the adorner is updated
    /// immediately. If the element is not yet loaded, the adorner is created and added to the <see
    /// cref="AdornerLayer"/> when the element's <see cref="FrameworkElement.Loaded"/> event is raised.</remarks>
    /// <typeparam name="TAdorner">The type of the adorner to be created or updated. Must derive from <see cref="Adorner"/>.</typeparam>
    /// <param name="d">The <see cref="DependencyObject"/> to which the attached property is applied. Must be of type <typeparamref
    /// name="T"/>.</param>
    /// <param name="e">The event data for the property change, containing the old and new values of the attached property.</param>
    /// <param name="updateAdorner">A callback action that updates the adorner. The action receives the element of type <typeparamref name="T"/> and
    /// the adorner of type <typeparamref name="TAdorner"/>.</param>
    protected static void AttachedPropertyChanged<TAdorner>(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e,
        Action<T, TAdorner> updateAdorner) where TAdorner : Adorner
    {
        if (d is T element)
        {
            if (element.IsLoaded)
            {
                // already loaded
                var adorner = (TAdorner)(AdornerLayer.GetAdornerLayer(element)?.GetAdorners(element).FirstOrDefault(a => a is TAdorner)
                    ?? Activator.CreateInstance(typeof(TAdorner), element))!;

                updateAdorner(element, adorner);
                adorner?.InvalidateVisual();
            }
            else
            {
                if (e.NewValue != null)
                {
                    element.Loaded += (sender, args) =>
                    {
                        Debug.WriteLine("Handling Loaded");
                        AdornerLayer layer = AdornerLayer.GetAdornerLayer((T)sender);

                        var adorner = layer?.GetAdorners(element)?.FirstOrDefault(a => a is TAdorner) as TAdorner;
                        if (adorner == null)
                        {
                            adorner = (TAdorner)Activator.CreateInstance(typeof(TAdorner), element)!;
                            layer?.Add(adorner);
                        }
                        updateAdorner(element, adorner);
                    };
                }
            }
        }
    }

    /// <summary>
    /// Invalidates the visual representation of the first adorner of the specified type applied to the given UI
    /// element.
    /// </summary>
    /// <remarks>This method retrieves the adorner layer for the specified <see cref="UIElement"/>, locates
    /// the first adorner of the specified type, and invalidates its visual state. If no adorner of the specified type
    /// is found, the method has no effect.</remarks>
    /// <typeparam name="TAdorner">The type of the adorner to invalidate. Must derive from <see cref="Adorner"/>.</typeparam>
    /// <param name="v">The <see cref="UIElement"/> to which the adorner is applied. Cannot be <see langword="null"/>.</param>
    protected static void InvalidateAdorner<TAdorner>(UIElement v) where TAdorner : Adorner
    {
        AdornerLayer.GetAdornerLayer(v)?.GetAdorners(v)?.FirstOrDefault(a => a is TAdorner)?.InvalidateVisual();
    }
}

