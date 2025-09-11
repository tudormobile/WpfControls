namespace WpfControls.Tests.Behaviors;

[STATestClass]
public class ContentControlTests
{
    [STATestMethod]
    public void ContentControl_GetSetTransition()
    {
        var contentControl = new System.Windows.Controls.ContentControl();
        var expected = new System.Windows.Media.Animation.Storyboard();
        Tudormobile.Wpf.Behaviors.ContentControl.SetTransition(contentControl, expected);
        var actual = Tudormobile.Wpf.Behaviors.ContentControl.GetTransition(contentControl);
        Assert.AreEqual(expected, actual);
    }
}
