using System.ComponentModel;
using System.Windows;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DateTime _testDate = new DateTime(1964, 3, 11);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime TestDate
        {
            get => _testDate;
            set { _testDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestDate))); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private void InlineObjectEditor_Opened(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opened");
        }

        private void InlineObjectEditor_Closed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Closed");
        }

        private void InlineObjectEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Closing", "Closing", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}