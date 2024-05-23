using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Stocks
{
    internal class AddStocksVM : BaseVM
    {
        Utilizatori user;
        StocksBLL stocksBLL;
        ProductsBLL productsBLL;

        public AddStocksVM(Utilizatori user, StocksBLL stocksBLL, ProductsBLL productsBLL)
        {
            this.user = user;
            this.stocksBLL = stocksBLL;
            this.productsBLL = productsBLL;
            ProductsNames = new ObservableCollection<string>(ProductsList.Select(productParsed => productParsed.NumeProdus).ToList());
            SupplyDate = System.DateTime.Now;
            ExpirationDate = System.DateTime.Now;  
        }

        public ObservableCollection<Produse> ProductsList
        {
            get => productsBLL.ProductsActive;
            set => productsBLL.ProductsActive = value;
        }

        Produse selectedProduct;

        public Produse SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        ObservableCollection<string> productsNames;
        public ObservableCollection<string> ProductsNames
        {
            get
            {
                return productsNames;
            }
            set
            {
                productsNames = value;
                OnPropertyChanged("ProductsNames");
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

        string quantity;

        public string Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        string measureUnit;

        public string MeasureUnit
        {
            get => measureUnit;
            set
            {
                measureUnit = value;
                OnPropertyChanged("MeasureUnit");
            }
        }

        System.DateTime supplyDate;

        public System.DateTime SupplyDate
        {
            get => supplyDate;
            set
            {
                supplyDate = value;
                OnPropertyChanged("SupplyDate");
            }
        }

        System.DateTime expirationDate;

        public System.DateTime ExpirationDate
        {
            get => expirationDate;
            set
            {
                expirationDate = value;
                OnPropertyChanged("ExpirationDate");
            }
        }

        string purchasePrice;

        public string PurchasePrice
        {
            get => purchasePrice;
            set
            {
                purchasePrice = value;
                OnPropertyChanged("PurchasePrice");
            }
        }

        string retailPrice;

        public string RetailPrice
        {
            get => retailPrice;
            set
            {
                retailPrice = value;
                OnPropertyChanged("RetailPrice");
            }
        }

        public void KeyboardKeyDown(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedProduct = ProductsList.FirstOrDefault(productParsed => productParsed.NumeProdus == text);
                }
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

        private ICommand addStockCommand;

        public ICommand AddStockCommand
        {
            get
            {
                if (addStockCommand == null)
                {
                    addStockCommand = new RelaySearchCommand(AddStock);
                }

                return addStockCommand;
            }
        }

        private void AddStock(object obj)
        {
            if (ValidateFields())
            {
                Stocuri stockToAdd = new Stocuri();

                stockToAdd.ProdusID = SelectedProduct.ProdusID;
                stockToAdd.Cantitate = int.Parse(Quantity);
                stockToAdd.UnitateMasura = MeasureUnit;
                stockToAdd.DataAprovizionare = SupplyDate;
                stockToAdd.DataExpirare = ExpirationDate;
                stockToAdd.PretAchizitie = decimal.Parse(PurchasePrice);
                stockToAdd.PretVanzare = calculateRetailPrice(1.2m);
                stockToAdd.IsActive = true;

                stocksBLL.AddMethod(stockToAdd);

                InfoMessage = "Stock Added!";
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


                clearFields();
            }
        }

        private bool ValidateFields()
        {
            if(SelectedProduct == null)
            {
                InfoMessage = "Select a product!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if(!IsNumeric(Quantity) || Quantity == null || Quantity == "0")
            {
                InfoMessage = "Invalid Quantity!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if(MeasureUnit == null)
            {
                InfoMessage = "Invalid Measure Unit!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if (SupplyDate == null)
            {
                InfoMessage = "Invalid SupplyDate!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if(ExpirationDate == null)
            {
                InfoMessage = "Invalid ExpirationDate!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if(ExpirationDate < SupplyDate)
            {
                InfoMessage = "Invalid Expiration/SupplyDate!";
                InfoVisibility = Visibility.Visible;
                return false;
            }

            if(PurchasePrice == null || PurchasePrice == "0" || !IsDecimal(PurchasePrice))
            {
                InfoMessage = "Invalid Purchase Price!";
                InfoVisibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            SelectedProduct = null;
            Quantity = null;
            MeasureUnit = null;
            PurchasePrice = null;
            SupplyDate = System.DateTime.Now;
            ExpirationDate = System.DateTime.Now;
        }

        private decimal calculateRetailPrice(decimal additionPercentage)
        {
            decimal purchasePrice = decimal.Parse(PurchasePrice);
            decimal retailPrice = purchasePrice * additionPercentage;
            return retailPrice;
        }

        private bool IsNumeric(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDecimal(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    if(c != '.')
                    {
                    return false;
                    }
                }
            }
            return true;
        }



        #region Navigation

        // COMMANDS

        private ICommand switchAdministratorMenuCommand;

        public ICommand SwitchAdministratorMenuCommand
        {
            get
            {
                if (switchAdministratorMenuCommand == null)
                {
                    switchAdministratorMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAdministratorMenu(); });
                }

                return switchAdministratorMenuCommand;
            }
        }





        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        #endregion
    }
}
