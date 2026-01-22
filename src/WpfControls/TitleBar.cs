using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Provides a title bar, complete with window state and drag/move management.
/// <para>
/// This control adds a title bar with optional Icon, Minimize/Maximize buttons, a close
/// button, and 'content'. The control is implemented as a ContentControl. The content area
/// is located between the Icon and the window state control box. <see cref="Tudormobile.Wpf.Controls.WindowStateControl"/>
/// for additional information.
/// behavior to their containing Window to manage window state (normal, minimize, maximize).
/// 
/// Set the CanMinimize/CanMaximize properties to enable minimize/maximize window states. (Default is TRUE).
/// Set the ShowMinMax property to show/hide the minimize/maximize buttons. (Default is TRUE).
/// Set the Icon property to display an Icon on the left side of the Title bar. (Default is Icon property of the host window)
/// Set the Content property to display content in the Content area (Default is Title property of the host window).
/// </para>
/// </summary>
/// <remarks>
/// This control is only suitable for use in a Transparent window with custom chrome (or no chrome).
/// </remarks>
// 
public class TitleBar : ContentControl
{
    static TitleBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
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
        typeof(TitleBar),
        new PropertyMetadata(true));

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
        typeof(TitleBar),
        new PropertyMetadata(true));

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
        typeof(TitleBar),
        new PropertyMetadata(true));

    /// <summary>
    /// Icon to display.
    /// </summary>
    public ImageSource? Icon
    {
        get { return (ImageSource?)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    /// <summary>
    /// Dependency property for Drag/Move.  Default is true.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty
        .Register(nameof(Icon),
        typeof(ImageSource),
        typeof(TitleBar),
        new PropertyMetadata(null));

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var w = Window.GetWindow(this);
        if (w != null)
        {
            // Grab some properties from the hosting window
            // (if they are not explicitly set)
            Icon ??= w.Icon;
            Content ??= w.Title;
        }
    }

    /// <inheritdoc/>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (Icon != null && e.GetPosition(this).X < Icon.Width)
        {
            showControlMenu(new Point(0, this.ActualHeight));
            e.Handled = true;
        }
    }

    /// <inheritdoc/>
    protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
    {
        showControlMenu(e.GetPosition(this));
        e.Handled = true;
    }

    /// <inheritdoc/>
    private void showControlMenu(Point p)
    {
#if DEBUG
        Debug.WriteLine(p);
#endif
        var w = Window.GetWindow(this);
        if (w != null)
        {
            var h = new WindowInteropHelper(w).Handle;
            if (h != 0)
            {
                var m = GetSystemMenu(h, false);
                var screenPoint = PointToScreen(p);

                EnableMenuItem(m, SC_MAXIMIZE,
                    (w.WindowState == WindowState.Maximized || !CanMaximize)
                    ? MF_GRAYED : MF_ENABLED);

                EnableMenuItem(m, SC_RESTORE, (w.WindowState != WindowState.Maximized)
                    ? MF_GRAYED : MF_ENABLED);

                EnableMenuItem(m, SC_MINIMIZE,
                    (w.WindowState == WindowState.Minimized || !CanMinimize)
                    ? MF_GRAYED : MF_ENABLED);

                nint id = TrackPopupMenu(m, 0x102, (int)screenPoint.X, (int)screenPoint.Y, 0, h, IntPtr.Zero);
                if (id != 0)
                {
                    PostMessage(h, WM_SYSCOMMAND, (IntPtr)id, IntPtr.Zero);
                }
            }
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y,
       int nReserved, IntPtr hWnd, IntPtr prcRect);
    [DllImport("user32.dll")]
    private static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    private const int WM_SYSCOMMAND = 0x112;
    private const UInt32 MF_ENABLED = 0x00000000;
    private const UInt32 MF_GRAYED = 0x00000001;
    private const UInt32 SC_MAXIMIZE = 0xF030;
    private const UInt32 SC_MINIMIZE = 0xF020;
    private const UInt32 SC_RESTORE = 0xF120;

    [DllImport("user32.dll")]
    static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
}
