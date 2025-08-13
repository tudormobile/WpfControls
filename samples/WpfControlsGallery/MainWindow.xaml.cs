using System.Windows;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(5000);
                Dispatcher.Invoke(() =>
                {
                    testContent.Content = "CHanged!";
                });
            });
        }

        private void Card_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Tudormobile.Wpf.Controls.Card)sender).Content = DateTime.Now;
        }
    }
}