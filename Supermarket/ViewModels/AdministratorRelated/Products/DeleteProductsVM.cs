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

namespace Supermarket.ViewModels.AdministratorRelated.Products
{
    internal class DeleteProductsVM : BaseVM
    {
        Utilizatori userOperating;
        ProductsBLL productsBLL;
        ProducersBLL producersBLL;
        CategoriesBLL categoriesBLL;

        public DeleteProductsVM(Utilizatori user, ProductsBLL productsBLLParam, ProducersBLL producersBLL, CategoriesBLL categoriesBLL)
        {
            userOperating = user;
            productsBLL = productsBLLParam;
            this.producersBLL = producersBLL;
            this.categoriesBLL = categoriesBLL;
        }

        public ProducersBLL ProducersBLL
        {
            get => producersBLL;
            set
            {
                producersBLL = value;
                OnPropertyChanged("ProductsBLL");
            }
        }

        public CategoriesBLL CategoriesBLL
        {
            get => categoriesBLL;
            set
            {
                categoriesBLL = value;
                OnPropertyChanged("CategoriesBLL");
            }

        }



        public ObservableCollection<Produse> ProductsList
        {
            get => productsBLL.ProductsActive;
            set => productsBLL.ProductsActive = value;
        }

        Produse selectedProduct;
        public Produse SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
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

        private ICommand deleteProductCommand;

        public ICommand DeleteProductCommand
        {
            get
            {
                if (deleteProductCommand == null)
                {
                    deleteProductCommand = new RelayCommand<object>(DeleteProduct);
                }

                return deleteProductCommand;
            }
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct.NumeProdus == null || SelectedProduct.NumeProdus == "")
            {
                InfoMessage = "Select a producer!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            productsBLL.DeleteProduct(SelectedProduct);

            SelectedProduct = new Produse();

            InfoMessage = "Product Deleted!";
            InfoVisibility = Visibility.Visible;

            // Set up a timer to hide the error message after 5 seconds
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5000 milliseconds = 5 seconds
            timer.AutoReset = false; // Only fire once
            timer.Elapsed += (sender, e) =>
            {
                InfoVisibility = Visibility.Hidden;
                timer.Dispose(); // Dispose the timer to release resources
            };
            timer.Start();
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
