using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Categories
{
    internal class AddCategoriesVM : BaseVM
    {
        Utilizatori user;
        CategoriesBLL categoriesBLL;

        public AddCategoriesVM(Utilizatori userOperating, CategoriesBLL categoriesBLLParam)
        {
            user = userOperating;
            categoriesBLL = categoriesBLLParam;
        }

        private string categoryName;

        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
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

        private ICommand addCategoryCommand;

        public ICommand AddCategoryCommand
        {
            get
            {
                if (addCategoryCommand == null)
                {
                    addCategoryCommand = new RelayCommand<object>(AddCategory);
                }

                return addCategoryCommand;
            }
        }

        private void AddCategory(object obj)
        {
            Categorii category = new Categorii();
            category.NumeCategorie = CategoryName;
            category.IsActive = true;
            categoriesBLL.AddMethod(category);

            if(categoriesBLL.ErrorMessage != "" && categoriesBLL.ErrorMessage != null)
            {
                InfoMessage = categoriesBLL.ErrorMessage;
                InfoVisibility = Visibility.Visible;
                return;
            }

            InfoVisibility = Visibility.Visible;
            InfoMessage = "Category Added!";

            // Set up a timer to hide the error message after 5 seconds
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5000 milliseconds = 5 seconds
            timer.AutoReset = false; // Only fire once
            timer.Elapsed += (sender, e) =>
            {
                InfoVisibility = Visibility.Hidden;
                InfoMessage = "";
                timer.Dispose(); // Dispose the timer to release resources
            };
            timer.Start();

            CategoryName = "";
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
