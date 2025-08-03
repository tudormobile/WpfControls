using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfControls.Tests
{
    [STATestClass]
    public class TitleBarTests
    {
        [STATestMethod]
        public void ConstructorTest()
        {
            var target = new TitleBar();
            // Validate default values
            Assert.IsTrue(target.ShowMinMax);
            Assert.IsTrue(target.CanMaximize);
            Assert.IsTrue(target.CanMinimize);
            Assert.IsNull(target.Icon);
        }

        [STATestMethod]
        public void DependencyPropertyTest()
        {
            var target = new TitleBar
            {
                ShowMinMax = false,
                CanMaximize = false,
                CanMinimize = false,
                Icon = new DrawingImage()
            };


            // Validate all values set
            Assert.IsFalse(target.ShowMinMax);
            Assert.IsFalse(target.CanMaximize);
            Assert.IsFalse(target.CanMinimize);
            Assert.IsNotNull(target.Icon);
        }

        [STATestMethod]
        public void OnApplyTemplateTest()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                var target = new TitleBar();
                var b = new Border() { Child = target };
                b.Measure(new Size(100, 100));
                b.Arrange(new Rect(0, 0, 100, 100));
                Assert.AreEqual(100, b.ActualHeight);
                Assert.AreEqual(100, b.ActualWidth);
            }));
        }
    }
}
