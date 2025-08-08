namespace WpfControls.Tests;

[STATestClass]
public class ToggleSwitchTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var target = new ToggleSwitch();
        Assert.IsNull(target.OnContent);
        Assert.IsFalse(target.HasContent);
    }
}
