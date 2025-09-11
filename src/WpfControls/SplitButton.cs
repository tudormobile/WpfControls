using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a button control that provides a primary action and a secondary action to display a user-defined popup.
/// This implementation combines a PushButton with a Popup property than can either be defined as a ContextMenu or as
/// a UIElement which will be contained in a System.Windows.Controls.Popup control. The implementation is designed to 
/// work reasonably well in stylized environments without needing to define a new control template.
/// </summary>
/// <remarks>The <see cref="SplitButton"/> control is typically used when a primary action is the most common
/// operation, but additional related actions are also available via a dropdown menu. This control is styled to
/// visually distinguish the primary action from the secondary options.</remarks>
public class SplitButton : Button
{
    private ContentPresenter? _contentPresenter;
    private ToggleButton? _toggleButton;
    private Popup? _popup;
    private bool _suppressClick;
    static SplitButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
    }

    /// <summary>
    /// Content for the popup to be displayed when the dropdown arrow is clicked. This can be a ContextMenu or any UIElement.
    /// When a UIElement is provided, it will be hosted within a Popup control. The popup and context menu will be positioned
    /// according to the Placement property. The defaut value is (null), which results in no popup (or Glyph) being displayed.
    /// Note that the Glyph will also be oriented according the the Placement property.
    /// </summary>
    public UIElement? Popup
    {
        get { return (UIElement)GetValue(PopupProperty); }
        set { SetValue(PopupProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty PopupProperty = DependencyProperty
        .Register("Popup",
        typeof(UIElement),
        typeof(SplitButton),
        new PropertyMetadata(null));

    /// <summary>
    /// Placement of the Menu or Popup.
    /// </summary>
    public PlacementMode Placement
    {
        get { return (PlacementMode)GetValue(PlacementProperty); }
        set { SetValue(PlacementProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty PlacementProperty = DependencyProperty
        .Register("Placement",
        typeof(PlacementMode),
        typeof(SplitButton),
        new PropertyMetadata(PlacementMode.Bottom));

    /// <inheritdoc/>
    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        var toggleButton = _toggleButton ??= (ToggleButton)Template.FindName("PART_MainButton", this);
        var contentPresenter = _contentPresenter ??= (ContentPresenter)Template.FindName("PART_ContentPresenter", this);
        var minX = toggleButton.ActualWidth - (toggleButton.ActualWidth - contentPresenter.ActualWidth);
        var curX = Mouse.GetPosition(toggleButton).X;
        minX = toggleButton.ActualWidth - 12 - 14 - 4; // Approximate width of the arrow area

        if (toggleButton.IsChecked == true)
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
            e.Handled = true;
            return;
        }

        if (Mouse.GetPosition(this).X > minX)
        {
            if (Popup != null)
            {
                if (Popup is ContextMenu menu)
                {
                    menu.PlacementTarget = this;
                    menu.Placement = Placement;
                    menu.IsOpen = true;
                }
                else
                {
                    if (_popup == null)
                    {
                        _popup = new Popup
                        {
                            PlacementTarget = this,
                            StaysOpen = false,
                            AllowsTransparency = true,
                            PopupAnimation = PopupAnimation.Fade,
                            Child = Popup,
                        };
                        _popup.Closed += (s, args) =>
                        {
                            if (_toggleButton?.IsChecked == true)
                            {
                                _suppressClick = true;
                                _toggleButton.IsChecked = false;
                            }
                        };
                    }
                    _popup.Placement = Placement;
                    _popup.Child = Popup;
                    _popup.IsOpen = true;
                }
                e.Handled = true;
                toggleButton!.IsChecked = true;
            }
            return;
        }
        base.OnPreviewMouseLeftButtonDown(e);
    }

    /// <inheritdoc/>
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        checkToggleButton();
    }
    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        checkToggleButton();
    }

    /// <inheritdoc/>
    protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
    {
        base.OnTemplateChanged(oldTemplate, newTemplate);
        checkToggleButton();
    }

    private void checkToggleButton()
    {
        var tb = (ToggleButton)Template.FindName("PART_MainButton", this);
        if (tb == _toggleButton) return;
        _toggleButton = tb;
        tb.Unchecked += tb_Unchecked;
        tb.Checked += tb_Checked;
        if (tb.Padding.Right < 8)
        {
            tb.Padding = new Thickness(tb.Padding.Left, tb.Padding.Top, 8, tb.Padding.Bottom);
        }
        if (Popup is ContextMenu menu)
        {
            menu.Closed += (s, e) => tb.IsChecked = false;
        }
    }

    private void tb_Checked(object sender, RoutedEventArgs e)
    {
        ((ToggleButton)sender).IsChecked = _popup?.IsOpen ?? (Popup as ContextMenu)?.IsOpen ?? false;
    }

    private void tb_Unchecked(object sender, RoutedEventArgs e)
    {
        // Here we either raise the Clicked event or close the popup
        if (Popup != null)
        {
            if (Popup is ContextMenu menu && menu.IsOpen)
            {
                menu.IsOpen = false;
                return;
            }
            else if (_popup != null && _popup.IsOpen)
            {
                _popup.IsOpen = false;
                return;
            }
        }
        OnClick();
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        if (_suppressClick)
        {
            _suppressClick = false;
            return;
        }
        if (Popup is ContextMenu menu && menu.IsOpen)
        {
            return;
        }
        if (_popup != null && _popup.IsOpen)
        {
            return;
        }
        base.OnClick();
    }
}
