using System.Windows;
using System.Windows.Controls.Primitives;
using Tudormobile.Wpf.Controls;

namespace Tudormobile.Wpf.Behaviors;

/// <summary>
/// Provides attached properties and methods for adding confirmation behavior to buttons.
/// </summary>
/// <remarks>The <see cref="Button"/> class defines an attached property, <see cref="ConfirmProperty"/>,  which
/// allows you to associate confirmation parameters with a button. When the property is set,  the button's <see
/// cref="ButtonBase.Click"/> event is intercepted to display a confirmation dialog  before executing the button's
/// action.</remarks>
public class Button
{
    /// <summary>
    /// Retrieves the <see cref="ConfirmParameters"/> associated with the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> from which to retrieve the confirmation parameters.</param>
    /// <returns>The <see cref="ConfirmParameters"/> associated with the specified <paramref name="obj"/>,  or <see
    /// langword="null"/> if no confirmation parameters are set.</returns>
    public static ConfirmParameters? GetConfirm(DependencyObject obj)
    {
        return (ConfirmParameters?)obj.GetValue(ConfirmProperty);
    }

    /// <summary>
    /// Sets the confirmation parameters for the specified dependency object.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to which the confirmation parameters will be applied. Cannot be <see
    /// langword="null"/>.</param>
    /// <param name="value">The <see cref="ConfirmParameters"/> to set, or <see langword="null"/> to clear the confirmation parameters.</param>
    public static void SetConfirm(DependencyObject obj, ConfirmParameters? value)
    {
        obj.SetValue(ConfirmProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty ConfirmProperty = DependencyProperty
        .RegisterAttached("Confirm",
        typeof(ConfirmParameters), typeof(Button),
        new PropertyMetadata(confirmPropertyChanged));

    private static void confirmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ButtonBase button)
        {
            button.Click -= button_Click;
            if (e.NewValue != null)
            {
                button.Click += button_Click;
            }
        }
    }

    private static void button_Click(object sender, RoutedEventArgs e)
    {
        if (GetConfirm((DependencyObject)sender) is ConfirmParameters parameters)
        {
            var hostWindow = Window.GetWindow((DependencyObject)sender);
            if (hostWindow != null)
            {
                parameters.ShowDialog(Window.GetWindow((DependencyObject)sender)!);
            }
        }
    }
}
