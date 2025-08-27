using System.ComponentModel;
using System.Windows;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DateTime _testDate = new(1964, 3, 11);
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

        private void card_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Tudormobile.Wpf.Controls.Card)sender).Content = DateTime.Now;
        }

        private void inlineObjectEditor_Opened(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opened");
        }

        private void inlineObjectEditor_Closed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Closed");
        }

        private void inlineObjectEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Closing", "Closing", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Confirmed");
        }
    }
}