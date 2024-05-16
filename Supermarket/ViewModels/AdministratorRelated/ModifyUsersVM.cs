using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels.AdministratorRelated
{
    internal class ModifyUsersVM : BaseVM
    {
        Utilizatori userOperating;
        UsersBLL usersBLL;

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

        ObservableCollection<Utilizatori> users;
        public ObservableCollection<Utilizatori> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }

        public ModifyUsersVM(Utilizatori user, UsersBLL userBLLParam)
        {
            userOperating = user;
            usersBLL = userBLLParam;
            users = usersBLL.GetAllUsers();
        }
    }
}
