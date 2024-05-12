using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
     class LoginVM : BaseVM
    {
        UsersBLL usersBLL = new UsersBLL();

        private string username;

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        // COMMANDS

        private ICommand loginButtonPressedCommand;

        public ICommand LoginButtonPressedCommand
        {
            get
            {
                if (loginButtonPressedCommand == null)
                {
                    loginButtonPressedCommand = new RelayCommand<object>(Login, CanLogin);
                }

                return loginButtonPressedCommand;
            }
        }

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

        public ICommand SwitchToCashierMenuCommand
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

        // METHODS

        private bool CanLogin(object parameter)
        {
            // Implement any validation logic here, e.g., checking if username and password are not empty
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void Login(object parameter)
        {
            ObservableCollection<UserEntity> allUsers = usersBLL.GetAllUsers();

            // Find the user with the entered username and password
            foreach (var user in allUsers)
            {
                if (user.Username == Username && user.Password == Password)
                {
                    // User found, perform login actions
                    if (user.UserType == UserType.Administrator)
                    {
                        //TODO: Switch to admin menu
                        SwitchToAdministratorMenuCommand.Execute(user);

                    }
                    else
                    {
                        //TODO: Switch to cashier menu
                        SwitchToCashierMenuCommand.Execute(user);
                    }
                    return;
                }
            }

            // No matching user found, display error message
        }
    }
}
