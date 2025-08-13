namespace WpfControls.Tests
{
    [STATestClass]
    public class FontIconTests
    {
        [STATestMethod]
        public void ContructorTest()
        {
            var target = new FontIcon();
            Assert.AreEqual("Segoe Fluent Icons", target.FontFamily.Source, "Must default to Segoe Fluent Icons font.");
        }
    }
}
