using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlLibrary;
public class DelegateCommand : ICommand
{
    private readonly Action? _action;

    public DelegateCommand(Action? action)
    {
        _action = action;
    }
    public void Execute(object? parameter)
    {
        if (_action is null) 
            throw new InvalidOperationException("No action provided for this command.");

        _action();
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public event EventHandler? CanExecuteChanged;
}
