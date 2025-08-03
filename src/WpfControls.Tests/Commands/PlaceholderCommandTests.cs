using System.Diagnostics.CodeAnalysis;

namespace WpfControls.Tests.Commands
{
    [TestClass]
    public class PlaceholderCommandTests
    {
        [TestMethod, ExcludeFromCodeCoverage]
        public void PlaceholderTest()
        {
            Assert.ThrowsExactly<NotImplementedException>(() => new PlaceholderCommand().CanExecute(null));
            Assert.ThrowsExactly<NotImplementedException>(() => new PlaceholderCommand().Execute(null));
        }

        [TestMethod]
        public void CanExecuteChangedTest()
        {
            var target = new testPlaceholderCommand();
            target.CanExecuteChanged += (sender, args) =>
            {
                Assert.AreSame(sender, target);
            };
            target.CallOnCanExecuteChanged();
        }

        [TestMethod]
        public void CanExecuteChangedWithNoHandlersTest()
        {
            var target = new testPlaceholderCommand();
            var expected = 1;
            target.CallOnCanExecuteChanged();
            var actual = target.CallOnExecuteChangedCounter;
            Assert.AreEqual(expected, actual);
        }

        class testPlaceholderCommand : PlaceholderCommand
        {
            public int CallOnExecuteChangedCounter { get; private set; }
            public void CallOnCanExecuteChanged() { CallOnExecuteChangedCounter++; OnCanExecuteChanged(); }
            protected override void OnCanExecuteChanged() => base.OnCanExecuteChanged();
        }
    }
}
