using System.Windows;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var border = new Border()
            //{
            //    CornerRadius = new CornerRadius(0, 0, 10, 10),
            //    Background = new SolidColorBrush(Color.FromArgb(0xff, 0x42, 0x42, 0x42))
            //};
            //var sb = new StatusBar() { Background = Brushes.Transparent, Foreground = Brushes.WhiteSmoke };
            //border.Child = sb;
            //sb.Items.Add(new StatusBarItem() { Content = "this is the status bar area" });

            //var appWindow = new AppWindow()
            //{
            //    Title = "This is an AppWindow instance",
            //    Width = 800,
            //    Height = 600,
            //    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 113, 96, 232)),
            //    BorderThickness = new Thickness(1),
            //    Background = new SolidColorBrush(Color.FromArgb(255, 0x1f, 0x1f, 0x1f)),
            //    Foreground = Brushes.White,
            //    StatusAreaContent = border
            //};
            //var bar = new ProgressBar() { Width = 90, Height = 20, IsIndeterminate = false, Minimum = 0, Maximum = 100, Value = 50 };
            //Tudormobile.Wpf.Behaviors.ProgressBar.SetCornerRadius(bar, new CornerRadius(10));
            //var content = new Grid();
            //content.Children.Add(bar);
            //appWindow.Content = content;
            //appWindow.Show();
        }
    }

}
