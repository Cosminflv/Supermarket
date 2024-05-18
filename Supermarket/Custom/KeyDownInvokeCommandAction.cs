using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.Custom
{
    public class KeyDownInvokeCommandAction : TriggerAction<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyDownInvokeCommandAction), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(KeyDownInvokeCommandAction), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (Command != null && parameter is KeyEventArgs keyEventArgs)
            {
                var parameters = new object[] { CommandParameter, keyEventArgs };
                if (Command.CanExecute(parameters))
                {
                    Command.Execute(parameters);
                }
            }
        }
    }
}
