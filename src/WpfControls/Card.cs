using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Tudormobile.Wpf.Converters;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// A headered content control with optional footer, corner radius, and content animation.
/// <para>
/// Animation is applied to the content presenter when the content changes. A number of
/// built-in animations are available, or you can define your own.
/// </para>
/// </summary>
public class Card : HeaderedContentControl
{
    static Card()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Card), new FrameworkPropertyMetadata(typeof(Card)));
    }

    /// <summary>
    /// Gets or sets the data used for the header of each control.
    /// </summary>
    public object? Footer
    {
        get { return (object)GetValue(FooterProperty); }
        set { SetValue(FooterProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty FooterProperty = DependencyProperty
        .Register("Footer",
        typeof(object),
        typeof(Card),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets a value that indicates whether the header is null.
    /// </summary>
    public bool HasFooter
    {
        get { return (bool)GetValue(HasFooterProperty); }
        set { SetValue(HasFooterProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty HasFooterProperty = DependencyProperty
        .Register("HasFooter",
        typeof(bool),
        typeof(Card),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value that represents the degree to which the corners of a Border are rounded.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty
        .Register("CornerRadius",
        typeof(CornerRadius),
        typeof(Card),
        new PropertyMetadata(new CornerRadius(0)));

    /// <summary>
    /// Gets or sets the storyboard that defines the transition animation.
    /// </summary>
    /// <remarks>The <see cref="Storyboard"/> should be configured to define the desired visual
    /// transition effect.  Ensure that the storyboard is properly initialized and contains the necessary animations
    /// for the transition to function as expected.</remarks>
    [TypeConverter(typeof(CardTransitionTypeConverter))]
    public Storyboard Transition
    {
        get { return (Storyboard)GetValue(TransitionProperty); }
        set { SetValue(TransitionProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty TransitionProperty = DependencyProperty
        .Register("Transition",
        typeof(Storyboard),
        typeof(Card),
        new PropertyMetadata(null));

    /// <inheritdoc/>
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        if (Transition != null)
        {
            // If the new content is a UIElement, we can apply the transition.
            // This assumes that the Transition storyboard is set up to animate the ContentPresenter.
            // You may need to adjust this logic based on your specific requirements.
            if (Template?.FindName("PART_ContentPresenter", this) is ContentPresenter presenter)
            {
                Transition.Begin(presenter);
            }
            else if (newContent is FrameworkElement fe
                     && fe is UIElement ui
                     && ui.RenderTransform is TranslateTransform)
            {
                Transition.Begin(fe);
            }
        }
        base.OnContentChanged(oldContent, newContent);
    }
}

