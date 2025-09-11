using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace WpfControls.Tests.Adorners;

[STATestClass, ExcludeFromCodeCoverage]
public class GlyphAdornerTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.GlyphAdorner(button);
        Assert.IsNotNull(adorner);
        Assert.AreSame(button, adorner.AdornedElement);
        Assert.IsFalse(adorner.IsHitTestVisible);
    }

    [STATestMethod]
    public void InvalidConstructorTest()
    {
        var uie = new UIElement();
        Assert.ThrowsExactly<ArgumentException>(() => new Tudormobile.Wpf.Adorners.GlyphAdorner(uie));
    }

    [STATestMethod]
    public void GlyphTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.GlyphAdorner(button);
        Assert.IsNull(adorner.Glyph);
        var glyph = "Test";
        adorner.Glyph = glyph;
        Assert.AreSame(glyph, adorner.Glyph);
    }

    [STATestMethod]
    public void PaddingTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.GlyphAdorner(button);
        Assert.AreEqual(new Thickness(0), adorner.Padding);
        adorner.Padding = new Thickness(5);
        Assert.AreEqual(new Thickness(5), adorner.Padding);
    }
}
