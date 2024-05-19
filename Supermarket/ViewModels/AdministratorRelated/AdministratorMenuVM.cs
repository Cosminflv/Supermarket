using Supermarket.Commands;
using Supermarket.Models;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    class AdministratorMenuVM : BaseVM
    {
        Utilizatori user;

        public Utilizatori User
        {
            get { return user; }
            set { 
                user = value; 
                OnPropertyChanged(); 
            }
        }

        public AdministratorMenuVM() { user = null; }

        public AdministratorMenuVM(Utilizatori userOperating) { 
            User = userOperating; 
        }

        #region Navigation

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

        private ICommand switchToUsersCRUDCommand;
        public ICommand SwitchToUsersCRUDCommand
        {
            get
            {
                if (switchToUsersCRUDCommand == null)
                {
                    switchToUsersCRUDCommand = new RelayPagesCommand(o => true, o => { OnSwitchToUsersCRUD(); });
                }

                return switchToUsersCRUDCommand;
            }
        }

        private ICommand switchToCategoriesCRUDCommand;
        public ICommand SwitchToCategoriesCRUDCommand
        {
            get
            {
                if (switchToCategoriesCRUDCommand == null)
                {
                    switchToCategoriesCRUDCommand = new RelayPagesCommand(o => true, o => { OnSwitchToCategoriesCRUD(); });
                }

                return switchToCategoriesCRUDCommand;
            }
        }

        private ICommand switchToProducersCRUDCommand;
        public ICommand SwitchToProducersCRUDCommand
        {
            get
            {
                if (switchToProducersCRUDCommand == null)
                {
                    switchToProducersCRUDCommand = new RelayPagesCommand(o => true, o => { OnSwitchProducersCRUDViewModel(); });
                }

                return switchToProducersCRUDCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToLogin();
        public SwitchToLogin OnSwitchToLogin { get; set; }

        public delegate void SwitchToUsersCRUD();
        public SwitchToUsersCRUD OnSwitchToUsersCRUD { get; set; }

        public delegate void SwitchToCategoriesCRUD();

        public SwitchToCategoriesCRUD OnSwitchToCategoriesCRUD { get; set; }

        public delegate void SwitchToProducersCRUD();

        public SwitchToProducersCRUD OnSwitchProducersCRUDViewModel { get; set; }
    }
    #endregion
}
