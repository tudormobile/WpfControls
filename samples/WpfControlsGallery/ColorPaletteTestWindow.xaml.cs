using System.Windows;
using Tudormobile.Wpf.Controls;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for ColorPaletteTestWindow.xaml
    /// </summary>
    public partial class ColorPaletteTestWindow : Window
    {
        public List<ColorPalette> Palettes { get; set; }

        public ColorPaletteTestWindow()
        {
            InitializeComponent();
            Palettes = [.. Enum.GetValues<ColorPalette.ColorGroup>().Select(x => new ColorPalette(x))];
            DataContext = this;
        }
    }
}
