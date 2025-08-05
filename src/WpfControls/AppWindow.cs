using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Application Window containing a custom title bar, status bar, and simulated chrome.
/// <para>
/// The TitleBar and StausBar properties are exposed read-only to allow customization.
/// </para>
/// </summary>
public class AppWindow : Window
{
    private ResizeGrip? _resizeGrip;
    private nint? _windowHandle;
    static AppWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AppWindow), new FrameworkPropertyMetadata(typeof(AppWindow)));
    }

    /// <summary>
    /// Content for the status area (bottom) of the AppWindow. Default value is (null).
    /// <para>
    /// Typically, a status bar is placed into this area. You can wrap this in a border to maintain and
    /// adjust corner radius values.
    /// </para>
    /// </summary>
    public object? StatusAreaContent
    {
        get { return (Brush)GetValue(StatusAreaContentProperty); }
        set { SetValue(StatusAreaContentProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty StatusAreaContentProperty = DependencyProperty
        .Register("StatusAreaContent",
        typeof(object),
        typeof(AppWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Background brush for the window content area.
    /// <para>
    /// The default value is Transparent, which results in the window content having the same
    /// color as the window background.
    /// </para>
    /// </summary>
    public Brush? ContentBackground
    {
        get { return (Brush?)GetValue(ContentBackgroundProperty); }
        set { SetValue(ContentBackgroundProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty ContentBackgroundProperty = DependencyProperty
        .Register("ContentBackground",
        typeof(Brush),
        typeof(AppWindow),
        new PropertyMetadata(new SolidColorBrush(SystemColors.WindowColor)));

    /// <summary>
    /// Create and initialize a new instance.
    /// </summary>
    public AppWindow()
    {
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        WindowChrome.SetWindowChrome(this, new WindowChrome()
        {
            ResizeBorderThickness = new Thickness(6, 2, 6, 6),
            CaptionHeight = 0,
        });
        _resizeGrip = Template.FindName("resizeGrip", this) as ResizeGrip;
        if (_resizeGrip != null)
        {
            _resizeGrip.MouseLeftButtonDown += resizeGrip_MouseLeftButtonDown;
            var w = Window.GetWindow(this);
            if (w != null)
            {
                _windowHandle = new WindowInteropHelper(w).Handle;
            }
            base.OnApplyTemplate();
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    private const int WM_SYSCOMMAND = 0x112;
    private enum ResizeDirection
    {
        Left = 61441,
        Right = 61442,
        Top = 61443,
        TopLeft = 61444,
        TopRight = 61445,
        Bottom = 61446,
        BottomLeft = 61447,
        BottomRight = 61448,
    }

    private void resizeGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_windowHandle != null)
        {
            PostMessage(_windowHandle.Value, WM_SYSCOMMAND, (IntPtr)ResizeDirection.BottomRight, IntPtr.Zero);
        }
        e.Handled = true;
    }
}
