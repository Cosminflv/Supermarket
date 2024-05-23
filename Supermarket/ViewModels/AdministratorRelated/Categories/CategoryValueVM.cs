using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Categories
{
    internal class CategoryValueVM : BaseVM
    {
        Utilizatori user;
        CategoriesBLL categoriesBLL;

        public CategoryValueVM(Utilizatori user, CategoriesBLL categoriesBLL)
        {
            this.user = user;
            this.categoriesBLL = categoriesBLL;
            Categories = categoriesBLL.CategoriesActive;
            CalculateValues();
        }

        ObservableCollection<Categorii> categories;

        public ObservableCollection<Categorii> Categories
        {
            get
            {
                return this.categories;
            }
            set
            {
                this.categories = value;
                OnPropertyChanged("Categories");
            }
        }

        Dictionary<string, decimal> categoryTotalPrices;

        public Dictionary<string, decimal> CategoryTotalPrices
        {
            get
            {
                return categoryTotalPrices;
            }
            set
            {
                categoryTotalPrices = value; OnPropertyChanged("CategoryTotalPrices");
            }
        }

        private void CalculateValues()
        {
            // Dictionary to store the total prices for each category
            Dictionary<string, decimal> categoryTotalPricesLocal = new Dictionary<string, decimal>();

            // Iterate through each product
            foreach (var produs in Categories.SelectMany(c => c.Produses))
            {
                // Get the category name for the current product
                string categoryName = produs.Categorii.NumeCategorie;

                // Calculate the total price for the current product
                decimal totalPrice = 0;

                // Iterate through each stock of the current product
                foreach (var stoc in produs.Stocuris)
                {
                    // Add the sale price of the stock to the total price
                    totalPrice += stoc.PretVanzare ?? stoc.PretAchizitie;
                }

                // Add the total price to the sum of prices for the corresponding category
                if (categoryTotalPricesLocal.ContainsKey(categoryName))
                {
                    categoryTotalPricesLocal[categoryName] += totalPrice;
                }
                else
                {
                    categoryTotalPricesLocal.Add(categoryName, totalPrice);
                }
            }

            CategoryTotalPrices = categoryTotalPricesLocal;
        }



        #region Navigation

        // COMMANDS

        private ICommand switchToCategoriesCRUDMenuCommand;

        public ICommand SwitchToCategoriesCRUDMenuCommand
        {
            get
            {
                if (switchToCategoriesCRUDMenuCommand == null)
                {
                    switchToCategoriesCRUDMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToCategoriesCRUDMenu(); });
                }

                return switchToCategoriesCRUDMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToCategoriesCRUDMenu();
        public SwitchToCategoriesCRUDMenu OnSwitchToCategoriesCRUDMenu { get; set; }

        #endregion
    }
}
