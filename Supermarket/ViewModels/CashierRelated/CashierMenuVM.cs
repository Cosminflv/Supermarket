using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace Supermarket.ViewModels.CashierRelated
{
    class CashierMenuVM : BaseVM
    {
        Utilizatori user;
        ProducersBLL producersBLL;
        ProductsBLL productsBLL;
        CategoriesBLL categoriesBLL;

        public CashierMenuVM(Utilizatori user, ProducersBLL producersBLLParam, ProductsBLL productsBLLParam, CategoriesBLL categoriesBLLParam)
        {
            this.user = user;
            productsBLL = productsBLLParam;
            producersBLL = producersBLLParam;
            categoriesBLL = categoriesBLLParam;
            FilteredProducts = productsBLL.ProductsActive;
            FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
            SelectedExpiringDate = DateTime.Now;
        }

        ObservableCollection<string> filteredProductsNames;

        public ObservableCollection<string> FilteredProductsNames
        {
            get
            {
                return filteredProductsNames;
            }
            set
            {
                filteredProductsNames = value;
                OnPropertyChanged("FilteredProductsNames");
            }
        }

        Produse selectedProduct;

        public Produse SelectedProduct
        {
            get
            {
                return selectedProduct;
            }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProducts");
            }
        }

        string barcode;

        public string Barcode
        {
            get
            {
                return barcode;
            }
            set
            {
                barcode = value;
                OnPropertyChanged("Barcode");
                FilteredProducts = new ObservableCollection<Produse>(FilteredProducts.Where(item => item.CodBare.StartsWith(Barcode)).ToList());
                FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
            }
        }

        System.DateTime selectedExpiringDate;

        public System.DateTime SelectedExpiringDate
        {
            get
            {
                return selectedExpiringDate;
            }
            set
            {
                selectedExpiringDate = value;
                OnPropertyChanged("SelectedExpiringDate");

                if (SelectedExpiringDate.Hour != DateTime.Now.Hour)
                {
                    FilteredProducts = new ObservableCollection<Produse>(
                        FilteredProducts.Where(item =>
                            {
                                var activeStocks = item.Stocuris.Where(stoc => stoc.IsActive);
                                return activeStocks.Any(stock => stock.DataExpirare == SelectedExpiringDate);
                            }
                            ).ToList());

                    FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
                }
            }
        }

        public ObservableCollection<Producatori> ProducersList
        {
            get => producersBLL.ProducersActive;
            set => producersBLL.ProducersActive = value;
        }

        public ObservableCollection<Categorii> CategoriesList
        {
            get => categoriesBLL.CategoriesActive;
            set => categoriesBLL.CategoriesActive = value;
        }



        private ObservableCollection<Produse> filteredProducts;

        public ObservableCollection<Produse> FilteredProducts
        {
            get
            {
                return filteredProducts;
            }
            set
            {
                filteredProducts = value;
                OnPropertyChanged("FilteredProducts");
            }
        }

        private Producatori selectedProducer;

        public Producatori SelectedProducer
        {
            get
            {
                return selectedProducer;
            }
            set
            {
                selectedProducer = value;
                OnPropertyChanged("SelectedProducer");
            }
        }

        private Categorii selectedCategory;

        public Categorii SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
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

        private ICommand emitReceiptCommand;

        public ICommand EmitReceiptCommand
        {
            get
            {
                if (emitReceiptCommand == null)
                {
                    emitReceiptCommand = new RelayCommand<object>(EmitReceipt);
                }

                return emitReceiptCommand;
            }
        }

        private ICommand producerSelectedCommand;

        public ICommand ProducerSelectedCommand
        {
            get
            {
                if (producerSelectedCommand == null)
                {
                    producerSelectedCommand = new RelayCommand<object>(ProducerSelected);
                }

                return producerSelectedCommand;
            }
        }

        private ICommand categorySelectedCommand;

        public ICommand CategorySelectedCommand
        {
            get
            {
                if (categorySelectedCommand == null)
                {
                    categorySelectedCommand = new RelayCommand<object>(CategorySelected);
                }

                return categorySelectedCommand;
            }
        }

        private ICommand addProductCommand;

        public ICommand AddProductCommand
        {
            get
            {
                if (addProductCommand == null)
                {
                    addProductCommand = new RelayCommand<object>(AddProduct);
                }

                return addProductCommand;
            }
        }



        private ICommand resetFiltersCommand;

        public ICommand ResetFiltersCommand
        {
            get
            {
                if (resetFiltersCommand == null)
                {
                    resetFiltersCommand = new RelayCommand<object>(ResetFilters);
                }

                return resetFiltersCommand;
            }
        }

        private void ResetFilters(object obj)
        {
            FilteredProducts = productsBLL.ProductsActive;
            FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());

            SelectedProducer = null;
            SelectedCategory = null;
            Barcode = "";
            SelectedExpiringDate = DateTime.Now;
        }

        private void CategorySelected(object obj)
        {
            if (SelectedCategory != null)
            {
                FilteredProducts = new ObservableCollection<Produse>(FilteredProducts.Where(item => item.Categorii.NumeCategorie == SelectedCategory.NumeCategorie).ToList());
                FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
            }
        }

        private void ProducerSelected(object obj)
        {
            if (SelectedProducer != null)
            {
                FilteredProducts = new ObservableCollection<Produse>(FilteredProducts.Where(item => item.Producatori.NumeProducator == SelectedProducer.NumeProducator).ToList());
                FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
            }
        }

        private void AddProduct(object obj)
        {

        }

        private void EmitReceipt(object obj)
        {

        }

        public void KeyboardKeyDown(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedProduct = FilteredProducts.FirstOrDefault(productParsed => productParsed.NumeProdus == text);
                }
            }

        }

        #region Navigation

        // COMMANDS

        private ICommand switchToLoginCommand;
        public ICommand SwitchToLoginCommand
        {
            get
            {
                if (switchToLoginCommand == null)
                {
                    switchToLoginCommand = new RelayPagesCommand(o => true, o => { OnSwitchToLogin(); });
                }

                return switchToLoginCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToLogin();
        public SwitchToLogin OnSwitchToLogin { get; set; }

        #endregion
    }
}
