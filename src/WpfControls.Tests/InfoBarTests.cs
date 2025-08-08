namespace WpfControls.Tests;

[STATestClass]
public class InfoBarTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var target = new InfoBar();
        Assert.IsFalse(target.IsOpen, "Default value must be false for IsOpen property.");
        Assert.IsNull(target.Message);
        Assert.IsNull(target.Title);
        Assert.IsNull(target.CloseCommand);
        Assert.IsTrue(target.IsClosable);
        Assert.AreEqual(InfoBar.InfoSeverity.Informational, target.Severity);
    }

}
