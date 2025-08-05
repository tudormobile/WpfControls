using System.Windows;
using System.Windows.Media;

namespace WpfControls.Tests;

[STATestClass]
public class AppWindowTests
{
    [STATestMethod]
    public void ConstructorTest()
    {
        var target = new AppWindow();
        // Validate default values
        Assert.IsNull(target.StatusAreaContent);
        Assert.AreEqual(SystemColors.WindowColor, ((SolidColorBrush)target.ContentBackground!).Color);
    }

}
