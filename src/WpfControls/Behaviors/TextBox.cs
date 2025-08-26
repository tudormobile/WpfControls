using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Tudormobile.Wpf.Adorners;

namespace Tudormobile.Wpf.Behaviors;

/// <summary>
/// Provides attached properties and behaviors for a <see cref="System.Windows.Controls.TextBox"/> control,
/// including glyph adorners, placeholder text, and auto-select functionality.
/// </summary>
public class TextBox : Behavior<System.Windows.Controls.TextBox>
{
    /// <summary>
    /// Identifies the Glyph attached property, which displays a glyph (icon) in the text box.
    /// </summary>
    public static readonly DependencyProperty GlyphProperty = DependencyProperty
        .RegisterAttached("Glyph",
            typeof(String),
            typeof(TextBox),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) => AttachedPropertyChanged<GlyphAdorner>(d, e, updateGlyph)));

    /// <summary>
    /// Identifies the PlaceholderText attached property, which displays placeholder text in the text box when empty.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty
        .RegisterAttached("PlaceholderText",
            typeof(String),
            typeof(TextBox),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) => AttachedPropertyChanged<PlaceholderAdorner>(d, e, updatePlaceholderText)));

    /// <summary>
    /// Identifies the AutoSelect attached property, which automatically selects all text when the text box receives focus.
    /// </summary>
    public static readonly DependencyProperty AutoSelectProperty = DependencyProperty
        .RegisterAttached("AutoSelect",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(false,
            (d, e) => AttachedPropertyChanged(d, e, autoSelectChanged)));

    /// <summary>
    /// Gets the value of the Glyph attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object from which to read the property value.</param>
    /// <returns>The glyph string value.</returns>
    public static String? GetGlyph(DependencyObject obj) => (String?)obj.GetValue(GlyphProperty);

    /// <summary>
    /// Sets the value of the Glyph attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object on which to set the property value.</param>
    /// <param name="value">The glyph string value to set.</param>
    public static void SetGlyph(DependencyObject obj, String? value) => obj.SetValue(GlyphProperty, value);

    /// <summary>
    /// Gets the value of the PlaceholderText attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object from which to read the property value.</param>
    /// <returns>The placeholder text string value.</returns>
    public static String? GetPlaceholderText(DependencyObject obj) => (String)obj.GetValue(PlaceholderTextProperty);

    /// <summary>
    /// Sets the value of the PlaceholderText attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object on which to set the property value.</param>
    /// <param name="value">The placeholder text string value to set.</param>
    public static void SetPlaceholderText(DependencyObject obj, String? value) => obj.SetValue(PlaceholderTextProperty, value);

    /// <summary>
    /// Gets the value of the AutoSelect attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object from which to read the property value.</param>
    /// <returns><c>true</c> if auto-select is enabled; otherwise, <c>false</c>.</returns>
    public static bool GetAutoSelect(DependencyObject obj) => (bool)obj.GetValue(AutoSelectProperty);

    /// <summary>
    /// Sets the value of the AutoSelect attached property for the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="obj">The object on which to set the property value.</param>
    /// <param name="value"><c>true</c> to enable auto-select; otherwise, <c>false</c>.</param>
    public static void SetAutoSelect(DependencyObject obj, bool value) => obj.SetValue(AutoSelectProperty, value);

    private static void autoSelectChanged(System.Windows.Controls.TextBox box, DependencyProperty property, bool isLoading)
    {
        if (isLoading)
        {
            box.IsKeyboardFocusedChanged += tb_IsKeyboardFocusedChanged;
            box.PreviewMouseLeftButtonDown += tb_PreviewMouseLeftButtonDown;
        }
    }

    private static void tb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount > 2)
        {
            ((System.Windows.Controls.TextBox)sender).SelectAll();
            e.Handled = true;
        }
    }

    private static void tb_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is System.Windows.Controls.TextBox textBox)
        {
            if ((bool)textBox.GetValue(AutoSelectProperty))
            {
                var w = Window.GetWindow(textBox);
                if (w != null)
                {
                    _ = w.Dispatcher.InvokeAsync(new Action(() =>
                    {
                        if (Mouse.Captured == textBox)
                        {
                            textBox.ReleaseMouseCapture();
                        }
                        textBox.SelectAll();
                    }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }
    }

    private static void updateGlyph(System.Windows.Controls.TextBox textBox, GlyphAdorner adorner)
    {
        var g = GetGlyph(textBox);
        adorner.Glyph = g;
        if (adorner.Padding == new Thickness(0)) adorner.Padding = textBox.Padding;
    }

    private static void updatePlaceholderText(System.Windows.Controls.TextBox textBox, PlaceholderAdorner adorner)
    {
        var t = GetPlaceholderText(textBox);
        if (adorner.PlaceholderText == null)
        {
            textBox.TextChanged += (s, e) => InvalidateAdorner<PlaceholderAdorner>((TextBoxBase)s);
        }
        adorner.PlaceholderText = t;
    }
}

