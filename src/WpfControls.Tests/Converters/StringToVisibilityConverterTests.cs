using System.Windows;
using Tudormobile.Wpf.Controls.Converters;

namespace WpfControls.Tests.Converters;

[TestClass]
public class StringToVisibilityConverterTests
{
    [TestMethod]
    [DataRow(null, false, false, Visibility.Collapsed)]
    [DataRow("", false, false, Visibility.Collapsed)]
    [DataRow("test", false, false, Visibility.Visible)]
    [DataRow(null, true, false, Visibility.Visible)]
    [DataRow("", true, false, Visibility.Visible)]
    [DataRow("test", true, false, Visibility.Collapsed)]
    [DataRow(null, false, true, Visibility.Hidden)]
    [DataRow("", false, true, Visibility.Hidden)]
    [DataRow("test", false, true, Visibility.Visible)]
    [DataRow(null, true, true, Visibility.Visible)]
    [DataRow("", true, true, Visibility.Visible)]
    [DataRow("test", true, true, Visibility.Hidden)]
    public void Convert_ReturnsExpectedVisibility(string input, bool isInverted, bool useHidden, Visibility expected)
    {
        var converter = new StringToVisibilityConverter
        {
            IsInverted = isInverted,
            UseHidden = useHidden
        };

        var result = converter.Convert(input, typeof(Visibility), null, CultureInfo.InvariantCulture);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        var converter = new StringToVisibilityConverter();
        Assert.ThrowsExactly<NotImplementedException>(() =>
            converter.ConvertBack(Visibility.Visible, typeof(string), null, CultureInfo.InvariantCulture));
    }
}

