using System.Windows;
using System.Windows.Input;

namespace Tudormobile.Wpf.Helpers;

internal static class WindowHelpers
{
    public static void DragResize(this Window window)
    {
        if (window == null) return;
        window.CaptureMouse();
        window.PreviewMouseMove += mouse_move;
        window.MouseLeftButtonUp += (s, e) =>
        {
            window.ReleaseMouseCapture();
        };
    }

    private static void mouse_move(object sender, MouseEventArgs e)
    {
        var window = sender as Window;
        if (window == null) return;
        if (!window.IsMouseCaptured || e.LeftButton != MouseButtonState.Pressed)
        {
            window.ReleaseMouseCapture();
            window.PreviewMouseMove -= mouse_move;
            return;
        }
        var pos = e.GetPosition(window);
        window.Width = Math.Max(window.MinWidth, pos.X + 16);
        window.Height = Math.Max(window.MinHeight, pos.Y + 40);
    }

}
