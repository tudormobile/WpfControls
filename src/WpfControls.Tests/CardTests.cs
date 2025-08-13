namespace WpfControls.Tests;

[STATestClass]
public class CardTests
{
    [TestMethod]
    public void ConstructorTests()
    {
        var target = new Card();
        Assert.IsNull(target.Footer, "Default value of Footer should be null.");

    }
}
