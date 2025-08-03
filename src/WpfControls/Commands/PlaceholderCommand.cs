using System.Windows.Input;

namespace Tudormobile.Wpf.Commands;

/// <summary>
/// This is placeholder command.
/// </summary>
public class PlaceholderCommand : ICommand
{
    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc/>
    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Raises the CanExecuteChanged event.
    /// </summary>
    /// <exception cref="NotImplementedException">Always raises this exception as this is only a placeholder.</exception>
    protected virtual void OnCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
