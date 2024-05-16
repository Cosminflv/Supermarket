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

        private void AddUser(object obj)
        {
            if (ValidateFields() && CheckUniqueName())
            {
                Utilizatori userToAdd = new Utilizatori();
                userToAdd.NumeUtilizator = Username;
                userToAdd.TipUtilizator = UserType;
                userToAdd.Parola = Password;
                userToAdd.IsActive = true;

                usersBLL.AddMethod(userToAdd);

                clearFields();
            }
        }

        private void clearFields()
        {
            Username = null;
            UserType = null;
            Password = null;
            ErrorVisibility = Visibility.Collapsed;
            ErrorMessage = string.Empty;
        }

        private bool CheckUniqueName()
        {
            ObservableCollection<Utilizatori> users = usersBLL.GetAllUsers();

            bool userExists = users.Any(user => user.NumeUtilizator == Username);

            if (userExists)
            {
                ErrorMessage = "Username already exists";
                ErrorVisibility = Visibility.Visible;
            }

            return !userExists;
        }

        private bool ValidateFields()
        {
            if (Username == "")
            {
                ErrorMessage = "Fill the username";
                ErrorVisibility = Visibility.Visible;
                return false;
            }

            if (UserType == null)
            {
                ErrorMessage = "Choose the type of user";
                ErrorVisibility = Visibility.Visible;
                return false;
            }

            if (Password == null)
            {
                ErrorMessage = "Fill the password";
                ErrorVisibility = Visibility.Visible;
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
