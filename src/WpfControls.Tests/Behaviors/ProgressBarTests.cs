using System.Windows;
using Tudormobile.Wpf.Behaviors;

namespace WpfControls.Tests.Behaviors;

[STATestClass]
public class ProgressBarTests
{
    [STATestMethod]
    public void CornetRadiusTest1()
    {
        var expected = new CornerRadius(1, 2, 3, 4);
        var obj = new DependencyObject();
        ProgressBar.SetCornerRadius(obj, expected);
        var actual = ProgressBar.GetCornerRadius(obj);
        Assert.AreEqual(expected, actual);
    }

    [STATestMethod]
    public void CornetRadiusTest2()
    {
        var expected = new CornerRadius(1, 2, 3, 4);
        var obj = new System.Windows.Controls.ProgressBar();
        ProgressBar.SetCornerRadius(obj, expected);
        var actual = ProgressBar.GetCornerRadius(obj);
        Assert.AreEqual(expected, actual);
    }

}
