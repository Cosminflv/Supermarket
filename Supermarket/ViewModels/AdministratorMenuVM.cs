using Supermarket.Commands;
using Supermarket.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    class AdministratorMenuVM : BaseVM
    {
        UserEntity? user;

        public UserEntity User
        {
            get { return user; }
            set { 
                user = value; 
                OnPropertyChanged(); 
            }
        }

        public AdministratorMenuVM() { user = null; }

        public AdministratorMenuVM(UserEntity userOperating) { 
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

        // DELEGATES

        public delegate void SwitchToLogin();
        public SwitchToLogin OnSwitchToLogin { get; set; }
    }
}
