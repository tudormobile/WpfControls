using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Tudormobile.Wpf.Adorners;

namespace Tudormobile.Wpf.Behaviors;
/// <summary>
/// Adds a 'Popup' (Menu, Popup, or arbitrary content) to existing framework elements. Frequently used to add
/// 'drop-down' menus and other content to controls such as Buttons, Images, and text.
/// <para>
/// The popup content can be a ContextMenu, Popup Control, or any content that can be hosted in a Popup control. A
/// glyph in the form of a down arrow (or right-facing arrow) is overlayed, which makes this implementation mostly
/// compatible with any control template and theme (Original, Fluent, Dark, Light, etc.). You may need to adjust
/// things like button padding, or place your control in a frame to achieve the desired visuals, but the glyph is
/// designed to work in many environemnts without having to define custome templates. As such, this behavior does
/// not contain any visuals that can be modified.
/// </para>
/// </summary>
public static class Popup
{
    /// <summary>
    /// Gets the content associated with the specified <see cref="DependencyObject"/> for a popup.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> from which to retrieve the popup content.</param>
    /// <returns>The content object associated with the specified <see cref="DependencyObject"/>. Returns <see langword="null"/>
    /// if no content is set.</returns>
    public static object GetPopupContent(DependencyObject obj) => (object)obj.GetValue(PopupContentProperty);

    /// <summary>
    /// Sets the content to be displayed in the popup associated with the specified dependency object.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to which the popup content is associated. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The content to display in the popup. This can be any object, such as a string, a UI element, or a data model.</param>
    public static void SetPopupContent(DependencyObject obj, object value) => obj.SetValue(PopupContentProperty, value);

    /// <summary>
    /// Attached property to specify popup content on an existing control. The value must be a ContextMenu, a Popup control, or
    /// a FrameworkElement. ContextMenus and Popup control are displayed as they are define. A down-arrow glyph is used in all
    /// cases except when 
    /// </summary>
    public static readonly DependencyProperty PopupContentProperty = DependencyProperty
        .RegisterAttached("PopupContent", typeof(object), typeof(Popup),
            new PropertyMetadata(null, popupContentChanged));

    private static void popupContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Can only be applied to FrameworkElements
        if (d is FrameworkElement fe)
        {
            if (!fe.IsLoaded)
            {
                // Tap-in so we can add the glyph when loaded.
                fe.Loaded += (sender, args) => addGlyph((FrameworkElement)sender);
                return;
            }
            addGlyph(fe, e.NewValue);
        }
    }

    private static void addGlyph(FrameworkElement element, object? popupContent = null)
    {
        popupContent ??= GetPopupContent(element);
        var adornerLayer = AdornerLayer.GetAdornerLayer(element);
        if (adornerLayer != null)
        {
            // Remove existing PopupAdorners
            var toRemove = adornerLayer.GetAdorners(element);
            if (toRemove != null)
            {
                foreach (var adorner in toRemove)
                {
                    if (adorner is PopupAdorner)
                    {
                        adornerLayer.Remove(adorner);
                    }
                }
            }
            // Add new PopupAdorner if PopupContent is not null
            if (popupContent != null)
            {
                var popupAdorner = new PopupAdorner(element);
                popupAdorner.PreviewMouseLeftButtonDown += popupAdorner_PreviewMouseLeftButtonDown;
                //popupAdorner.PreviewMouseLeftButtonUp += popupAdorner_MouseUp;
                adornerLayer.Add(popupAdorner);
            }
        }
    }

    private static void popupAdorner_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is PopupAdorner adorner && adorner.AdornedElement is FrameworkElement fe)
        {
            var popupContent = GetPopupContent(fe);
            if (popupContent != null)
            {
                if (popupContent is ContextMenu contextMenu)
                {
                    contextMenu.PlacementTarget = fe;
                    contextMenu.Placement = contextMenu.Placement == System.Windows.Controls.Primitives.PlacementMode.MousePoint ?
                        System.Windows.Controls.Primitives.PlacementMode.Bottom
                        : contextMenu.Placement;
                    contextMenu.IsOpen = true;
                    e.Handled = true;
                }
                else if (popupContent is System.Windows.Controls.Primitives.Popup popupControl)
                {
                    popupControl.PlacementTarget = fe;
                    popupControl.StaysOpen = false;
                    popupControl.IsOpen = true;

                    popupControl.Child.Focusable = true;
                    popupControl.Child.Focus();
                    popupControl.Child.CaptureMouse();
                    popupControl.Child.MouseUp += releaseMouseCapture;
                    popupControl.Child.MouseLeave += releaseMouseCapture;
                    e.Handled = true;
                }
                else if (popupContent is FrameworkElement element)
                {
                    // TODO: Bug is here on second open
                    var popup = (element.Parent as Border)?.Parent as System.Windows.Controls.Primitives.Popup
                        ?? new System.Windows.Controls.Primitives.Popup
                        {
                            Child = createBorder(element, fe),
                            PlacementTarget = fe,
                            Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                            IsOpen = false,
                            AllowsTransparency = true,
                            PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.Slide,
                            Focusable = true,
                        };
                    popup.IsOpen = true;
                    popup.StaysOpen = false;
                    popup.Child.Focus();
                    popup.Child.CaptureMouse();
                    e.Handled = true;
                }
            }
        }
    }

    private static Border createBorder(FrameworkElement element, FrameworkElement fe)
    {
        var result = new Border()
        {
            Child = element,
            Focusable = true,
            CornerRadius = new CornerRadius(10),
            Background = Brushes.White
        };
        result.MouseUp += releaseMouseCapture;
        result.MouseLeave += releaseMouseCapture;
        //result.Background = Window.GetWindow(fe).Background;
        return result;
    }

    private static void releaseMouseCapture(object sender, MouseEventArgs e)
    {
        if (sender is UIElement uie)
        {
            uie.MouseUp -= releaseMouseCapture;
            uie.MouseLeave -= releaseMouseCapture;
            ((UIElement)sender).ReleaseMouseCapture();
#if DEBUG
            Debug.WriteLine("Released");
#endif
        }
    }

}
