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
            LoginViewModel.OnSwitchToAdministratorMenu = switchToAdministratorMenu;
            LoginViewModel.OnSwitchToCashierMenu = switchToCashierMenu;
            SelectedVM = LoginViewModel;
        }

        public void switchToAdministratorMenu()
        {
            AdministratorMenuViewModel = new AdministratorMenuVM();
            AdministratorMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = AdministratorMenuViewModel;
        }

        public void switchToCashierMenu()
        {
            CashierMenuViewModel = new CashierMenuVM();
            CashierMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = CashierMenuViewModel;
        }
    }
}
