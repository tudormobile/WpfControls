using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace WpfControls.Tests.Converters;

[STATestClass, ExcludeFromCodeCoverage]
public class BooleanConverterTests
{
    [STATestMethod]
    public void Default_Convert_Behavior()
    {
        var target = new BooleanConverter();
        Assert.AreEqual(Visibility.Visible, target.Convert(true, typeof(Visibility), null, null));
        Assert.AreEqual(Visibility.Collapsed, target.Convert(false, typeof(Visibility), null, null));
    }

    [STATestMethod]
    public void ConvertWithFalseSet()
    {
        var target = new BooleanConverter() { FalseValue = Visibility.Hidden };
        Assert.AreEqual(Visibility.Visible, target.Convert(true, typeof(Visibility), null, null));
        Assert.AreEqual(Visibility.Hidden, target.Convert(false, typeof(Visibility), null, null));
    }

    [STATestMethod]
    public void ConvertWithTrueFalseBothNull()
    {
        var target = new BooleanConverter() { TrueValue = null, FalseValue = null };
        Assert.IsNull(target.Convert(true, typeof(Visibility), null, null));
        Assert.IsNull(target.Convert(false, typeof(Visibility), null, null));
    }

    [STATestMethod]
    public void ConvertWithFalseNull()
    {
        var target = new BooleanConverter() { FalseValue = null };
        Assert.AreEqual(Visibility.Visible, target.Convert(true, typeof(Visibility), null, null));
        Assert.IsNull(target.Convert(false, typeof(Visibility), null, null));
    }

    [STATestMethod]
    public void ConvertWithValueNotBoolean()
    {
        var target = new BooleanConverter();
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => target.Convert("true", typeof(Visibility), null, null));
        StringAssert.Contains(ex.Message, "The value must be a Boolean.");
    }

    [STATestMethod]
    public void ConvertBackWithTypeNotBoolean()
    {
        var target = new BooleanConverter();
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => target.ConvertBack(true, typeof(Visibility), null, null));
        StringAssert.Contains(ex.Message, "The target type must be a Boolean.");
    }

    [STATestMethod]
    public void ConvertBackWithTrueSet()
    {
        var target = new BooleanConverter() { TrueValue = 123 };
        Assert.IsTrue((bool)target.ConvertBack(123, typeof(bool), null, null));
    }

    [STATestMethod]
    public void ConvertBackWithFalseSet()
    {
        var target = new BooleanConverter() { FalseValue = 123 };
        Assert.IsFalse((bool)target.ConvertBack(123, typeof(bool), null, null));
    }

    [STATestMethod]
    public void ConvertBackWithTrueAndFalseSet()
    {
        var target = new BooleanConverter() { FalseValue = 123, TrueValue = 456 };
        Assert.IsFalse((bool)target.ConvertBack(123, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(456, typeof(bool), null, null));
    }

    [STATestMethod]
    public void ConvertAllSupportedTypes()
    {
        var target = new BooleanConverter();
        Assert.AreEqual("True", target.Convert(true, typeof(string), null, null));
        Assert.AreEqual("False", target.Convert(false, typeof(string), null, null));
        Assert.AreEqual(1, target.Convert(true, typeof(int), null, null));
        Assert.AreEqual(0, target.Convert(false, typeof(int), null, null));
        Assert.AreEqual(1.0, target.Convert(true, typeof(double), null, null));
        Assert.AreEqual(0.0, target.Convert(false, typeof(double), null, null));
        Assert.AreEqual(1f, target.Convert(true, typeof(float), null, null));
        Assert.AreEqual(0f, target.Convert(false, typeof(float), null, null));
        Assert.AreEqual(1m, target.Convert(true, typeof(decimal), null, null));
        Assert.AreEqual(0m, target.Convert(false, typeof(decimal), null, null));
        Assert.AreEqual(1L, target.Convert(true, typeof(long), null, null));
        Assert.AreEqual(0L, target.Convert(false, typeof(long), null, null));
        Assert.AreEqual((short)1, target.Convert(true, typeof(short), null, null));
        Assert.AreEqual((short)0, target.Convert(false, typeof(short), null, null));
        Assert.AreEqual((byte)1, target.Convert(true, typeof(byte), null, null));
        Assert.AreEqual((byte)0, target.Convert(false, typeof(byte), null, null));
        Assert.AreEqual('T', target.Convert(true, typeof(char), null, null));
        Assert.AreEqual('F', target.Convert(false, typeof(char), null, null));
        Assert.IsTrue((bool?)target.Convert(true, typeof(bool), null, null));
        Assert.IsFalse((bool?)target.Convert(false, typeof(bool), null, null));
    }

    [STATestMethod]
    public void ConvertUnsupportedType()
    {
        var target = new BooleanConverter();
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => target.Convert(true, typeof(DateTime), null, null));
        StringAssert.Contains(ex.Message, "Cannot convert Boolean to System.DateTime.");
    }

    [STATestMethod]
    public void ConvertBackUnsupportedValue()
    {
        var target = new BooleanConverter();
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => target.ConvertBack(DateTime.Now, typeof(bool), null, null));
        StringAssert.Contains(ex.Message, "Cannot convert ");
    }

    [STATestMethod]
    public void ConvertBackAllSupportedTypes()
    {
        var target = new BooleanConverter();
        Assert.IsTrue((bool)target.ConvertBack(Visibility.Visible, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(Visibility.Collapsed, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(Visibility.Hidden, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack("True", typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack("False", typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(1, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(0, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(1.0, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(0.0, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(1f, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(0f, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(1m, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(0m, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(1L, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(0L, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack((short)1, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack((short)0, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack((byte)1, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack((byte)0, typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack('T', typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack('F', typeof(bool), null, null));
        Assert.IsTrue((bool)target.ConvertBack(true, typeof(bool), null, null));
        Assert.IsFalse((bool)target.ConvertBack(false, typeof(bool), null, null));
    }


}
