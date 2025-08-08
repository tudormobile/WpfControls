namespace WpfControls.Tests;

[STATestClass]
public class SpinnerTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var target = new Spinner();
        Assert.IsFalse(target.IsEnabled, "Default value must be false for IsEnabled property.");
    }
}
