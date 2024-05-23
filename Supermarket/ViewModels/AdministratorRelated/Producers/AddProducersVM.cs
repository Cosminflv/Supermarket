using Supermarket.Commands;
using Supermarket.Models;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.ViewModels.AdministratorRelated.Producers
{
    internal class AddProducersVM : BaseVM
    {
        Utilizatori user;

        ProducersBLL producersBLL;

        public AddProducersVM(Utilizatori userOperating, ProducersBLL producersBLLParam)
        {
            user = userOperating;
            producersBLL = producersBLLParam;
        }

        private string producerName;
        public string ProducerName
        {
            get => producerName;

            set
            {
                producerName = value;
                OnPropertyChanged("ProducerName");
            }
        }

        private string countryOrigin;

        public string CountryOrigin
        {
            get => countryOrigin;

            set
            {
                countryOrigin = value;
                OnPropertyChanged("CountryOrigin");
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

        private ICommand addProducerCommand;

        public ICommand AddProducerCommand
        {
            get
            {
                if (addProducerCommand == null)
                {
                    addProducerCommand = new RelayCommand<object>(AddProducer);
                }

                return addProducerCommand;
            }
        }

        private void AddProducer(object obj)
        {
            Producatori producer = new Producatori();
            producer.NumeProducator = ProducerName;
            producer.TaraOrigine = CountryOrigin;
            producer.IsActive = true;
            producersBLL.AddMethod(producer);

            if (producersBLL.ErrorMessage != "" && producersBLL.ErrorMessage != null)
            {
                InfoMessage = producersBLL.ErrorMessage;
                InfoVisibility = Visibility.Visible;
                return;
            }

            InfoVisibility = Visibility.Visible;
            InfoMessage = "Producer Added!";

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

            ProducerName = "";
            CountryOrigin = "";
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
