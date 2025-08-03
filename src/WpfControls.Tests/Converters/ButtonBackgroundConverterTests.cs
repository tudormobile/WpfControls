using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace WpfControls.Tests.Converters
{
    [TestClass, ExcludeFromCodeCoverage]
    public class ButtonBackgroundConverterTests
    {
        [TestMethod]
        public void ConvertTest()
        {
            var target = new ButtonBackgroundConverter();
            var parameter = 0.123;
            var value = Brushes.Red;
            var actual = (Brush)target.Convert(value, typeof(Brush), parameter, CultureInfo.CurrentCulture)!;
            Assert.AreEqual(parameter, actual.Opacity);
        }

        [TestMethod]
        public void ConvertCacheTest()
        {
            var target = new ButtonBackgroundConverter();
            var parameter = 0.123;
            var value = Brushes.Red;
            var actual = (Brush)target.Convert(value, typeof(Brush), parameter, CultureInfo.CurrentCulture)!;
            var expected = (Brush)target.Convert(value, typeof(Brush), parameter, CultureInfo.CurrentCulture)!;
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void BadValueTest()
        {
            var target = new ButtonBackgroundConverter();
            object? actual = target.Convert(null, typeof(Brush), null, null);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ConvertWithNullParameterTest()
        {
            var target = new ButtonBackgroundConverter();
            object? parameter = null;
            var value = Brushes.Red;
            var actual = (Brush)target.Convert(value, typeof(Brush), parameter, CultureInfo.CurrentCulture)!;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0.0, actual.Opacity);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var target = new ButtonBackgroundConverter();
            Assert.ThrowsExactly<NotImplementedException>(() => target.ConvertBack(null, typeof(Brush), null, null));
        }
    }
}
