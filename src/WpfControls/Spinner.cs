using System.Windows;
using System.Windows.Controls;

namespace Tudormobile.Wpf.Controls
{
    /// <summary>
    /// A 'Spinner' is a content control that rotates its content continuously when IsEnabled is true.
    /// <para>
    /// The default values of IsEnabled is set to false, which also causes the Visibility to be set to Collapsed.
    /// The default value of content is a 'chasing arrow' glyph.
    /// </para>
    /// </summary>
    public class Spinner : ContentControl
    {
        static Spinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Spinner), new FrameworkPropertyMetadata(typeof(Spinner)));
        }

        /// <summary>
        /// Create and initialize a new instance.
        /// </summary>
        public Spinner()
        {
            this.IsEnabled = false;
        }
    }
}
