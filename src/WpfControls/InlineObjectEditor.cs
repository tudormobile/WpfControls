using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Implements a ContentControl exposes an 'Editor' UI (the Content property) below a expandable
/// header. The DisplayMemberPath indicates what displays in the header. Suitable for use in
/// ItemsControls (ItemTemplates) where DataContext is automatically set to the individual item.
/// </summary>
/// <remarks>
/// Not sure if this control will remain implemented this way. It may become a HeaderedContentControl
/// in which the Header can be bound rather than the DisplayMemberPath property use. As it is, this
/// element can be tricky to wire up properly.
/// </remarks>
public class InlineObjectEditor : ContentControl
{
    private bool _skipNextOpenedEvent;
    static InlineObjectEditor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(InlineObjectEditor), new FrameworkPropertyMetadata(typeof(InlineObjectEditor)));
    }

    /// <summary>
    /// Gets or sets a path to a value on the source object to serve as the visual representation of the object.
    /// </summary>
    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty
        .Register("DisplayMemberPath",
        typeof(string),
        typeof(InlineObjectEditor),
        new PropertyMetadata(".", displayMemberPathChanged));

    /// <summary>
    /// True if editor content is 'Open' (expanded); otherwise false.
    /// </summary>
    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty
        .Register("IsOpen",
        typeof(bool),
        typeof(InlineObjectEditor),
        new PropertyMetadata(false, onIsOpenChanged));


    /// <summary>
    /// Closed Routed Event.Raised when the user completes inline editing.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
        name: "Closed",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(InlineObjectEditor));

    /// <summary>
    /// Closed Routed Event.Raised when the user completes inline editing.
    /// </summary>
    public event RoutedEventHandler Closed
    {
        add { AddHandler(ClosedEvent, value); }
        remove { RemoveHandler(ClosedEvent, value); }
    }

    /// <summary>
    /// Closed Routed Event.Raised when the user completes inline editing.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(
        name: "Opened",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(InlineObjectEditor));

    /// <summary>
    /// Closed Routed Event.Raised when the user completes inline editing.
    /// </summary>
    public event RoutedEventHandler Opened
    {
        add { AddHandler(OpenedEvent, value); }
        remove { RemoveHandler(OpenedEvent, value); }
    }

    /// <summary>
    /// Event raised when the Editor is closing. You can cancel the 'Close'
    /// after data validation (for example).
    /// </summary>
    public event CancelEventHandler? Closing;

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        // TODO: This is not the best way to implement the header area. For example,
        // the template can be changed at runtime and/or a custom template would not
        // have the TitleContent element. Work on this later.
        var title = Template.FindName("TitleContent", this);
        if (title is TextBlock textBlock)
        {
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(DisplayMemberPath)
            {
                Path = new PropertyPath(DisplayMemberPath),
                Mode = BindingMode.OneWay,
            });
        }
        base.OnApplyTemplate();
    }

    /// <summary>
    /// Raises the OnClosed event.
    /// </summary>
    protected virtual void OnClosed() => RaiseEvent(new RoutedEventArgs(ClosedEvent));

    /// <summary>
    /// Raises the OnClosingEvent. Allows cancel of the subsequent 'Close" event via the CancelEventArgs.
    /// </summary>
    /// <param name="args">Event arguments allowing for cancellation of the close.</param>
    protected virtual void OnClosing(CancelEventArgs args) => Closing?.Invoke(this, args);

    /// <summary>
    /// Raises the OnOpened event.
    /// </summary>
    protected virtual void OnOpened()
    {
        // we have to 'skip' the OpenedEvent if it is the result
        // of a close event being cancelled.
        if (!_skipNextOpenedEvent)
        {
            RaiseEvent(new RoutedEventArgs(OpenedEvent));
        }
        _skipNextOpenedEvent = false;
    }

    /// <summary>
    /// Raised when the IsOpen property is changed.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    protected virtual void OnIsOpenChanged(DependencyPropertyChangedEventArgs args)
    {
        // Override this method to handle changes to the IsOpen property
        // For example, you might want to open or close a popup or editor control here
    }

    private static void displayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InlineObjectEditor editor && e.NewValue is String path)
        {
            var title = editor.Template?.FindName("TitleContent", editor);
            if (title != null)
            {

            }
        }
    }

    private static void onIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InlineObjectEditor editor)
        {
            editor.OnIsOpenChanged(e);
            if ((bool)e.NewValue)
            {
                editor.OnOpened();
            }
            else
            {
                CancelEventArgs args = new CancelEventArgs();
                editor.OnClosing(args);
                if (args.Cancel)
                {
                    // If the closing is cancelled, revert the IsOpen property
                    editor._skipNextOpenedEvent = true; // Prevent the next Opened event from being raised
                    editor.SetCurrentValue(IsOpenProperty, true);
                    editor._skipNextOpenedEvent = false; // Reset the flag
                    return;
                }
                editor.OnClosed();
            }
        }
    }
}
