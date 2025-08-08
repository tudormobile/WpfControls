using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Displays an information bar to provide a message to the user.
/// <para>
/// Messages are marked with a severity type <seealso cref="InfoSeverity"/> and will display
/// an icon corresponding to the severity. An optional title and message text is provided. The
/// user can dismiss the bar if the IsClosable property is true, which is the default behavior.
/// If IsClosable is false, the Closed event is not raised when the information bar is programatically
/// removed.
/// </para>
/// </summary>
/// <remarks>
/// You can apply a border, background, and padding to easily stylize the message. The background
/// is used when the Informational severity is applied, otherwise it is 'tinited' green, yellow,
/// or red according to the severity. The informational background is otherwise transparent.
/// 
/// You can attach a Command or handle the Closed event to be signaled when the user dismisses the bar.
/// </remarks>
public class InfoBar : Control
{
    static InfoBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoBar), new FrameworkPropertyMetadata(typeof(InfoBar)));
    }

    /// <summary>
    /// Closed Routed Event.Raised when the user dismisses the information bar.
    /// <para>
    /// This even is not raised if the IsClosable property is set to (false). 
    /// </para>
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
        name: "Closed",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(InfoBar));

    /// <summary>
    /// Closed Routed Event.Raised when the user dismisses the information bar.
    /// <para>
    /// This even is not raised if the IsClosable property is set to (false). 
    /// </para>
    /// </summary>
    public event RoutedEventHandler Closed
    {
        add { AddHandler(ClosedEvent, value); }
        remove { RemoveHandler(ClosedEvent, value); }
    }

    /// <summary>
    /// True if the information bar is closable by the user, otherwise false.
    /// </summary>
    public bool IsClosable
    {
        get { return (bool)GetValue(IsClosableProperty); }
        set { SetValue(IsClosableProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
        name: "IsClosable",
        propertyType: typeof(bool),
        ownerType: typeof(InfoBar),
        typeMetadata: new PropertyMetadata(true));

    /// <summary>
    /// Close Command. Invoked when the user closes the information bar.
    /// </summary>
    /// <remarks>
    /// Similar to the closed event, this command is not invoked is IsClosable is false.
    /// </remarks>
    public ICommand? CloseCommand
    {
        get { return (ICommand)GetValue(CloseCommandProperty); }
        set { SetValue(CloseCommandProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
        "CloseCommand",
        typeof(ICommand),
        typeof(InfoBar),
        new PropertyMetadata(null));

    /// <summary>
    /// Severity level of the information. The information bar is stylized according to the severity level.
    /// The default value is 'Informational', which results in a minimally stylized bar including transparent
    /// background and no border (unless explicity set).
    /// </summary>
    public InfoSeverity Severity
    {
        get { return (InfoSeverity)GetValue(SeverityProperty); }
        set { SetValue(SeverityProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(
        "Severity",
        typeof(InfoSeverity),
        typeof(InfoBar),
        new PropertyMetadata(InfoSeverity.Informational));

    /// <summary>
    /// True if the information bar is currently displayed to the user (Open); otherwise False. Setting
    /// this value to false will close the information bar, however, the Closed event and command are
    /// not raised in this case. By default, the InformationBar is not open (IsOpen = false) and therefore not displayed.
    /// </summary>
    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        "IsOpen",
        typeof(bool),
        typeof(InfoBar),
        new PropertyMetadata(false, isOpenChanged));

    /// <summary>
    /// Title for the information. (Optional). The title is displayed in semi-bold text.
    /// </summary>
    public string? Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(string),
        typeof(InfoBar),
        new PropertyMetadata(null));

    /// <summary>
    /// Message for the information bar. The message is displayed following the title if there is room
    /// for both to fit on a single line, otherwise, it is displayed below the title.
    /// </summary>
    public string? Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        "Message",
        typeof(string),
        typeof(InfoBar),
        new PropertyMetadata(null));

    /// <summary>
    /// Severity of the information represented by the information bar.
    /// </summary>
    public enum InfoSeverity
    {
        /// <summary>
        /// Info only.
        /// </summary>
        Informational,
        /// <summary>
        /// Success confirmation.
        /// </summary>
        Success,
        /// <summary>
        /// Information is a warning to the user.
        /// </summary>
        Warning,
        /// <summary>
        /// Information represents an error.
        /// </summary>
        Error,

    }

    /// <summary>
    /// Raises the OnClosed event.
    /// </summary>
    protected virtual void OnClosed() => RaiseEvent(new RoutedEventArgs(ClosedEvent));

    private static void isOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InfoBar bar && bar.IsInitialized && bar.IsClosable)
        {
            if (e.NewValue is bool b && b == false)
            {
                bar.OnClosed();
                if (bar.CloseCommand?.CanExecute(null) == true) bar.CloseCommand.Execute(null);
            }
        }
    }

}
