using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace Supermarket.ViewModels.AdministratorRelated.Products
{
    internal class ModifyProductsVM : BaseVM
    {
        CategoriesBLL categoriesBLL;
        ProducersBLL producersBLL;
        ProductsBLL productsBLL;
        Utilizatori user;

        public ModifyProductsVM(Utilizatori userOperating, CategoriesBLL categoriesBLLParam, ProducersBLL producersBLLParam, ProductsBLL productsBLLParam)
        {
            user = userOperating;
            categoriesBLL = categoriesBLLParam;
            producersBLL = producersBLLParam;
            productsBLL = productsBLLParam;
            ProducersNames = new ObservableCollection<string>(ProducersList.Select(producerParsed => producerParsed.NumeProducator).ToList());
            CategoriesNames = new ObservableCollection<string>(CategoriesList.Select(categoryParsed => categoryParsed.NumeCategorie).ToList());
            ProductsNames = new ObservableCollection<string>(ProductsList.Select(productParsed => productParsed.NumeProdus).ToList());
        }

        public ObservableCollection<Categorii> CategoriesList
        {
            get => categoriesBLL.Categories;
            set => categoriesBLL.Categories = value;
        }

        public ObservableCollection<Producatori> ProducersList
        {
            get => producersBLL.Producers;
            set => producersBLL.Producers = value;
        }

        public ObservableCollection<Produse> ProductsList
        {
            get => productsBLL.Products;
            set => productsBLL.Products = value;
        }

        public ObservableCollection<string> CategoriesNames { get; set; }

        public ObservableCollection<string> ProducersNames { get; set; }

        public ObservableCollection<string> ProductsNames { get; set; }


        private Produse selectedProduct;

        public Produse SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private Categorii selectedCategory;

        public Categorii SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        private Producatori selectedProducer;
        public Producatori SelectedProducer
        {
            get => selectedProducer;

            set
            {
                selectedProducer = value;
                OnPropertyChanged("SelectedProducer");
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

        private ICommand keyDownProductSearchCommand;

        public ICommand KeyDownProductSearchCommand
        {
            get
            {
                if (keyDownProductSearchCommand == null)
                {
                    keyDownProductSearchCommand = new RelaySearchCommand(KeyboardKeyDownProductSearch);
                }

                return keyDownProductSearchCommand;
            }
        }

        public void KeyboardKeyDownProductSearch(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedProduct = ProductsList.FirstOrDefault(productParsed => productParsed.NumeProdus == text);
                }
            }
        }

        private ICommand keyDownCategorySearchCommand;

        public ICommand KeyDownCategorySearchCommand
        {
            get
            {
                if (keyDownCategorySearchCommand == null)
                {
                    keyDownCategorySearchCommand = new RelaySearchCommand(KeyboardKeyDownCategorySearch);
                }

                return keyDownCategorySearchCommand;
            }
        }

        public void KeyboardKeyDownCategorySearch(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedCategory = CategoriesList.FirstOrDefault(categoryParsed => categoryParsed.NumeCategorie == text);
                }
            }
        }

        private ICommand keyDownProducersSearchCommand;

        public ICommand KeyDownProducersSearchCommand
        {
            get
            {
                if (keyDownProducersSearchCommand == null)
                {
                    keyDownProducersSearchCommand = new RelaySearchCommand(KeyboardKeyDownProducersSearch);
                }

                return keyDownProducersSearchCommand;
            }
        }

        public void KeyboardKeyDownProducersSearch(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedProducer = ProducersList.FirstOrDefault(producerParsed => producerParsed.NumeProducator == text);
                }
            }
        }

        private ICommand modifyProductCommand;

        public ICommand ModifyProductCommand
        {
            get
            {
                if (modifyProductCommand == null)
                {
                    modifyProductCommand = new RelayCommand<object>(ModifyProduct);
                }

                return modifyProductCommand;
            }
        }

        private void ModifyProduct(object obj)
        {
            if (SelectedProducer == null)
            {
                InfoMessage = "Select a producer";
                InfoVisibility = Visibility.Visible;
                return;

            }

            if (SelectedCategory == null)
            {
                InfoMessage = "Select a category";
                InfoVisibility = Visibility.Visible;
                return;

            }

            productsBLL.UpdateProduct(SelectedProduct);

            if (productsBLL.ErrorMessage != "" && productsBLL.ErrorMessage != null)
            {
                InfoMessage = productsBLL.ErrorMessage;
                InfoVisibility = Visibility.Visible;
                return;
            }

            UpdateLocalData();

            InfoMessage = "Product Modified!";
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

        private void UpdateLocalData()
        {
            ProductsNames = new ObservableCollection<string>(ProducersList.Select(categoryParsed => categoryParsed.NumeProducator).ToList());
            SelectedProduct = null;
        }


        #region Navigation

        // COMMANDS

        private ICommand switchToProductsCRUDMenuCommand;

        public ICommand SwitchToProductsCRUDMenuCommand
        {
            get
            {
                if (switchToProductsCRUDMenuCommand == null)
                {
                    switchToProductsCRUDMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToProductsCRUDMenu(); });
                }

                return switchToProductsCRUDMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToProductsCRUDMenu();
        public SwitchToProductsCRUDMenu OnSwitchToProductsCRUDMenu { get; set; }

        #endregion
    }
}
