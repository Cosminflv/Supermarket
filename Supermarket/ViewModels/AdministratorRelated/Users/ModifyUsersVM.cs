using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System.Linq;
using System.Collections.ObjectModel;
using Supermarket.Commands;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.ViewModels.AdministratorRelated
{
    internal class ModifyUsersVM : BaseVM
    {
        Utilizatori userOperating;
        UsersBLL usersBLL;

        Utilizatori selectedUser;

        public ObservableCollection<string> UserTypes { get; }

        public Utilizatori SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public ObservableCollection<Utilizatori> UsersList
        {
            get => usersBLL.Users;
            set => usersBLL.Users = value;
        }

        ObservableCollection<string> usersNames;
        public ObservableCollection<string> UsersNames
        {
            get
            {
                return usersNames;
            }
            set
            {
                usersNames = value;
                OnPropertyChanged("UsersNames");
            }
        }

        public ModifyUsersVM(Utilizatori user, UsersBLL userBLLParam)
        {
            userOperating = user;
            usersBLL = userBLLParam;
            UsersNames = new ObservableCollection<string>(UsersList.Select(userParsed => userParsed.NumeUtilizator).ToList());
            UserTypes = new ObservableCollection<string> { "Administrator", "Cashier" };
            SelectedUser = new Utilizatori();
        }

        private ICommand keyDownCommand;

        public ICommand KeyDownCommand
        {
            get
            {
                if (keyDownCommand == null)
                {
                    keyDownCommand = new RelaySearchCommand(KeyboardKeyDown);
                }

                return keyDownCommand;
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
                OnPropertyChanged("InfoVisibility");
            }

        }

        public void KeyboardKeyDown(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedUser = UsersList.FirstOrDefault(userParsed => userParsed.NumeUtilizator == text);
                }
            }

        }

        private ICommand modifyUserCommand;

        public ICommand ModifyUserCommand
        {
            get
            {
                if(modifyUserCommand == null)
                {
                    modifyUserCommand = new RelayCommand<object>(ModifyUser);
                }

                return modifyUserCommand;
            }
        }

        private void ModifyUser(object obj)
        {
            if (!ValidateFields())
            {
                return;
            }

            usersBLL.UpdateUser(SelectedUser);

            UpdateLocalData();
            InfoMessage = "User Modified!";
            InfoVisibility = Visibility.Visible;

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
        }

        private void UpdateLocalData()
        {
            UsersNames = new ObservableCollection<string>(UsersList.Select(userParsed => userParsed.NumeUtilizator).ToList());
            SelectedUser = new Utilizatori();
        }

        private bool ValidateFields()
        {
            if (SelectedUser.NumeUtilizator == null)
            {
                InfoMessage = "Invalid Name!";
                InfoVisibility = Visibility.Visible;
                return false;
            }
            if (SelectedUser.Parola == null)
            {
                InfoMessage = "Invalid Password!";
                InfoVisibility = Visibility.Visible;
                return false;
            }
            if (SelectedUser.TipUtilizator == null)
            {
                InfoMessage = "Invalid User Type";
                InfoVisibility = Visibility.Visible;
                return false;
            }
            return true;
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
