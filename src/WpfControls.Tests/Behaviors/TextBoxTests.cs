using System.Diagnostics.CodeAnalysis;

namespace WpfControls.Tests.Behaviors;

[STATestClass, ExcludeFromCodeCoverage]
public class TextBoxTests
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
}
