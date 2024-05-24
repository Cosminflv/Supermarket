

using Supermarket.Models.BusinessLogicLayer;
using Supermarket.ViewModels.AdministratorRelated;
using Supermarket.ViewModels.AdministratorRelated.Categories;
using Supermarket.ViewModels.AdministratorRelated.Producers;
using Supermarket.ViewModels.AdministratorRelated.Products;
using Supermarket.ViewModels.AdministratorRelated.Stocks;
using Supermarket.ViewModels.AdministratorRelated.Users;

namespace Supermarket.ViewModels
{
    internal class MainWindowVM : BaseVM
    {
        private BaseVM selectedVM;

        UsersBLL usersBLL;
        CategoriesBLL categoriesBLL;
        ProducersBLL producersBLL;
        ProductsBLL productsBLL;
        StocksBLL stocksBLL;

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

        public CategoryValueVM CategoryValueViewModel { get; set; } 

        public ProducersCRUDVM ProducersCRUDViewModel { get; set; }

        public AddProducersVM AddProducersViewModel { get; set; }

        public ModifyProducersVM ModifyProducersViewModel { get; set; }

        public DeleteProducersVM DeleteProducersViewModel { get; set; }

        public ProductsCRUDVM ProductsCRUDViewModel { get; set; }   

        public AddProductsVM AddProductsViewModel { get; set; }

        public ModifyProductsVM ModifyProductsViewModel { get; set; }

        public DeleteProductsVM DeleteProductsViewModel { get; set; }

        public AddStocksVM AddStocksViewModel { get; set; } 

        public ProducerProductsVM ProducerProductsViewModel { get; set; }

        public UserSalesVM UserSalesViewModel { get; set; }



        #endregion


