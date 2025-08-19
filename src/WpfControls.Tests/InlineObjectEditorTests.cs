namespace WpfControls.Tests
{
    [STATestClass]
    public class InlineObjectEditorTests
    {
        [STATestMethod]
        public void ConstructorTest()
        {
            var target = new InlineObjectEditor();
            Assert.AreEqual(".", target.DisplayMemberPath);
            Assert.IsFalse(target.IsOpen);
        }
    }
}
