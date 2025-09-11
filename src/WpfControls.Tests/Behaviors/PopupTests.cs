using System.Windows.Controls;

namespace WpfControls.Tests.Behaviors;

[STATestClass]
public class PopupTests
{
    [STATestMethod]
    public void PopupGetSetPopupContent()
    {
        var button = new Button();
        var menu = new ContextMenu();
        Tudormobile.Wpf.Behaviors.Popup.SetPopupContent(button, menu);
        var content = Tudormobile.Wpf.Behaviors.Popup.GetPopupContent(button);
        Assert.AreEqual(menu, content);
    }
}