        public MainWindowVM()
        {
            usersBLL = new UsersBLL();
            categoriesBLL = new CategoriesBLL();
            producersBLL = new ProducersBLL();
            productsBLL = new ProductsBLL();
            stocksBLL = new StocksBLL();
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
            AdministratorMenuViewModel.OnSwitchProducersCRUD = () => switchToProducersCRUDMenu(user);
            AdministratorMenuViewModel.OnSwitchToProductsCRUD = () => switchToProductsCRUDMenu(user);
            AdministratorMenuViewModel.OnSwitchToAddStocks = () => switchToAddStocksPage(user);
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
            UsersCRUDViewModel.OnSwitchToUsersSalesPage = () => switchToUserSalesPage(user);
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

        public void switchToUserSalesPage(Utilizatori user)
        {
            UserSalesViewModel = new UserSalesVM(user, usersBLL);
            UserSalesViewModel.OnSwitchToUsersCRUDMenu = () => switchToUsersCRUDMenu(user);
            SelectedVM = UserSalesViewModel;
        }
        #endregion

        #region CategoriesCRUD
        public void switchToCategoriesCRUDMenu(Utilizatori user)
        {
            CategoriesCRUDViewModel = new CategoriesCRUDVM();
            CategoriesCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            CategoriesCRUDViewModel.OnSwitchToAddCategoriesPage = () => switchToAddCategoriesPage(user);
            CategoriesCRUDViewModel.OnSwitchToModifyCategoriesPage = () => switchToModifyCategoriesPage(user);
            CategoriesCRUDViewModel.OnSwitchToDeleteCategoriesPage = () => switchToDeleteCategoriesPage(user);
            CategoriesCRUDViewModel.OnSwitchToCategoryValuePage = () => switchToCategoryValuePage(user);
           
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

        public void switchToCategoryValuePage(Utilizatori user)
        {
            CategoryValueViewModel = new CategoryValueVM(user, categoriesBLL);
            CategoryValueViewModel.OnSwitchToCategoriesCRUDMenu = () => switchToCategoriesCRUDMenu(user);
            SelectedVM = CategoryValueViewModel;
        }

        #endregion

        #region ProducersCRUD

        public void switchToProducersCRUDMenu(Utilizatori user)
        {
            ProducersCRUDViewModel = new ProducersCRUDVM(user);
            ProducersCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            ProducersCRUDViewModel.OnSwitchToAddProducersPage = () => switchToAddProducersPage(user);
            ProducersCRUDViewModel.OnSwitchToModifyProducersPage = () => switchToModifyProducersPage(user);
            ProducersCRUDViewModel.OnSwitchToDeleteProducersPage = () => switchToDeleteProducersPage(user);
            ProducersCRUDViewModel.OnSwitchToProducerProductsPage = () => switchToProducerProductsPage(user);
            SelectedVM = ProducersCRUDViewModel;
        }

        public void switchToAddProducersPage(Utilizatori user)
        {
            AddProducersViewModel = new AddProducersVM(user, producersBLL);
            AddProducersViewModel.OnSwitchToProducersCRUDMenu = () => switchToProducersCRUDMenu(user);
            SelectedVM = AddProducersViewModel;
        }

        public void switchToModifyProducersPage(Utilizatori user)
        {
            ModifyProducersViewModel = new ModifyProducersVM(user, producersBLL);
            ModifyProducersViewModel.OnSwitchToProducersCRUDMenu = () => switchToProducersCRUDMenu(user);
            SelectedVM = ModifyProducersViewModel;
        }

        public void switchToDeleteProducersPage(Utilizatori user)
        {
            DeleteProducersViewModel = new DeleteProducersVM(user, producersBLL);
            DeleteProducersViewModel.OnSwitchToProducersCRUDMenu = () => switchToProducersCRUDMenu(user);
            SelectedVM = DeleteProducersViewModel;
        }

        public void switchToProducerProductsPage(Utilizatori user)
        {
            ProducerProductsViewModel = new ProducerProductsVM(user, producersBLL, categoriesBLL);
            ProducerProductsViewModel.OnSwitchToProducersCRUDMenu = () => switchToProducersCRUDMenu(user);
            SelectedVM = ProducerProductsViewModel;
        }

        #endregion

        #region ProductsCRUD

        public void switchToProductsCRUDMenu(Utilizatori user)
        {
            ProductsCRUDViewModel = new ProductsCRUDVM(user);
            ProductsCRUDViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            ProductsCRUDViewModel.OnSwitchToAddProductsPage = () => switchToAddProductsPage(user);
            ProductsCRUDViewModel.OnSwitchToModifyProductsPage = () => switchToModifyProductsPage(user);
            ProductsCRUDViewModel.OnSwitchToDeleteProductsPage = () => switchToDeleteProductsPage(user);

            SelectedVM = ProductsCRUDViewModel;
        }

        public void switchToAddProductsPage(Utilizatori user)
        {
            AddProductsViewModel = new AddProductsVM(user, categoriesBLL, producersBLL, productsBLL);
            AddProductsViewModel.OnSwitchToProductsCRUDMenu = () => switchToProductsCRUDMenu(user);
            SelectedVM = AddProductsViewModel;
        }

        public void switchToModifyProductsPage(Utilizatori user)
        {
            ModifyProductsViewModel = new ModifyProductsVM(user, categoriesBLL, producersBLL, productsBLL);
            ModifyProductsViewModel.OnSwitchToProductsCRUDMenu = () => switchToProductsCRUDMenu(user);
            SelectedVM = ModifyProductsViewModel;
        }

        public void switchToDeleteProductsPage(Utilizatori user)
        {
            DeleteProductsViewModel = new DeleteProductsVM(user, productsBLL, producersBLL, categoriesBLL);
            DeleteProductsViewModel.OnSwitchToProductsCRUDMenu = () => switchToProductsCRUDMenu(user);
            SelectedVM = DeleteProductsViewModel;
        }
        #endregion

        #region Stocks
        public void switchToAddStocksPage(Utilizatori user)
        {
            AddStocksViewModel = new AddStocksVM(user, stocksBLL, productsBLL);
            AddStocksViewModel.OnSwitchToAdministratorMenu = () => switchToAdministratorMenu(user);
            SelectedVM = AddStocksViewModel;
        }
        #endregion
    }
}
