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

namespace Supermarket.ViewModels.AdministratorRelated.Producers
{
    internal class DeleteProducersVM : BaseVM
    {

        Utilizatori userOperating;
        ProducersBLL producersBLL;

        public DeleteProducersVM(Utilizatori user, ProducersBLL producersBLLParam)
        {
            userOperating = user;
            producersBLL = producersBLLParam;

        }

        public ObservableCollection<Producatori> ProducersList
        {
            get => producersBLL.ProducersActive;
            set => producersBLL.ProducersActive = value;
        }

        Producatori selectedProducer;
        public Producatori SelectedProducer
        {
            get { return selectedProducer; }
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

        private ICommand deleteProducerCommand;

        public ICommand DeleteProducerCommand
        {
            get
            {
                if (deleteProducerCommand == null)
                {
                    deleteProducerCommand = new RelayCommand<object>(DeleteProducer);
                }

                return deleteProducerCommand;
            }
        }

        private void DeleteProducer(object obj)
        {
            if (SelectedProducer.NumeProducator == null || SelectedProducer.NumeProducator == "")
            {
                InfoMessage = "Select a producer!";
                InfoVisibility = Visibility.Visible;
                return;
            }

            producersBLL.DeleteProducer(SelectedProducer);

            SelectedProducer = new Producatori();

            InfoMessage = "Producer Deleted!";
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
