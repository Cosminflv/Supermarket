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

        // DELEGATES

        public delegate void SwitchToLogin();
        public SwitchToLogin OnSwitchToLogin { get; set; }

        public delegate void SwitchToUsersCRUD();
        public SwitchToUsersCRUD OnSwitchToUsersCRUD { get; set; }
    }
}
