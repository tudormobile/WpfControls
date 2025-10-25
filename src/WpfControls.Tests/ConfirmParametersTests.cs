using System.Windows.Input;
using Tudormobile.Wpf.Behaviors;

namespace WpfControls.Tests;

[STATestClass]
public class ConfirmParametersTests
{
    [TestMethod]
    public void MessageProperty_SetAndGet_Works()
    {
        var parameters = new ConfirmParameters();
        parameters.Message = "Test message";
        Assert.AreEqual("Test message", parameters.Message);
    }

    [TestMethod]
    public void ButtonTextProperty_SetAndGet_Works()
    {
        var parameters = new ConfirmParameters();
        parameters.ButtonText = "Yes|No|Cancel";
        Assert.AreEqual("Yes|No|Cancel", parameters.ButtonText);
    }

    [TestMethod]
    public void CommandProperty_SetAndGet_Works()
    {
        var parameters = new ConfirmParameters();
        var command = new RoutedCommand();
        parameters.Command = command;
        Assert.AreEqual(command, parameters.Command);
    }

    [TestMethod]
    public void CommandParameterProperty_SetAndGet_Works()
    {
        var parameters = new ConfirmParameters();
        var param = new object();
        parameters.CommandParameter = param;
        Assert.AreEqual(param, parameters.CommandParameter);
    }

    [TestMethod]
    public void WasCancelledProperty_SetAndGet_Works()
    {
        var parameters = new ConfirmParameters();
        parameters.WasCancelled = true;
        Assert.IsTrue(parameters.WasCancelled);
        parameters.WasCancelled = false;
        Assert.IsFalse(parameters.WasCancelled);
        parameters.WasCancelled = null;
        Assert.IsNull(parameters.WasCancelled);
    }

}
