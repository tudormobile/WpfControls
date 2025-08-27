using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a button control that prompts the user for confirmation before performing the Click action and/or command.
/// <para>
/// The default confirmation dialog is a simple message box with "Yes" and "No" options. Use the ButtonText property to
/// define up to three buttons with custom text, separated by the '|' character. The first button is consider theed
/// the "affirmative" action, while the second and third buttons are considered "negative" actions. Buttons are placed
/// from the right to the left in the order they are defined.
/// </para>
/// </summary>
/// <remarks>The <see cref="ConfirmButton"/> can be used in scenarios where user confirmation is required  before
/// executing a potentially destructive or critical operation. It inherits all functionality  from the <see
/// cref="Button"/> class and can be customized further as needed.</remarks>
public class ConfirmButton : Button
{
    /// <summary>
    /// Gets or sets the text displayed on the buttons. Up to three button texts can be defined, separated by the '|' character.
    /// By default, buttons are labeld "Yes" and "No" ("Yes|No") and displayed right to left. The first letter of each button
    /// text is used as the access key (mnemonic) for that button. The first button is considered the "affirmative" action.
    /// </summary>
    public string? ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty
        .Register("ButtonText",
            typeof(string),
            typeof(ConfirmButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the confirmation message displayed to the user when the button is clicked.
    /// This message appears in the confirmation dialog and should clearly describe the action
    /// requiring user approval. If not set, the button's content or a default prompt is used.
    /// </summary>
    public string Message
    {
        get => (string)GetValue(MessageProperty) ?? "Are you sure?";
        set => SetValue(MessageProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty MessageProperty = DependencyProperty
        .Register("Message",
        typeof(string),
        typeof(ConfirmButton),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value indicating whether the operation was cancelled. This property can be used
    /// to determine if the user chose to 'cancel' rather than just 'not confirm'. It is null if the
    /// button has not been clicked yet, true if the user cancelled, and false if the user confirmed.
    /// </summary>
    public bool? WasCancelled
    {
        get => (bool?)GetValue(WasCancelledProperty);
        set => SetValue(WasCancelledProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty WasCancelledProperty = DependencyProperty
        .Register("WasCancelled",
        typeof(bool?),
        typeof(ConfirmButton),
        new PropertyMetadata(null));

    /// <summary>
    /// Occurs when the user cancels a confirmation dialog by pressing the 'cancel' button or hitting ESCAPE key.
    /// </summary>
    /// <remarks>This event is typically raised in response to user actions, such as clicking a "Cancel"
    /// button. Handlers for this event can be used to perform cleanup or rollback operations.</remarks>
    public event RoutedEventHandler UserCancelled
    {
        add { AddHandler(UserCancelledEvent, value); }
        remove { RemoveHandler(UserCancelledEvent, value); }
    }

    /// <inheritdoc/>
    public static readonly RoutedEvent UserCancelledEvent = EventManager.RegisterRoutedEvent(
        name: "UserCancelled",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(ConfirmButton));

    /// <summary>
    /// Handles the click event for the control by displaying a confirmation dialog using the current theme mode.
    /// </summary>
    /// <remarks>
    /// This method is called when the control is clicked. It first displays a confirmation dialog to the user, and,
    /// if the user confirms, it proceeds to execute the base class's OnClick method to perform the standard click
    /// action and or command.
    /// </remarks>
    protected override void OnClick()
    {
        var supressClick = false;
        var message = Message ?? "Are you sure?";
        var buttonDefinition = string.IsNullOrWhiteSpace(ButtonText) ? "Yes|No" : ButtonText;
        var buttons = buttonDefinition.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var hostWindow = Window.GetWindow(this);
        if (hostWindow != null)
        {
            var brush = hostWindow.Foreground.Clone();
            brush.Opacity = 0.10;
            var title = $"{hostWindow.Title} - Confirm";
#pragma warning disable WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var w = new Window
            {
                Title = title,
                Icon = createImage(),
                SizeToContent = SizeToContent.WidthAndHeight,
                MinHeight = 160,
                MinWidth = buttons.Length > 2 ? 400 : 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = hostWindow,
                ThemeMode = hostWindow.ThemeMode,
#pragma warning restore WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                Content = createContent(message, buttons.Take(3), brush)
            };
            w.MouseLeftButtonDown += (s, e) => w.DragMove();
            w.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    w.DialogResult = false;
                    WasCancelled = true;
                    RaiseUserCancelledRoutedEvent();
                    w.Close();
                }
                else if (e.Key == System.Windows.Input.Key.Enter)
                {
                    w.DialogResult = true;
                    WasCancelled = false;
                    w.Close();
                }
            };
            var cancelKeys = buttons.Skip(1).Select(b => b.ToLower()[0]).ToArray();
            var confirmKey = buttons.FirstOrDefault()?.ToLower()[0];
            w.TextInput += (s, e) =>
            {
                var keyText = e.Text.ToLower();
                if (keyText.Length == 1)
                {
                    var keyChar = keyText[0];
                    if (cancelKeys.Contains(keyChar))
                    {
                        w.DialogResult = false;
                        if (keyChar == 'c')
                        {
                            WasCancelled = keyChar == 'c'; // only consider it a "cancel" if the key is 'c'
                            WasCancelled = true;
                            RaiseUserCancelledRoutedEvent();
                        }
                        w.Close();
                    }
                    else if (keyChar == confirmKey)
                    {
                        w.DialogResult = true;
                        WasCancelled = false;
                        w.Close();
                    }
                }
            };
            supressClick = w.ShowDialog() != true;
        }

        // finally, if not supressed, perform the click action
        if (!supressClick) base.OnClick();
    }

    /// <summary>
    /// Raises the <see cref="UserCancelledEvent"/> routed event to notify listeners that the user has canceled an
    /// operation.
    /// </summary>
    protected void RaiseUserCancelledRoutedEvent()
    {
        RoutedEventArgs routedEventArgs = new(routedEvent: UserCancelledEvent);
        RaiseEvent(routedEventArgs);
    }

    private static BitmapSource createImage()
    {
        var ff = new FontFamily("Segoe Fluent Icons");

        FrameworkElement frameworkElement = new Grid()
        {
            Children =
            {
                new TextBlock()
                {
                    Text = "\uE91F", // full circle mask
                    FontSize = 48,
                    Width = 48,
                    Height = 48,
                    Foreground = Brushes.Blue,
                    FontFamily = ff,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
                new TextBlock()
                {
                    Text = "\uE897",  // help
                    FontSize = 36,
                    Width = 36,
                    Height = 36,
                    Foreground = Brushes.White,
                    FontFamily = ff, FontWeight = FontWeights.SemiBold,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
            }
        };


        RenderTargetBitmap renderTargetBitmap = new((int)48, (int)48, 96, 96, PixelFormats.Pbgra32);
        frameworkElement.Measure(new Size(48, 48));
        frameworkElement.Arrange(new Rect(0, 0, 48, 48));
        renderTargetBitmap.Render(frameworkElement);
        BitmapSource bitmapSource = renderTargetBitmap;
        return bitmapSource;
    }

    private static Grid createContent(string message, IEnumerable<string> buttons, Brush buttonBackground)
    {
        var tb = new TextBlock()
        {
            Text = message,
            Margin = new Thickness(10),
            TextWrapping = TextWrapping.Wrap,
        };
        var sp = new DockPanel()
        {
            Margin = new Thickness(0),
            Background = buttonBackground,
            LastChildFill = false,
        };
        foreach (var btnText in buttons)
        {
            var btn = new Button()
            {
                Content = btnText,
                Width = 90,
                MinHeight = 24,
                Margin = new Thickness(5, 10, 5, 10),
            };
            btn.Click += (s, e) =>
            {
                var w = Window.GetWindow(btn);
                if (w != null)
                {
                    w.DialogResult = btn.IsDefault;
                    w.Close();
                }
            };
            DockPanel.SetDock(btn, Dock.Right);
            sp.Children.Add(btn);
        }
        var firstButton = sp.Children.OfType<Button>().FirstOrDefault();
        if (firstButton != null)
        {
            firstButton.SetValue(Button.IsDefaultProperty, true);
            firstButton.Margin = new Thickness(5, 10, 10, 10);
        }
        var g = new Grid()
        {
            Margin = new Thickness(0),
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = GridLength.Auto }
            },
        };

        // load it up
        Grid.SetRow(tb, 0);
        Grid.SetRow(sp, 1);
        g.Children.Add(tb);
        g.Children.Add(sp);
        return g;
    }
}
