using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.ViewModels.AdministratorRelated.Categories
{
    internal class DeleteCategoriesVM : BaseVM
    {
        Utilizatori userOperating;
        CategoriesBLL categoriesBLL;

        public DeleteCategoriesVM(Utilizatori user, CategoriesBLL categoriesBLLParam)
        {
            userOperating = user;
            categoriesBLL = categoriesBLLParam;
            SelectedCategory = new Categorii();
        }

        public ObservableCollection<Categorii> CategoriesList
        {
            get => categoriesBLL.CategoriesActive;
            set => categoriesBLL.CategoriesActive = value;
        }

        Categorii selectedCategory;
        public Categorii SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
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

        private ICommand deleteCategoryCommand;

        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (deleteCategoryCommand == null)
                {
                    deleteCategoryCommand = new RelayCommand<object>(DeleteUser);
                }

                return deleteCategoryCommand;
            }
        }

        private void DeleteUser(object obj)
        {
            if (SelectedCategory.NumeCategorie == null)
            {
                InfoMessage = "Select an category!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            categoriesBLL.DeleteCategory(SelectedCategory);

            SelectedCategory = new Categorii();

            InfoMessage = "User Deleted!";
            InfoVisibility = Visibility.Visible;

            // Set up a timer to hide the error message after 5 seconds
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5000 milliseconds = 5 seconds
            timer.AutoReset = false; // Only fire once
            timer.Elapsed += (sender, e) =>
            {
                InfoVisibility = Visibility.Hidden;
                InfoMessage = "User Deleted!";
                timer.Dispose(); // Dispose the timer to release resources
            };
            timer.Start();
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
