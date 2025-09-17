using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a set of parameters used to configure and display a confirmation dialog.
/// </summary>
/// <remarks>This class provides properties for specifying the message, button text, and command behavior for a
/// confirmation dialog. It also includes a property to determine whether the operation was cancelled. The dialog can be
/// used to prompt the user for confirmation before executing an action, with support for data binding and
/// commands.</remarks>
public class ConfirmParameters : Freezable
{
    /// <inheritdoc/>
    protected override Freezable CreateInstanceCore() => new ConfirmParameters();

    /// <summary>
    /// Gets or sets the confirmation message displayed to the user when the button is clicked.
    /// This message appears in the confirmation dialog and should clearly describe the action
    /// requiring user approval. If not set, the button's content or a default prompt is used.
    /// </summary>
    public string? Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty MessageProperty = DependencyProperty
        .Register("Message",
        typeof(string),
        typeof(ConfirmParameters),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the text displayed on the buttons. Up to three button texts can be defined, separated by the '|' character.
    /// By default, buttons are labeld "Yes" and "No" ("Yes|No") and displayed right to left. The first letter of each button
    /// text is used as the access key (mnemonic) for that button. The first button is considered the "affirmative" action.
    /// </summary>
    public string? ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty
        .Register("ButtonText",
        typeof(string),
        typeof(ConfirmParameters),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the command to invoke when the user confirms.
    /// </summary>
    public ICommand? Command
    {
        get { return (ICommand?)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty CommandProperty = DependencyProperty
        .Register("Command",
        typeof(ICommand),
        typeof(ConfirmParameters),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the parameter to pass to the Command property.
    /// </summary>
    public object? CommandParameter
    {
        get { return (object?)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty
        .Register("CommandParameter",
        typeof(object),
        typeof(ConfirmParameters),
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
        typeof(ConfirmParameters),
        new PropertyMetadata(null));

    internal bool ShowDialog(Window hostWindow)
    {
        if (Command?.CanExecute(CommandParameter) == false) return false;

        var message = Message ?? "Are you sure?";
        var buttonDefinition = string.IsNullOrWhiteSpace(ButtonText) ? "Yes|No" : ButtonText;
        var buttons = buttonDefinition.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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
        var result = w.ShowDialog();
        if (result == true && WasCancelled != true)
        {
            Command?.Execute(CommandParameter);
        }
        return result ?? false;
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
