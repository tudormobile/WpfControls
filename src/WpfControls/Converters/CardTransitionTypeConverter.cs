using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

namespace Tudormobile.Wpf.Converters;

/// <summary>
/// Provides a type converter to convert string representations of card transition types into corresponding animations
/// or storyboards.
/// </summary>
/// <remarks>This converter supports predefined transition types, including "SlideLeft", "SlideRight", "SlideUp",
/// "SlideDown", and "FadeIn". These transitions are represented as animations that can be used in UI elements. If an
/// unsupported transition type is provided, an <see cref="ArgumentException"/> is thrown.</remarks>
internal class CardTransitionTypeConverter : TypeConverter
{
    private static string[] _allowed = ["SlideLeft", "SlideRight", "SlideUp", "SlideDown", "FadeIn"];

    /// <summary>
    /// Determines whether the converter can convert an object of the specified type to the type of this converter.
    /// </summary>
    /// <param name="context">An optional <see cref="ITypeDescriptorContext"/> that provides a format context. This parameter can be <see
    /// langword="null"/>.</param>
    /// <param name="sourceType">The <see cref="Type"/> representing the type to evaluate for conversion.</param>
    /// <returns><see langword="true"/> if the converter can convert the specified <paramref name="sourceType"/> to the type of
    /// this converter; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Converts the specified value to a storyboard or animation based on predefined transition names.
    /// </summary>
    /// <remarks>Supported transition names include "SlideLeft", "SlideRight", "SlideUp", "SlideDown", and
    /// "FadeIn". If the value is not a string or does not match a supported transition name, the method delegates to
    /// the base implementation.</remarks>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context. This parameter can be <see
    /// langword="null"/>.</param>
    /// <param name="culture">A <see cref="CultureInfo"/> object that specifies the culture to use during conversion. This parameter can be
    /// <see langword="null"/>.</param>
    /// <param name="value">The value to convert. This must be a <see cref="string"/> representing a transition name.</param>
    /// <returns>A storyboard or animation corresponding to the specified transition name.  For example, "SlideLeft" returns a
    /// storyboard that animates the X-axis transform from 100 to 0.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is a string that does not match one of the allowed transition names.</exception>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str)
        {
            // Here you can implement logic to convert the string to a specific storyboard or animation.
            // For simplicity, we return null. You should replace this with actual conversion logic.
            return str switch
            {
                "SlideLeft" => createStoryBoard("Margin", createSlideAnimation(new Thickness(100, 0, -100, 0), new Thickness(0))),
                "SlideRight" => createStoryBoard("Margin", createSlideAnimation(new Thickness(-100, 0, 100, 0), new Thickness(0))),
                "SlideUp" => createStoryBoard("Margin", createSlideAnimation(new Thickness(0, 100, 0, -100), new Thickness(0))),
                "SlideDown" => createStoryBoard("Margin", createSlideAnimation(new Thickness(0, -100, 0, 100), new Thickness(0))),
                "FadeIn" => createStoryBoard("Opacity", new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                }),
                _ => throw new ArgumentException($"Transition must be '{string.Join('|', _allowed)}'")
            };
        }
        return base.ConvertFrom(context, culture, value);
    }
    private DoubleAnimation createDoubleAnimation(double from, double to) => new DoubleAnimation()
    {
        From = from,
        To = to,
        Duration = new Duration(TimeSpan.FromSeconds(0.75)),
        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
    };
    private ThicknessAnimation createSlideAnimation(Thickness from, Thickness to) => new ThicknessAnimation()
    {
        From = from,
        To = to,
        Duration = new Duration(TimeSpan.FromSeconds(0.5)),
        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
    };
    private Storyboard createStoryBoard(string path, Timeline child)
    {
        var storyboard = new Storyboard();
        Storyboard.SetTargetProperty(child, new PropertyPath(path));
        storyboard.Children.Add(child);
        return storyboard;
    }
}