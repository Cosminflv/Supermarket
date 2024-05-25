using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
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
        StocksBLL stocksBLL;
        RecieptsBLL recieptsBLL;

        public CashierMenuVM(Utilizatori user, ProducersBLL producersBLLParam, ProductsBLL productsBLLParam, CategoriesBLL categoriesBLLParam, StocksBLL stocksBLLParam, RecieptsBLL recieptsBLLParam)
        {
            this.user = user;
            productsBLL = productsBLLParam;
            producersBLL = producersBLLParam;
            categoriesBLL = categoriesBLLParam;
            stocksBLL = stocksBLLParam;
            recieptsBLL = recieptsBLLParam;
            FilteredProducts = productsBLL.ProductsActive;
            FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(producerParsed => producerParsed.NumeProdus).ToList());
            SelectedExpiringDate = DateTime.Now;
            SelectedProducts = new Dictionary<string, Tuple<Produse, int, decimal>>();
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

         Dictionary<string, Tuple<Produse, int, decimal>> selectedProducts;

        public Dictionary<string, Tuple<Produse, int, decimal>> SelectedProducts
        {
            get
            {
                return selectedProducts;
            }
            set
            {
                selectedProducts = value;
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
            bool isStock = stocksBLL.IsProductAvailable(SelectedProduct.ProdusID);

            if (isStock)
            {
                decimal productPrice = productsBLL.CalculateProductPrice(SelectedProduct.ProdusID);

                // Create a new dictionary
                Dictionary<string, Tuple<Produse, int, decimal>> newDict = copyDict(SelectedProducts);


                if (!SelectedProducts.ContainsKey(SelectedProduct.NumeProdus))
                {
                    newDict[SelectedProduct.NumeProdus] = new Tuple<Produse, int, decimal>(SelectedProduct, 1, productPrice);
                    SelectedProducts = newDict;
                    return;
                }
                newDict[SelectedProduct.NumeProdus] = new Tuple<Produse, int, decimal>(SelectedProduct, newDict[SelectedProduct.NumeProdus].Item2 + 1, newDict[SelectedProduct.NumeProdus].Item3 + productPrice);
                SelectedProducts = newDict;
            }
        }

        private void EmitReceipt(object obj)
        {
            recieptsBLL.AddReceipt(DateTime.Now, user.UtilizatorID);

            foreach(var pair in SelectedProducts)
            {
                string productName = pair.Key;
                Tuple<Produse, int, decimal> productDetails = pair.Value;
                recieptsBLL.AddReceiptDetail(recieptsBLL.GetLastReceiptID(), productDetails.Item1.ProdusID, productDetails.Item2);
            }
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

        Dictionary<string, Tuple<Produse, int, decimal>> copyDict(Dictionary<string, Tuple<Produse, int, decimal>> dictToCopy)
        {
            Dictionary<string, Tuple<Produse, int, decimal>> newDict = new Dictionary<string, Tuple<Produse, int, decimal>>();

            // Iterate over the original dictionary and add each key-value pair to the new dictionary
            foreach (var kvp in dictToCopy)
            {
                // Assuming Produse is a reference type and needs to be cloned if you want a deep copy.
                // For now, we'll just use the same instance of Produse.
                Produse productCopy = new Produse
                {
                    ProdusID = kvp.Value.Item1.ProdusID,
                    NumeProdus = kvp.Value.Item1.NumeProdus,
                    CodBare = kvp.Value.Item1.CodBare,
                    CategorieID = kvp.Value.Item1.CategorieID,
                    ProducatorID = kvp.Value.Item1.ProducatorID,
                    IsActive = kvp.Value.Item1.IsActive,
                    Categorii = kvp.Value.Item1.Categorii,
                    DetaliiBons = kvp.Value.Item1.DetaliiBons,
                    Producatori = kvp.Value.Item1.Producatori,
                    Stocuris = kvp.Value.Item1.Stocuris,

                    // Copy other properties if needed
                };

                // Add the copied key-value pair to the new dictionary
                newDict[kvp.Key] = new Tuple<Produse, int, decimal>(productCopy, kvp.Value.Item2, kvp.Value.Item3);
            }

            return newDict;
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
