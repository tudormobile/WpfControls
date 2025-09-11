using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Tudormobile.Wpf.Behaviors;

namespace WpfControls.Tests.Behaviors;

[STATestClass, ExcludeFromCodeCoverage]
public class TextBoxExtensionTests
{
    [STATestMethod]
    public void TextBox_GetSetGlyph()
    {
        var textBox = new System.Windows.Controls.TextBox();
        Tudormobile.Wpf.Behaviors.TextBox.SetGlyph(textBox, "A");
        var glyph = Tudormobile.Wpf.Behaviors.TextBox.GetGlyph(textBox);
        Assert.AreEqual("A", glyph);
    }

    [STATestMethod]
    public void TextBox_GetSetPlaceholderText()
    {
        var textBox = new System.Windows.Controls.TextBox();
        Tudormobile.Wpf.Behaviors.TextBox.SetPlaceholderText(textBox, "Enter text...");
        var placeholderText = Tudormobile.Wpf.Behaviors.TextBox.GetPlaceholderText(textBox);
        Assert.AreEqual("Enter text...", placeholderText);
    }

    [STATestMethod]
    public void TextBox_GetSetAutoSelect()
    {
        var textBox = new System.Windows.Controls.TextBox();
        Tudormobile.Wpf.Behaviors.TextBox.SetAutoSelect(textBox, true);
        var autoSelect = Tudormobile.Wpf.Behaviors.TextBox.GetAutoSelect(textBox);
        Assert.IsTrue(autoSelect);
    }

    [STATestMethod]
    public void TextBox_GetSetAutoComplete()
    {
        var expected = new[] { "Apple", "Banana", "Cherry" };
        var textBox = new System.Windows.Controls.TextBox();
        Tudormobile.Wpf.Behaviors.TextBox.SetAutoComplete(textBox, expected);
        var actual = Tudormobile.Wpf.Behaviors.TextBox.GetAutoComplete(textBox);
        Assert.AreEqual(expected, actual);
    }

}
