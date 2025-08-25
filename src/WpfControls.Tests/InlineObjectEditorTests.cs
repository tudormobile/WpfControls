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
            Assert.AreEqual(InlineObjectEditor.GlyphLocations.Right, target.GlyphLocation, "Must default to (Right) for triggers to work properly.");
        }
    }
}
