namespace WpfControls.Tests;

[STATestClass]
public class SplitButtonTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var target = new SplitButton();
        Assert.AreEqual(System.Windows.Controls.Primitives.PlacementMode.Bottom, target.Placement);
        Assert.IsNull(target.Popup);
    }
}
