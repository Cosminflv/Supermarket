using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    class LoginVM : BaseVM
    {
        UsersBLL usersBLL;

        public LoginVM(UsersBLL usersBLLParam)
        {
            usersBLL = usersBLLParam;
            UsersList = usersBLL.GetAllUsers();
        }

        public ObservableCollection<Utilizatori> UsersList
        {
            get => usersBLL.Users;
            set => usersBLL.Users = value;
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private Visibility errorVisibility = Visibility.Collapsed;

        public Visibility ErrorVisibility
        {
            get
            {
                return errorVisibility;
            }
             set
            {
                errorVisibility = value;
                OnPropertyChanged("ErrorVisibility");
            }

        }

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

        public delegate void LoginSuccess(Utilizatori user);
        public LoginSuccess OnLoginSuccess { get; set; }

        // METHODS

        private bool CanLogin(object parameter)
        {
            // Implement any validation logic here, e.g., checking if username and password are not empty
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void Login(object parameter)
        {
            // Find the user with the entered username and password
            foreach (var user in UsersList)
            {
                if (user.NumeUtilizator == Username && user.Parola == Password)
                {
                    // User found, perform login actions
                    OnLoginSuccess?.Invoke(user);
                    ErrorMessage = "";
                    return;
                }
            }
            // No matching user found, display error message
            ErrorMessage = "Invalid username/password!";
            ErrorVisibility = Visibility.Visible;

            // Set up a timer to hide the error message after 5 seconds
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5000 milliseconds = 5 seconds
            timer.AutoReset = false; // Only fire once
            timer.Elapsed += (sender, e) =>
            {
                ErrorVisibility = Visibility.Hidden;
                ErrorMessage = "";
                timer.Dispose(); // Dispose the timer to release resources
            };
            timer.Start();
        }
    }
}
