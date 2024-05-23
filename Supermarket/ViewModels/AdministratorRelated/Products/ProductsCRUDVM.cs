using Supermarket.Commands;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Products
{
    internal class ProductsCRUDVM : BaseVM
    {
        Utilizatori user;

        public ProductsCRUDVM(Utilizatori userOperating)
        {
            this.user = userOperating;
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

        private ICommand switchToAddProductsPageCommand;
        public ICommand SwitchToAddProductsPageCommand
        {
            get
            {
                if (switchToAddProductsPageCommand == null)
                {
                    switchToAddProductsPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAddProductsPage(); });
                }

                return switchToAddProductsPageCommand;
            }
        }

        private ICommand switchToModifyProductsPageCommand;
        public ICommand SwitchToModifyProductsPageCommand
        {
            get
            {
                if (switchToModifyProductsPageCommand == null)
                {
                    switchToModifyProductsPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToModifyProductsPage(); });
                }

                return switchToModifyProductsPageCommand;
            }
        }

        private ICommand switchToDeleteProductsPageCommand;
        public ICommand SwitchToDeleteProductsPageCommand
        {
            get
            {
                if (switchToDeleteProductsPageCommand == null)
                {
                    switchToDeleteProductsPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToDeleteProductsPage(); });
                }

                return switchToDeleteProductsPageCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        public delegate void SwitchToAddProducersPage();
        public SwitchToAddProducersPage OnSwitchToAddProductsPage { get; set; }

        public delegate void SwitchToModifyProducersPage();

        public SwitchToModifyProducersPage OnSwitchToModifyProductsPage { get; set; }

        public delegate void SwitchToDeleteProducersPage();

        public SwitchToDeleteProducersPage OnSwitchToDeleteProductsPage { get; set; }

        #endregion


    }
}
