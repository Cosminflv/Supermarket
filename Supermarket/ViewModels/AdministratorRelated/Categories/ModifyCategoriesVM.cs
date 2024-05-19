using Supermarket.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;

namespace Supermarket.ViewModels.AdministratorRelated.Categories
{
    internal class ModifyCategoriesVM : BaseVM
    {

        public ModifyCategoriesVM(Utilizatori userOperating, CategoriesBLL categoriesBLLParam)
        {
            userOperating = user;
            categoriesBLL = categoriesBLLParam;
            CategoriesNames = new ObservableCollection<string>(CategoriesList.Select(categoryParsed => categoryParsed.NumeCategorie).ToList());
            SelectedCategory = new Categorii();
        }

        public ObservableCollection<Categorii> CategoriesList
        {
            get => categoriesBLL.Categories;
            set => categoriesBLL.Categories = value;
        }

        public ObservableCollection<string> CategoriesNames { get; set; }

        Categorii selectedCategory;

        public Categorii SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        
        Utilizatori user;
        CategoriesBLL categoriesBLL;

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

        private ICommand keyDownCommand;

        public ICommand KeyDownCommand
        {
            get
            {
                if (keyDownCommand == null)
                {
                    keyDownCommand = new RelaySearchCommand(KeyboardKeyDown);
                }

                return keyDownCommand;
            }
        }

        private ICommand modifyCategoryCommand;

        public ICommand ModifyCategoryCommand
        {
            get
            {
                if (modifyCategoryCommand == null)
                {
                    modifyCategoryCommand = new RelayCommand<object>(ModifyCategory);
                }

                return modifyCategoryCommand;
            }
        }

        private void ModifyCategory(object obj)
        {
            if (SelectedCategory.NumeCategorie == null)
            {
                InfoMessage = "No Category Selected!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            categoriesBLL.UpdateCategory(SelectedCategory);

            UpdateLocalData();
            InfoMessage = "User Modified!";
            InfoVisibility = Visibility.Visible;

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
        }

        public void KeyboardKeyDown(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedCategory = CategoriesList.FirstOrDefault(categoryParsed => categoryParsed.NumeCategorie == text);
                }
            }

        }

        private void UpdateLocalData()
        {
            CategoriesNames = new ObservableCollection<string>(CategoriesList.Select(categoryParsed => categoryParsed.NumeCategorie).ToList());
            SelectedCategory = new Categorii();
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
