using Supermarket.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
     class LoginVM : BaseVM
    {
        // COMMANDS

        private ICommand switchToAdministratorMenuCommand;

        public ICommand SwitchToAdministratorMenuCommand
        {
            get
            {
                if (switchToAdministratorMenuCommand == null)
                {
                    switchToAdministratorMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAdministratorMenu(); });
                }

                return switchToAdministratorMenuCommand;
            }
        }

        private ICommand switchToCashierMenuCommand;

        public ICommand SwitchToCashierCommand
        {
            get
            {
                if (switchToCashierMenuCommand == null)
                {
                    switchToCashierMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToCashierMenu(); });
                }

                return switchToCashierMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        public delegate void SwitchToCashierMenu();
        public SwitchToCashierMenu OnSwitchToCashierMenu { get; set; }
    }
}
