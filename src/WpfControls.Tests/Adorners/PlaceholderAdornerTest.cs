namespace WpfControls.Tests.Adorners;

[STATestClass]
public class PlaceholderAdornerTest
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.PlaceholderAdorner(button);
        Assert.IsNotNull(adorner);
        Assert.AreSame(button, adorner.AdornedElement);
        Assert.IsFalse(adorner.IsHitTestVisible);
    }

    [STATestMethod]
    public void PlaceholderTextTest()
    {
        var button = new System.Windows.Controls.Button();
        var adorner = new Tudormobile.Wpf.Adorners.PlaceholderAdorner(button);
        Assert.IsNull(adorner.PlaceholderText);
        adorner.PlaceholderText = "Test";
        Assert.AreEqual("Test", adorner.PlaceholderText);
    }
}
