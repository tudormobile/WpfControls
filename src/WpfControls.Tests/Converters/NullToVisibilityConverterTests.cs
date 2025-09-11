using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace WpfControls.Tests.Converters;

[TestClass, ExcludeFromCodeCoverage]
public class NullToVisibilityConverterTests
{
    [TestMethod]
    public void Convert_Null_ReturnsCollapsed()
    {
        // Arrange
        var converter = new NullToVisibilityConverter();
        // Act
        var result = converter.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);
        // Assert
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void ConvertBackTest()
    {
        var target = new NullToVisibilityConverter();
        Assert.ThrowsExactly<NotImplementedException>(() => target.ConvertBack(null, typeof(Visibility), null, null));
    }

}
