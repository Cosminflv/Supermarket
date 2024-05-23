using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Producers
{
    internal class ProducerProductsVM : BaseVM
    {
        Utilizatori user;
        ProducersBLL producerBLL;
        CategoriesBLL categoriesBLL;

        public ProducerProductsVM(Utilizatori userOperating, ProducersBLL producersBLLParam, CategoriesBLL categoriesBLLParam)
        {
            user = userOperating;
            producerBLL = producersBLLParam;
            categoriesBLL = categoriesBLLParam;
            Producers = producerBLL.ProducersActive;
        }

        ObservableCollection<Producatori> producers;

        Dictionary<int, List<Produse>> groupedAndSortedProducts;

        public CategoriesBLL CategoriesBLL
        {
            get { return categoriesBLL; }
            set
            {
                categoriesBLL = value;
                OnPropertyChanged("CategoriesBLL");
            }
        }

        public Dictionary<int, List<Produse>> GroupedAndSortedProducts
        {
            get
            {
                return groupedAndSortedProducts;
            }
            set
            {
                groupedAndSortedProducts = value;
                OnPropertyChanged("GroupedAndSortedProducts");
            }
        }



        public ObservableCollection<Producatori> Producers
        {
            get => producers;
            set
            {
                producers = value;
                OnPropertyChanged("Producers");
            }
        }

        Producatori selectedProducer;

        public Producatori SelectedProducer
        {
            get => selectedProducer;
            set
            {
                selectedProducer = value;
                OnPropertyChanged("SelectedProducer");
            }
        }

        private ICommand producerSelectedCommand;

        public ICommand ProducerSelectedCommand
        {
            get
            {
                if (producerSelectedCommand == null)
                {
                    producerSelectedCommand = new RelayCommand<object>(ListProducts);
                }

                return producerSelectedCommand;
            }
        }

        private void ListProducts(object obj)
        {
            var groupedProduse = SelectedProducer.Produses
            .GroupBy(p => p.CategorieID)
            .ToDictionary(g => g.Key, g => g.OrderBy(p => p.NumeProdus).ToList());

            GroupedAndSortedProducts = groupedProduse;
        }

        #region Navigation

        // COMMANDS

        private ICommand switchToProducersCRUDMenuCommand;

        public ICommand SwitchToProducersCRUDMenuCommand
        {
            get
            {
                if (switchToProducersCRUDMenuCommand == null)
                {
                    switchToProducersCRUDMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToProducersCRUDMenu(); });
                }

                return switchToProducersCRUDMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToProducersCRUDMenu();
        public SwitchToProducersCRUDMenu OnSwitchToProducersCRUDMenu { get; set; }

        #endregion
    }
}
