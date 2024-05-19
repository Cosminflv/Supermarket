using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Commands;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Categories
{
    internal class CategoriesCRUDVM : BaseVM
    {
        public CategoriesCRUDVM()
        {
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

        private ICommand switchToAddCategoriesPageCommand;
        public ICommand SwitchToAddCategoriesPageCommand
        {
            get
            {
                if (switchToAddCategoriesPageCommand == null)
                {
                    switchToAddCategoriesPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAddCategoriesPage(); });
                }

                return switchToAddCategoriesPageCommand;
            }
        }

        private ICommand switchToModifyCategoriesPageCommand;
        public ICommand SwitchToModifyCategoriesPageCommand
        {
            get
            {
                if (switchToModifyCategoriesPageCommand == null)
                {
                    switchToModifyCategoriesPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToModifyCategoriesPage(); });
                }

                return switchToModifyCategoriesPageCommand;
            }
        }

        private ICommand switchToDeleteCategoriesPageCommand;
        public ICommand SwitchToDeleteCategoriesPageCommand
        {
            get
            {
                if (switchToDeleteCategoriesPageCommand == null)
                {
                    switchToDeleteCategoriesPageCommand = new RelayPagesCommand(o => true, o => { OnSwitchToDeleteCategoriesPage(); });
                }

                return switchToDeleteCategoriesPageCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        public delegate void SwitchToAddCategoriesPage();
        public SwitchToAddCategoriesPage OnSwitchToAddCategoriesPage { get; set; }

        public delegate void SwitchToModifyCategoriesPage();

        public SwitchToModifyCategoriesPage OnSwitchToModifyCategoriesPage { get; set; }

        public delegate void SwitchToDeleteCategoriesPage();

        public SwitchToDeleteCategoriesPage OnSwitchToDeleteCategoriesPage { get; set; }

        #endregion
    }
}
