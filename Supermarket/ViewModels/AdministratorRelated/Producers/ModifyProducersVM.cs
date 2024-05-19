using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.ViewModels.AdministratorRelated.Producers
{
    internal class ModifyProducersVM : BaseVM
    {
        public ModifyProducersVM(Utilizatori userOperating, ProducersBLL producersBLLParam)
        {
            userOperating = user;
            producersBLL = producersBLLParam;
            ProducersNames = new ObservableCollection<string>(ProducersList.Select(producerParsed => producerParsed.NumeProducator).ToList());
        }

        public ObservableCollection<Producatori> ProducersList
        {
            get => producersBLL.Producers;
            set => producersBLL.Producers = value;
        }

        public ObservableCollection<string> ProducersNames { get; set; }

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

        Utilizatori user;
        ProducersBLL producersBLL;

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

        private ICommand modifyProducerCommand;

        public ICommand ModifyProducerCommand
        {
            get
            {
                if (modifyProducerCommand == null)
                {
                    modifyProducerCommand = new RelayCommand<object>(ModifyProducer);
                }

                return modifyProducerCommand;
            }
        }

        private void ModifyProducer(object obj)
        {
            if(SelectedProducer == null)
            {
                InfoMessage = "Select a producer";
                InfoVisibility = Visibility.Visible;
                return;

            }

            producersBLL.UpdateProducer(SelectedProducer);

            if (producersBLL.ErrorMessage != "" && producersBLL.ErrorMessage != null)
            {
                InfoMessage = producersBLL.ErrorMessage;
                InfoVisibility = Visibility.Visible;
                return;
            }

            UpdateLocalData();
            InfoMessage = "Producer Modified!";
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
                    SelectedProducer = ProducersList.FirstOrDefault(producerParsed => producerParsed.NumeProducator == text);
                }
            }

        }

        private void UpdateLocalData()
        {
            ProducersNames = new ObservableCollection<string>(ProducersList.Select(categoryParsed => categoryParsed.NumeProducator).ToList());
            SelectedProducer = null;
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
