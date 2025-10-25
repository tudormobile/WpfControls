using System.Windows;
using System.Windows.Controls;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a button control that prompts the user for confirmation before performing the Click action and/or command.
/// <para>
/// The default confirmation dialog is a simple message box with "Yes" and "No" options. Use the ButtonText property to
/// define up to three buttons with custom text, separated by the '|' character. The first button is consider theed
/// the "affirmative" action, while the second and third buttons are considered "negative" actions. Buttons are placed
/// from the right to the left in the order they are defined.
/// </para>
/// </summary>
/// <remarks>The <see cref="ConfirmButton"/> can be used in scenarios where user confirmation is required  before
/// executing a potentially destructive or critical operation. It inherits all functionality  from the <see
/// cref="Button"/> class and can be customized further as needed.</remarks>
public class ConfirmButton : Button
{
    /// <summary>
    /// Gets or sets the text displayed on the buttons. Up to three button texts can be defined, separated by the '|' character.
    /// By default, buttons are labeld "Yes" and "No" ("Yes|No") and displayed right to left. The first letter of each button
    /// text is used as the access key (mnemonic) for that button. The first button is considered the "affirmative" action.
    /// </summary>
    public string? ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty
        .Register("ButtonText",
            typeof(string),
            typeof(ConfirmButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the confirmation message displayed to the user when the button is clicked.
    /// This message appears in the confirmation dialog and should clearly describe the action
    /// requiring user approval. If not set, the button's content or a default prompt is used.
    /// </summary>
    public string Message
    {
        get => (string)GetValue(MessageProperty) ?? "Are you sure?";
        set => SetValue(MessageProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty MessageProperty = DependencyProperty
        .Register("Message",
        typeof(string),
        typeof(ConfirmButton),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value indicating whether the operation was cancelled. This property can be used
    /// to determine if the user chose to 'cancel' rather than just 'not confirm'. It is null if the
    /// button has not been clicked yet, true if the user cancelled, and false if the user confirmed.
    /// </summary>
    public bool? WasCancelled
    {
        get => (bool?)GetValue(WasCancelledProperty);
        set => SetValue(WasCancelledProperty, value);
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty WasCancelledProperty = DependencyProperty
        .Register("WasCancelled",
        typeof(bool?),
        typeof(ConfirmButton),
        new PropertyMetadata(null));

    /// <summary>
    /// Occurs when the user cancels a confirmation dialog by pressing the 'cancel' button or hitting ESCAPE key.
    /// </summary>
    /// <remarks>This event is typically raised in response to user actions, such as clicking a "Cancel"
    /// button. Handlers for this event can be used to perform cleanup or rollback operations.</remarks>
    public event RoutedEventHandler UserCancelled
    {
        add { AddHandler(UserCancelledEvent, value); }
        remove { RemoveHandler(UserCancelledEvent, value); }
    }

    /// <inheritdoc/>
    public static readonly RoutedEvent UserCancelledEvent = EventManager.RegisterRoutedEvent(
        name: "UserCancelled",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(ConfirmButton));

    /// <summary>
    /// Handles the click event for the control by displaying a confirmation dialog using the current theme mode.
    /// </summary>
    /// <remarks>
    /// This method is called when the control is clicked. It first displays a confirmation dialog to the user, and,
    /// if the user confirms, it proceeds to execute the base class's OnClick method to perform the standard click
    /// action and or command.
    /// </remarks>
    protected override void OnClick()
    {
        var parameters = new Behaviors.ConfirmParameters()
        {
            ButtonText = ButtonText,
            Message = Message,
            Command = Command,
            CommandParameter = CommandParameter,
        };
        var hostWindow = Window.GetWindow(this);
        var supressClick = false;

        if (hostWindow != null)
        {
            supressClick = parameters.ShowDialog(hostWindow);
            if (parameters.WasCancelled == true)
            {
                // raise the UserCancelled event
                RaiseUserCancelledRoutedEvent();
            }
        }

        // finally, if not supressed, perform the click action
        if (!supressClick) base.OnClick();
    }

    /// <summary>
    /// Raises the <see cref="UserCancelledEvent"/> routed event to notify listeners that the user has canceled an
    /// operation.
    /// </summary>
    protected void RaiseUserCancelledRoutedEvent()
    {
        RoutedEventArgs routedEventArgs = new(routedEvent: UserCancelledEvent);
        RaiseEvent(routedEventArgs);
    }

}
