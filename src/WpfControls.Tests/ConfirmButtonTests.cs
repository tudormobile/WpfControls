namespace WpfControls.Tests
{
    [STATestClass]
    public class ConfirmButtonTests
    {
        [STATestMethod]
        public void ConfirmButton_Constructor_SetsDefaultProperties()
        {
            var button = new ConfirmButton();
            Assert.IsNull(button.WasCancelled);
            Assert.IsNull(button.ButtonText);
            Assert.AreEqual("Are you sure?", button.Message);
        }
    }
}
