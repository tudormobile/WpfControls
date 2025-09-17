using System.Windows;
using System.Windows.Input;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for ConfirmButtonWindow.xaml
    /// </summary>
    public partial class ConfirmButtonWindow : Window, ICommand
    {
        public ICommand DeleteCommand { get; set; }

        public ConfirmButtonWindow()
        {
            InitializeComponent();
            DataContext = this;
            DeleteCommand = this;
        }
        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MessageBox.Show($"Deleted {parameter}");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Regular Button Click Occurred.");
        }
    }
}
