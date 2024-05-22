using Supermarket.Commands;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Producers
{
    internal class ProducersCRUDVM : BaseVM
    {
        Utilizatori user;

        public ProducersCRUDVM(Utilizatori userOperating)
        {
            user = userOperating;
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

        private ICommand switchToAddProducersPageCommand;
        public ICommand SwitchToAddProducersPageCommand
        {
            get
            {
                if (switchToAddProducersPageCommand == null)
                {
                    switchToAddProducersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAddProducersPage(); });
                }

                return switchToAddProducersPageCommand;
            }
        }

        private ICommand switchToModifyProducersPageCommand;
        public ICommand SwitchToModifyProducersPageCommand
        {
            get
            {
                if (switchToModifyProducersPageCommand == null)
                {
                    switchToModifyProducersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToModifyProducersPage(); });
                }

                return switchToModifyProducersPageCommand;
            }
        }

        private ICommand switchToDeleteProducersPageCommand;
        public ICommand SwitchToDeleteProducersPageCommand
        {
            get
            {
                if (switchToDeleteProducersPageCommand == null)
                {
                    switchToDeleteProducersPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToDeleteProducersPage(); });
                }

                return switchToDeleteProducersPageCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        public delegate void SwitchToAddProducersPage();
        public SwitchToAddProducersPage OnSwitchToAddProducersPage { get; set; }

        public delegate void SwitchToModifyProducersPage();

        public SwitchToModifyProducersPage OnSwitchToModifyProducersPage { get; set; }

        public delegate void SwitchToDeleteProducersPage();

        public SwitchToDeleteProducersPage OnSwitchToDeleteProducersPage { get; set; }

        #endregion
    }
}
