using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventMaster._Helper
{
    public class BindingCommand : ICommand
    {
        Action<object> myAction;
        public BindingCommand(Action<object> myAction)
        {
            this.myAction = myAction;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            myAction.Invoke(parameter);
        }
    }
}
