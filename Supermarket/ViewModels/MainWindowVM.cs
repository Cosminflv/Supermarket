
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.ViewModels.AdministratorRelated;
using Supermarket.ViewModels.AdministratorRelated.Categories;

namespace Supermarket.ViewModels
{
    internal class MainWindowVM : BaseVM
    {
        private BaseVM selectedVM;

        UsersBLL usersBLL;
        CategoriesBLL categoriesBLL;

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

        public DeleteUsersVM DeleteUsersViewmodel { get; set; } 

        public CategoriesCRUDVM CategoriesCRUDViewModel { get; set; }

        public AddCategoriesVM AddCategoriesViewModel { get; set; }

        public ModifyCategoriesVM ModifyCategoriesViewModel { get; set; }  
        
        public DeleteCategoriesVM DeleteCategoriesViewModel {  get; set; }  

        #endregion


        public MainWindowVM()
        {
            usersBLL = new UsersBLL();
            categoriesBLL = new CategoriesBLL();
            switchToLogin(usersBLL);
        }

        public void switchToLogin(UsersBLL usersBLL)
        {
            LoginViewModel = new LoginVM(usersBLL);
            LoginViewModel.OnLoginSuccess = (user) =>
            {
                if (user.TipUtilizator == "Administrator")
                {
                    switchToAdministratorMenu(user);
                }
                else
                {
                    switchToCashierMenu(user);
                }
            };
            SelectedVM = LoginViewModel;
        }

        public void switchToAdministratorMenu(Utilizatori user)
        {
            AdministratorMenuViewModel = new AdministratorMenuVM(user);
            AdministratorMenuViewModel.OnSwitchToLogin = () => switchToLogin(usersBLL);
            AdministratorMenuViewModel.OnSwitchToUsersCRUD = () => switchToUsersCRUDMenu(user);
            AdministratorMenuViewModel.OnSwitchToCategoriesCRUD = () => switchToCategoriesCRUDMenu(user);
            SelectedVM = AdministratorMenuViewModel;
        }

        public void switchToCashierMenu(Utilizatori user)
        {
            CashierMenuViewModel = new CashierMenuVM(user);
            CashierMenuViewModel.OnSwitchToLogin = () => switchToLogin(usersBLL);
            SelectedVM = CashierMenuViewModel;
        }

        #region UsersCRUD

        public void switchToUsersCRUDMenu(Utilizatori user)
        {
            UsersCRUDViewModel = new UsersCRUDVM(user, usersBLL);
            UsersCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            UsersCRUDViewModel.OnSwitchToAddUsersPage = () => switchToAddUsersPage(user);
            UsersCRUDViewModel.OnSwitchToModifyUsersPage = () => switchToModifyUsersPage(user);
            UsersCRUDViewModel.OnSwitchToDeleteUsersPage = () => switchToDeleteUsersPage(user);
            SelectedVM = UsersCRUDViewModel;
        }

        public void switchToAddUsersPage(Utilizatori user)
        {
            AddUsersViewModel = new AddUsersVM(user, usersBLL);
            AddUsersViewModel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user);
            SelectedVM = AddUsersViewModel;
        }

        public void switchToModifyUsersPage(Utilizatori user)
        {
            ModifyUsersViewModel = new ModifyUsersVM(user, usersBLL);
            ModifyUsersViewModel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user);
            SelectedVM = ModifyUsersViewModel;
        }

        public void switchToDeleteUsersPage(Utilizatori user)
        {
            DeleteUsersViewmodel = new DeleteUsersVM(user, usersBLL);
            DeleteUsersViewmodel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user);
            SelectedVM = DeleteUsersViewmodel;
        }
        #endregion

        #region CategoriesCRUD
        public void switchToCategoriesCRUDMenu(Utilizatori user)
        {
            CategoriesCRUDViewModel = new CategoriesCRUDVM(user, categoriesBLL);
            CategoriesCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            CategoriesCRUDViewModel.OnSwitchToAddCategoriesPage = () => switchToAddCategoriesPage(user);
            CategoriesCRUDViewModel.OnSwitchToModifyCategoriesPage = () => switchToModifyCategoriesPage(user);
            CategoriesCRUDViewModel.OnSwitchToDeleteCategoriesPage = () => switchToDeleteCategoriesPage(user);
            SelectedVM = CategoriesCRUDViewModel;
        }

        public void switchToAddCategoriesPage(Utilizatori user)
        {
            AddCategoriesViewModel = new AddCategoriesVM(user, categoriesBLL);
            AddCategoriesViewModel.OnSwitchToCategoriesCRUDMenu = () => switchToCategoriesCRUDMenu(user);
            SelectedVM = AddCategoriesViewModel;
        }

        public void switchToModifyCategoriesPage(Utilizatori user)
        {
            ModifyCategoriesViewModel = new ModifyCategoriesVM(user, categoriesBLL);
            ModifyCategoriesViewModel.OnSwitchToCategoriesCRUDMenu = () => switchToCategoriesCRUDMenu(user);
            SelectedVM = ModifyCategoriesViewModel;
        }

        public void switchToDeleteCategoriesPage(Utilizatori user)
        {
            DeleteCategoriesViewModel = new DeleteCategoriesVM(user, categoriesBLL);
            DeleteCategoriesViewModel.OnSwitchToCategoriesCRUDMenu = () => switchToCategoriesCRUDMenu(user);
            SelectedVM = DeleteCategoriesViewModel;
        }

        #endregion
    }
}
