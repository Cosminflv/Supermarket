using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Users
{
    internal class UserSalesVM : BaseVM
    {
        Utilizatori user;
        UsersBLL usersBLL;

        public UserSalesVM(Utilizatori user, UsersBLL usersBLL)
        {
            this.user = user;
            this.usersBLL = usersBLL;
            SelectedDate = DateTime.Now;
        }

        public ObservableCollection<Utilizatori> UsersList
        {
            get => usersBLL.UsersActive;
            set => usersBLL.UsersActive = value;
        }

        Utilizatori selectedUser;

        public Utilizatori SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        System.DateTime selectedDate;

        public System.DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value; 
                OnPropertyChanged("SelectedDate");
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

        Dictionary<DateTime, decimal?> salesPerDay;

        public Dictionary<DateTime, decimal?> SalesPerDay
        {
            get
            {
                return salesPerDay;
            }
            set
            {
                salesPerDay = value;
                OnPropertyChanged("SalesPerDay");
            }
        }

        private ICommand displaySalesCommand;

        public ICommand DisplaySalesCommand
        {
            get
            {
                if (displaySalesCommand == null)
                {
                    displaySalesCommand = new RelayCommand<object>(DisplaySales);
                }

                return displaySalesCommand;
            }
        }

        private void DisplaySales(object obj)
        {
            if(SelectedUser == null)
            {
                InfoMessage = "Select an user!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            if(SelectedDate == DateTime.MinValue)
            {
                InfoMessage = "Select a date!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            SalesPerDay = usersBLL.GetSalesPerDay(SelectedUser.UtilizatorID, SelectedDate.Month, SelectedDate.Year);
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
