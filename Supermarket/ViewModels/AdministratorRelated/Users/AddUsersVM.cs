using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated
{
    internal class AddUsersVM : BaseVM
    {
        Utilizatori userOperating;
        UsersBLL usersBLL;

        public ObservableCollection<string> UserTypes { get; }

        public AddUsersVM(Utilizatori user, UsersBLL userBLLParam)
        {
            UserTypes = new ObservableCollection<string> { "Administrator", "Cashier" };
            userOperating = user;
            usersBLL = userBLLParam;
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

        private string userType;

        public string UserType
        {
            get
            {
                return userType;
            }
            set
            {
                userType = value;
                OnPropertyChanged("userType");
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

        private string infoMessage;
        public string InfoMessage
        {
            get => infoMessage;
            set
            {
                infoMessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }

        private Visibility infoVisibility = Visibility.Collapsed;

        public Visibility InfoVisibility
        {
            get
            {
                return infoVisibility;
            }
            set
            {
                infoVisibility = value;
                OnPropertyChanged("ErrorVisibility");
            }
        }

        private void AddUser(object obj)
        {
            if (ValidateFields())
            {
                Utilizatori userToAdd = new Utilizatori();
                userToAdd.NumeUtilizator = Username;
                userToAdd.TipUtilizator = UserType;
                userToAdd.Parola = Password;
                userToAdd.IsActive = true;

                usersBLL.AddMethod(userToAdd);

                InfoMessage = "User Added!";
                // Set up a timer to hide the error message after 5 seconds
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000; // 5000 milliseconds = 5 seconds
                timer.AutoReset = false; // Only fire once
                timer.Elapsed += (sender, e) =>
                {
                    InfoVisibility = Visibility.Hidden;
                    InfoMessage = "";
                    timer.Dispose(); // Dispose the timer to release resources
                };
                timer.Start();


                clearFields();
            }
        }

        private void clearFields()
        {
            Username = null;
            UserType = null;
            Password = null;
            InfoVisibility = Visibility.Collapsed;
            InfoMessage = string.Empty;
        }

        private bool CheckUniqueName()
        {
            ObservableCollection<Utilizatori> users = usersBLL.GetAllUsers();

            bool userExists = users.Any(user => user.NumeUtilizator == Username);

            if (userExists)
            {
                InfoMessage = "Username already exists";
                InfoVisibility = Visibility.Visible;
            }

            return !userExists;
        }

        private bool ValidateFields()
        {
            if (Username == "")
            {
                InfoMessage = "Fill the username";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if (UserType == null)
            {
                InfoMessage = "Choose the type of user";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if (Password == null)
            {
                InfoMessage = "Fill the password";
                InfoVisibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private ICommand addUserCommand;

        public ICommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                {
                    addUserCommand = new RelayCommand<object>(AddUser);
                }

                return addUserCommand;
            }
        }

        #region Navigation

        // COMMANDS

        private ICommand switchToUsersCRUDMenuCommand;

        public ICommand SwitchToUsersCRUDMenuCommand
        {
            get
            {
                if (switchToUsersCRUDMenuCommand == null)
                {
                    switchToUsersCRUDMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToUsersCRUDMenu(); });
                }

                return switchToUsersCRUDMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToUsersCRUDMenu();
        public SwitchToUsersCRUDMenu OnSwitchToUsersCRUDMenu { get; set; }

        #endregion
    }
}
