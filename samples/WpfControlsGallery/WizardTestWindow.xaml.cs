using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Tudormobile.Wpf.Controls;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for WizardTestWindow.xaml
    /// </summary>
    public partial class WizardTestWindow : Window
    {
        public WizardTestWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wizard = new Wizard();
            wizard.Title = "Test Wizard";
            wizard.Pages.Add("First Page");
            wizard.Pages.Add("Second Page");
            wizard.Pages.Add("Third Page");
            var result = wizard.ShowDialog();
            Debug.WriteLine($"Wizard result: {result}; DiaglogResult={wizard.DialogResult}");
        }

        private void ResizeGrip_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DependencyObject dobj)
            {
                var w = Window.GetWindow(dobj);
                if (w != null)
                {
                    w.DragResize();
                }
            }
        }
    }
    public static class WindowExtensions
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
}
