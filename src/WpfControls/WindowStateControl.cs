using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Tudormobile.Wpf.Helpers;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Manages window state via minimize, maximize, and close buttons.
/// <para>
/// This control adds a close button and optional Minimize/Maximize buttons and attaches
/// behavior to their containing Window to manage window state (normal, minimize, maximize).
/// It is designed to to be used on a transparent window with custom chrome (or no chrome).
/// 
/// You can set the CanMinimize/CanMaximize properties to enable minimize/maximize window states. (Default is TRUE).
/// You can set the ShowMinMax property to show/hide the minimize/maximize buttons. (Default is TRUE).
/// You can set the AllowDrag property to allow move/drag function on the host window. (Default is TRUE).
/// </para>
/// </summary>
public class WindowStateControl : Control
{
    private Button? _maxButton;
    private Button? _minButton;
    private Button? _closeButton;
    private nint _handle;
    private Window? _window;
    private Brush? _hoverBrush;
    private Brush? _hoverBrushPressed;


    static WindowStateControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowStateControl), new FrameworkPropertyMetadata(typeof(WindowStateControl)));
    }

    /// <summary>
    /// True if user can minimize (minimze button enabled).
    /// </summary>
    public bool CanMinimize
    {
        get { return (bool)GetValue(CanMinimizeProperty); }
        set { SetValue(CanMinimizeProperty, value); }
    }

    /// <summary>
    /// CanMinimize dependency property. Default is true.
    /// </summary>
    public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty
        .Register(nameof(CanMinimize),
        typeof(bool),
        typeof(WindowStateControl),
        new PropertyMetadata(true, (s, e) => ((WindowStateControl)s).setButtonStates()));

    /// <summary>
    /// True if user can maximize.
    /// </summary>
    public bool CanMaximize
    {
        get { return (bool)GetValue(CanMaximizeProperty); }
        set { SetValue(CanMaximizeProperty, value); }
    }

    /// <summary>
    /// CanMaximize dependency property. Default is true.
    /// </summary>
    public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty
        .Register(nameof(CanMaximize),
        typeof(bool),
        typeof(WindowStateControl),
        new PropertyMetadata(true, (s, e) => ((WindowStateControl)s).setButtonStates()));

    /// <summary>
    /// True if min/max buttons are present.
    /// </summary>
    public bool ShowMinMax
    {
        get { return (bool)GetValue(ShowMinMaxProperty); }
        set { SetValue(ShowMinMaxProperty, value); }
    }

    /// <summary>
    /// ShowMinMax dependency property. Default is true.
    /// </summary>
    public static readonly DependencyProperty ShowMinMaxProperty = DependencyProperty
        .Register(nameof(ShowMinMax),
        typeof(bool),
        typeof(WindowStateControl),
        new PropertyMetadata(true, (s, e) => ((WindowStateControl)s).setButtonStates()));

    /// <summary>
    /// Provide drag/move window functionality.
    /// </summary>
    public bool AllowDrag
    {
        get { return (bool)GetValue(AllowDragProperty); }
        set { SetValue(AllowDragProperty, value); }
    }

    /// <summary>
    /// Dependency property for Drag/Move.  Default is true.
    /// </summary>
    public static readonly DependencyProperty AllowDragProperty = DependencyProperty
        .Register(nameof(AllowDrag),
        typeof(bool),
        typeof(WindowStateControl),
        new PropertyMetadata(true, (s, e) => ((WindowStateControl)s).addRemoveDragHandler()));

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        _window = Window.GetWindow(this);
        if (_window != null) _handle = new WindowInteropHelper(_window).Handle;
        _maxButton = Template.FindName("maxButton", this) as Button;
        _minButton = Template.FindName("minButton", this) as Button;
        _closeButton = Template.FindName("closeButton", this) as Button;

        if (_window != null)
        {
            // Tap-into the windproc
            var source = HwndSource.FromHwnd(_handle);
            source.AddHook(wndProc);

            // Hook-up button handlers.
            if (_closeButton != null)
            {
                _closeButton.Click += (s, e) => _window.Close();
            }
            if (_minButton != null)
            {
                _minButton.Click += (s, e) => toggleMin(_window);
            }
            if (_maxButton != null)
            {
                _maxButton.Click += (s, e) => toggleMax(_window);
                _window.StateChanged += (s, e) => _maxButton.Content = ((Window)s!).WindowState == WindowState.Maximized ? "\xe923" : "\xe922";
            }
            addRemoveDragHandler();
            setButtonStates();
        }
    }

    /// <inheritdoc/>
    protected override void OnMouseLeave(MouseEventArgs e) => fixMaxButtonBackground();

    private void setButtonStates()
    {
        if (_minButton != null)
        {
            _minButton.IsEnabled = CanMinimize;
            _minButton.Visibility = ShowMinMax ? Visibility.Visible : Visibility.Collapsed;
        }
        if (_maxButton != null)
        {
            _maxButton.IsEnabled = CanMaximize;
            _maxButton.Visibility = ShowMinMax ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private void addRemoveDragHandler()
    {
        if (_window != null)
        {
            if (AllowDrag) _window.MouseLeftButtonDown += window_MouseLeftButtonDown;
            else _window.MouseLeftButtonDown -= window_MouseLeftButtonDown;
        }
    }

    private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        ((Window)sender).DragMove();
    }

    private static void toggleMin(Window w)
    {
        w.WindowState = w.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
    }

    private void toggleMax(Window w)
    {
        w.WindowState = w.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        if (_maxButton != null)
        {
            _maxButton.Content = w.WindowState == WindowState.Maximized ? "\xe923" : "\xe922";
        }
        fixMaxButtonBackground();
    }

    private Brush createHoverBrush(double? opacity = 0.3)
    {
        var b = Foreground.Clone();
        b.Opacity = opacity ?? 1;
        return b;
    }

    private void fixMaxButtonBackground()
    {
        if (_maxButton != null && _maxButton.Background != this.Background)
        {
            _maxButton.Background = this.Background;
        }
    }

    private IntPtr wndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        // Handle messages...but ONLY if max button is enabled and visible
        if (ShowMinMax && CanMaximize && _maxButton != null)
        {
            switch (msg)
            {
                case 0x02A2: // WM_NCMOUSELEAVE
                    fixMaxButtonBackground();
                    break;
                case 0x00A0: // WM_NCMOUSEMOVE
                    if (_window!.WindowState != WindowState.Minimized)
                    {
                        Point tl = _maxButton.PointToScreen(new Point(0, 0));    // top left in screen coordinates
                        Point br = _maxButton.PointToScreen(new Point(_maxButton.ActualWidth, _maxButton.ActualHeight));
                        var r = new Rect(tl, br);
                        Point screenPoint = new(loWord(lParam), hiWord(lParam));
                        if (!r.Contains(screenPoint))
                        {
                            fixMaxButtonBackground();
                        }
                    }
                    break;
                case 0x00A2:    // WM_NCLBUTTONUP
                    if (wParam == 9/*HTMAXBUTTON*/)
                    {
                        // Handle the maximize/restore action here and return 0
                        toggleMax(_window!);
                        handled = true;
                        return 0;
                    }
                    break;
                case 0x00A1: // WM_NCLBUTTONDOWN
                    if (wParam == 9/*HTMAXBUTTON*/)
                    {
                        // Handle the button down on max button
                        _maxButton.Background = _hoverBrushPressed ??= createHoverBrush(0.2);
                        handled = true;
                        return 0;
                    }
                    break;
                case 0x84: // WM_NCHITTEST:
                    {
                        if (_window!.WindowState != WindowState.Minimized)
                        {
                            Point tl = _maxButton.PointToScreen(new Point(0, 0));    // top left in screen coordinates
                            Point br = _maxButton.PointToScreen(new Point(_maxButton.ActualWidth, _maxButton.ActualHeight));
                            var r = new Rect(tl, br);
                            Point screenPoint = new(loWord(lParam), hiWord(lParam));
                            if (r.Contains(screenPoint))
                            {
                                handled = true;
                                _maxButton.Background = _hoverBrush ??= createHoverBrush(0.3);
                                return new IntPtr(9); // HTMAXBUTTON
                            }
                            fixMaxButtonBackground();
                            MouseHelpers.TRACKMOUSEEVENT tme = new();
                            tme.cbSize = (uint)Marshal.SizeOf(tme);
                            tme.dwFlags = MouseHelpers.TME_NONCLIENT | MouseHelpers.TME_LEAVE; // Track when mouse leaves
                            tme.hwndTrack = _handle; // Handle of the control to track
                            MouseHelpers.TrackMouseEvent(ref tme);
                        }
                    }
                    break;
            }
        }
        return IntPtr.Zero;
    }

    private static int loWord(IntPtr ptr) => (int)(ptr.ToInt64() & 0xFFFF);

    private static int hiWord(IntPtr ptr) => (int)(ptr.ToInt64() >> 16);

}
