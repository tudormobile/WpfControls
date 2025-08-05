namespace WpfControls.Tests
{
    [STATestClass]
    public class WindowStateControlTests
    {
        [STATestMethod]
        public void ConstructorTest()
        {
            var target = new WindowStateControl();
            // Validate default values
            Assert.IsTrue(target.ShowMinMax);
            Assert.IsTrue(target.CanMaximize);
            Assert.IsTrue(target.CanMinimize);
            Assert.IsTrue(target.AllowDrag);
        }

        [STATestMethod]
        public void DependencyPropertyTest()
        {
            var target = new WindowStateControl
            {
                ShowMinMax = false,
                CanMaximize = false,
                CanMinimize = false,
                AllowDrag = false
            };


            // Validate all values set
            Assert.IsFalse(target.ShowMinMax);
            Assert.IsFalse(target.CanMaximize);
            Assert.IsFalse(target.CanMinimize);
            Assert.IsFalse(target.AllowDrag);
        }
    }
}
