using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.ViewModels.AdministratorRelated.Products
{
    internal class AddProductsVM : BaseVM
    {
        CategoriesBLL categoriesBLL;
        ProducersBLL producersBLL;
        ProductsBLL productsBLL;
        Utilizatori user;

        public AddProductsVM(Utilizatori userOperating, CategoriesBLL categoriesBLLParam, ProducersBLL producersBLLParam, ProductsBLL productsBLLParam)
        {
            user = userOperating;
            categoriesBLL = categoriesBLLParam;
            producersBLL = producersBLLParam;
            productsBLL = productsBLLParam;
            ProducersNames = new ObservableCollection<string>(ProducersList.Select(producerParsed => producerParsed.NumeProducator).ToList());
            CategoriesNames = new ObservableCollection<string>(CategoriesList.Select(categoryParsed => categoryParsed.NumeCategorie).ToList());
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

        public ObservableCollection<string> CategoriesNames { get; set; }

        public ObservableCollection<string> ProducersNames { get; set; }

        private string productName;
        public string ProductName
        {
            get => productName;

            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }

        private string barCode;
        public string BarCode
        {
            get => barCode;

            set
            {
                barCode = value;
                OnPropertyChanged("BarCode");
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

        private ICommand addProductCommand;

        public ICommand AddProductCommand
        {
            get
            {
                if (addProductCommand == null)
                {
                    addProductCommand = new RelayCommand<object>(AddProducer);
                }

                return addProductCommand;
            }
        }

        private void AddProducer(object obj)
        {
            if(SelectedCategory == null)
            {
                InfoMessage = "Select a category";
                InfoVisibility = Visibility.Visible;
                return;
            }

            if (SelectedProducer == null)
            {
                InfoMessage = "Select a producer";
                InfoVisibility = Visibility.Visible;
                return;
            }

            Produse product = new Produse();
            product.NumeProdus = ProductName;
            product.CodBare = BarCode;
            product.CategorieID = SelectedCategory.CategorieID;
            product.ProducatorID = SelectedProducer.ProducatorID;
            product.IsActive = false;

            productsBLL.AddMethod(product);

            if (productsBLL.ErrorMessage != "" && productsBLL.ErrorMessage != null)
            {
                InfoMessage = productsBLL.ErrorMessage;
                InfoVisibility = Visibility.Visible;
                return;
            }

            InfoVisibility = Visibility.Visible;
            InfoMessage = "Product Added!";

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

            SelectedProducer.NumeProducator = null;
            SelectedProducer = null;
            SelectedCategory.NumeCategorie = null;
            SelectedCategory = null;
            ProductName = "";
            BarCode = "";
        }

        private ICommand keyDownProducerCommand;

        public ICommand KeyDownProducerCommand
        {
            get
            {
                if (keyDownProducerCommand == null)
                {
                    keyDownProducerCommand = new RelaySearchCommand(KeyDownCommandProducer);
                }

                return keyDownProducerCommand;
            }
        }

        public void KeyDownCommandProducer(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedProducer = ProducersList.FirstOrDefault(producerParsed => producerParsed.NumeProducator == text);
                }
            }

        }

        private ICommand keyDownCategoryCommand;

        public ICommand KeyDownCategoryCommand
        {
            get
            {
                if (keyDownCategoryCommand == null)
                {
                    keyDownCategoryCommand = new RelaySearchCommand(KeyDownCommandCategory);
                }

                return keyDownCategoryCommand;
            }
        }

        public void KeyDownCommandCategory(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string text && parameters[1] is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    SelectedCategory = CategoriesList.FirstOrDefault(categoryParsed => categoryParsed.NumeCategorie == text);
                }
            }

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
