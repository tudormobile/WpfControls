using Tudormobile.Wpf.Behaviors;

namespace WpfControls.Tests.Behaviors
{
    [STATestClass]
    public class ButtonTests
    {
        [TestMethod]
        public void GetConfirm_ReturnsNull_WhenNotSet()
        {
            var btn = new System.Windows.Controls.Button();
            var result = Button.GetConfirm(btn);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SetConfirm_SetsAndGetsConfirmParameters()
        {
            var btn = new System.Windows.Controls.Button();
            var parameters = new ConfirmParameters { Message = "Test" };
            Button.SetConfirm(btn, parameters);
            var result = Button.GetConfirm(btn);
            Assert.AreEqual(parameters, result);
        }

        [TestMethod]
        public void SetConfirm_ClearsConfirmParameters_WhenSetToNull()
        {
            var btn = new System.Windows.Controls.Button();
            var parameters = new ConfirmParameters { Message = "Test" };
            Button.SetConfirm(btn, parameters);
            Button.SetConfirm(btn, null);
            var result = Button.GetConfirm(btn);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConfirmProperty_IsRegistered()
        {
            Assert.IsNotNull(Button.ConfirmProperty);
            Assert.AreEqual("Confirm", Button.ConfirmProperty.Name);
        }
    }
}
