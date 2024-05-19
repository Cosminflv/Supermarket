using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    internal class UsersCRUDVM : BaseVM
    {
        Utilizatori user;
        UsersBLL usersBLL;

        public UsersCRUDVM(Utilizatori userOperating, UsersBLL userBLLParam)
        {
            user = userOperating;
            usersBLL = userBLLParam;
        }

        #region Navigation

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

        private ICommand switchToAddUsersPageCommand;
        public ICommand SwitchToAddUsersPageCommand
        {
            get
            {
                if (switchToAddUsersPageCommand == null)
                {
                    switchToAddUsersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAddUsersPage(); });
                }

                return switchToAddUsersPageCommand;
            }
        }

        private ICommand switchToModifyUsersPageCommand;
        public ICommand SwitchToModifyUsersPageCommand
        {
            get
            {
                if (switchToModifyUsersPageCommand == null)
                {
                    switchToModifyUsersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToModifyUsersPage(); });
                }

                return switchToModifyUsersPageCommand;
            }
        }

        private ICommand switchToDeleteUsersPageCommand;
        public ICommand SwitchToDeleteUsersPageCommand
        {
            get
            {
                if (switchToDeleteUsersPageCommand == null)
                {
                    switchToDeleteUsersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToDeleteUsersPage(); });
                }

                return switchToDeleteUsersPageCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        public delegate void SwitchToAddUsersPage();
        public SwitchToAddUsersPage OnSwitchToAddUsersPage { get; set; }

        public delegate void SwitchToModifyUsersPage();

        public SwitchToModifyUsersPage OnSwitchToModifyUsersPage { get; set; }

        public delegate void SwitchToDeleteUsersPage();

        public SwitchToDeleteUsersPage OnSwitchToDeleteUsersPage { get; set; }

        #endregion
    }
}
