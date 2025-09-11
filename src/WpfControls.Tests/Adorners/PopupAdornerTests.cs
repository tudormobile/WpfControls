using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace WpfControls.Tests.Adorners;

[STATestClass, ExcludeFromCodeCoverage]
public class PopupAdornerTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.PopupAdorner(button);
        Assert.IsNotNull(adorner);
        Assert.AreSame(button, adorner.AdornedElement);
        Assert.IsTrue(adorner.IsHitTestVisible);
    }

    [STATestMethod]
    public void InvalidConstructorTest()
    {
        var uie = new UIElement();
        Assert.ThrowsExactly<ArgumentException>(() => new Tudormobile.Wpf.Adorners.PopupAdorner(uie));
    }

}
