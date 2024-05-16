using Supermarket.Commands;
using Supermarket.Models;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    class CashierMenuVM : BaseVM
    {
        private Utilizatori user;

        public CashierMenuVM()
        {
            user = null;
        }

        public CashierMenuVM(Utilizatori userOperating)
        {
            user = userOperating;
        }


        // COMMANDS

        private ICommand switchToLoginCommand;
        public ICommand SwitchToLoginCommand
        {
            get
            {
                if (switchToLoginCommand == null)
                {
                    switchToLoginCommand = new RelayPagesCommand(o => true, o => { OnSwitchToLogin(); });
                }

                return switchToLoginCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToLogin();
        public SwitchToLogin OnSwitchToLogin { get; set; }
    }
}
