using Supermarket.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.ViewModels
{
    internal class MainWindowVM : BaseVM
    {
        private BaseVM selectedVM;

        public BaseVM SelectedVM
        {
            get { return selectedVM; }
            set { selectedVM = value; OnPropertyChanged(); }
        }

        public AdministratorMenuVM AdministratorMenuViewModel { get; set; }

        public LoginVM LoginViewModel { get; set; }

        public CashierMenuVM CashierMenuViewModel { get; set; }


        public MainWindowVM()
        {
            switchToLogin();
        }

        public void switchToLogin()
        {
            LoginViewModel = new LoginVM();
            LoginViewModel.OnLoginSuccess = (user) =>
            {
                if (user.UserType == UserType.Administrator)
                {
                    switchToAdministratorMenu(user);
                } else
                {
                    switchToCashierMenu(user);
                }
            };
            SelectedVM = LoginViewModel;
        }

        public void switchToAdministratorMenu(UserEntity user)
        {
            AdministratorMenuViewModel = new AdministratorMenuVM(user);
            AdministratorMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = AdministratorMenuViewModel;
        }

        public void switchToCashierMenu(UserEntity user)
        {
            CashierMenuViewModel = new CashierMenuVM(user);
            CashierMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = CashierMenuViewModel;
        }
    }
}
