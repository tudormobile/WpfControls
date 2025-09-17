using System.Windows;
using System.Windows.Controls;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for CardTestWindow.xaml
    /// </summary>
    public partial class CardTestWindow : Window
    {
        public CardTestWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var tb = new TextBox()
            {
                Height = 24,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            Tudormobile.Wpf.Behaviors.TextBox.SetPlaceholderText(tb, "This is a TextBox");
            card.Content = tb;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            card.Content = null;
        }
    }
}
