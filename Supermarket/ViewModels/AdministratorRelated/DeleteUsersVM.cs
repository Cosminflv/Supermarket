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
    internal class DeleteUsersVM : BaseVM
    {
        Utilizatori userOperating;
        UsersBLL usersBLL;

        public DeleteUsersVM(Utilizatori user, UsersBLL usersBLLParam)
        {
            userOperating = user;
            usersBLL = usersBLLParam;
            SelectedUser = new Utilizatori();
        }

        public ObservableCollection<Utilizatori> UsersList
        {
            get => usersBLL.UsersActive;
            set => usersBLL.UsersActive = value;
        }

        Utilizatori selectedUser;
        public Utilizatori SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
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

        private ICommand deleteUserCommand;

        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                {
                    deleteUserCommand = new RelayCommand<object>(DeleteUser);
                }

                return deleteUserCommand;
            }
        }

        private void DeleteUser(object obj)
        {
            if(SelectedUser.NumeUtilizator == null)
            {
                InfoMessage = "Select an user!";
                InfoVisibility = Visibility.Visible;
            }

            usersBLL.DeleteUser(SelectedUser);

            SelectedUser = new Utilizatori();

            InfoMessage = "User Deleted!";
            InfoVisibility = Visibility.Visible;

            // Set up a timer to hide the error message after 5 seconds
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5000 milliseconds = 5 seconds
            timer.AutoReset = false; // Only fire once
            timer.Elapsed += (sender, e) =>
            {
                InfoVisibility = Visibility.Hidden;
                InfoMessage = "User Deleted!";
                timer.Dispose(); // Dispose the timer to release resources
            };
            timer.Start();
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
