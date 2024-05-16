
using Supermarket.Models;

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
                if (user.TipUtilizator == "Administrator")
                {
                    switchToAdministratorMenu(user);
                } else
                {
                    switchToCashierMenu(user);
                }
            };
            SelectedVM = LoginViewModel;
        }

        public void switchToAdministratorMenu(Utilizatori user)
        {
            AdministratorMenuViewModel = new AdministratorMenuVM(user);
            AdministratorMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = AdministratorMenuViewModel;
        }

        public void switchToCashierMenu(Utilizatori user)
        {
            CashierMenuViewModel = new CashierMenuVM(user);
            CashierMenuViewModel.OnSwitchToLogin = switchToLogin;
            SelectedVM = CashierMenuViewModel;
        }
    }
}
