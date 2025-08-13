using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tudormobile.Wpf.Controls
{
    /// <summary>
    /// Represents an icon that uses a glyph from the specified font. If no
    /// font is specified, it defaults to the Segoe Fluent UI font.
    /// </summary>
    public class FontIcon : Control
    {
        static FontIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIcon), new FrameworkPropertyMetadata(typeof(FontIcon)));
            FontIcon.FontFamilyProperty.OverrideMetadata(
                typeof(FontIcon),
                new FrameworkPropertyMetadata(new FontFamily("Segoe Fluent Icons")));
        }

        /// <summary>
        /// Gets or sets the character code that identifies the icon glyph.
        /// </summary>
        public string? Glyph
        {
            get { return (string?)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        /// <inheritdoc/>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
            "Glyph",
            typeof(string),
            typeof(FontIcon),
            new PropertyMetadata(null));


    }
}
