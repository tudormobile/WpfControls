using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls.Primitives;

namespace WpfControls.Tests.Converters;

[TestClass, ExcludeFromCodeCoverage]
public class PlacementToGlyphConverterTests
{
    [TestMethod]
    public void Convert_Top_ReturnsUpArrow()
    {
        // Arrange
        var converter = new PlacementToGlyphConverter();
        // Act
        var result = converter.Convert(PlacementMode.Right, typeof(string), null, CultureInfo.InvariantCulture);
        // Assert
        Assert.AreEqual("\uF08F", result);
    }

    [TestMethod]
    public void Convert_Top_ReturnsDownArrow()
    {
        // Arrange
        var converter = new PlacementToGlyphConverter();
        // Act
        var result = converter.Convert(PlacementMode.Center, typeof(string), null, CultureInfo.InvariantCulture);
        // Assert
        Assert.AreEqual("\uF08E", result);
    }

    [TestMethod]
    public void ConvertBackTest()
    {
        var target = new PlacementToGlyphConverter();
        Assert.ThrowsExactly<NotImplementedException>(() => target.ConvertBack(null, typeof(PlacementMode), null, null));
    }

}