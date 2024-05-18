
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.ViewModels.AdministratorRelated;

namespace Supermarket.ViewModels
{
    internal class MainWindowVM : BaseVM
    {
        private BaseVM selectedVM;

        UsersBLL usersBLL;

        public BaseVM SelectedVM
        {
            get { return selectedVM; }
            set { selectedVM = value; OnPropertyChanged(); }
        }

        #region ViewModelsDeclaration
        public AdministratorMenuVM AdministratorMenuViewModel { get; set; }

        public LoginVM LoginViewModel { get; set; }

        public CashierMenuVM CashierMenuViewModel { get; set; }

        public UsersCRUDVM UsersCRUDViewModel { get; set; }

        public AddUsersVM AddUsersViewModel { get; set; }

        public ModifyUsersVM ModifyUsersViewModel { get; set; }

        #endregion


        public MainWindowVM()
        {
            usersBLL = new UsersBLL();
            switchToLogin(usersBLL);
        }

        public void switchToLogin(UsersBLL usersBLL)
        {
            LoginViewModel = new LoginVM(usersBLL);
            LoginViewModel.OnLoginSuccess = (user) =>
            {
                if (user.TipUtilizator == "Administrator")
                {
                    switchToAdministratorMenu(user, usersBLL);
                }
                else
                {
                    switchToCashierMenu(user);
                }
            };
            SelectedVM = LoginViewModel;
        }

        public void switchToAdministratorMenu(Utilizatori user, UsersBLL usersBLL)
        {
            AdministratorMenuViewModel = new AdministratorMenuVM(user);
            AdministratorMenuViewModel.OnSwitchToLogin = () => switchToLogin(usersBLL);
            AdministratorMenuViewModel.OnSwitchToUsersCRUD = () => switchToUsersCRUDMenu(user, usersBLL);
            SelectedVM = AdministratorMenuViewModel;
        }

        public void switchToCashierMenu(Utilizatori user)
        {
            CashierMenuViewModel = new CashierMenuVM(user);
            CashierMenuViewModel.OnSwitchToLogin = () => switchToLogin(usersBLL);
            SelectedVM = CashierMenuViewModel;
        }

        public void switchToUsersCRUDMenu(Utilizatori user, UsersBLL usersBLL)
        {
            UsersCRUDViewModel = new UsersCRUDVM(user, usersBLL);
            UsersCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user, usersBLL);
            UsersCRUDViewModel.OnSwitchToAddUsersPage = () => switchToAddUsersPage(user, usersBLL);
            UsersCRUDViewModel.OnSwitchToModifyUsersPage = () => switchToModifyUsersPage(user, usersBLL);
            SelectedVM = UsersCRUDViewModel;
        }

        public void switchToAddUsersPage(Utilizatori user, UsersBLL usersBLL)
        {
            AddUsersViewModel = new AddUsersVM(user, usersBLL);
            AddUsersViewModel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user, usersBLL);
            SelectedVM = AddUsersViewModel;
        }

        public void switchToModifyUsersPage(Utilizatori user, UsersBLL usersBLL)
        {
            ModifyUsersViewModel = new ModifyUsersVM(user, usersBLL);
            ModifyUsersViewModel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user, usersBLL);
            SelectedVM = ModifyUsersViewModel;
        }
    }
}
