using Supermarket.Commands;
using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels.AdministratorRelated.Reciepts
{
    internal class ViewRecieptVM : BaseVM
    {
        Utilizatori user;
        RecieptsBLL recieptsBLL;

        public ViewRecieptVM(Utilizatori user, RecieptsBLL recieptsBLLParam)
        {
            this.user = user;
            this.recieptsBLL = recieptsBLLParam;
            SelectedDate = DateTime.Now;
        }

        public ObservableCollection<DetaliiBon> ReceiptDetails { get; } = new ObservableCollection<DetaliiBon>();

        BonuriCasa highestValueReceipt;

        public BonuriCasa HighestValueReceipt
        {
            get
            {
                return highestValueReceipt;
            }
            set
            {
                highestValueReceipt = value;
                OnPropertyChanged("HighestValueReceipt");
            }
        }

        private System.DateTime selectedDate;

        public System.DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
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

        private ICommand displayReceiptCommand;

        public ICommand DisplayReceiptCommand
        {
            get
            {
                if (displayReceiptCommand == null)
                {
                    displayReceiptCommand = new RelayCommand<object>(GetHighesValueReciept);
                }

                return displayReceiptCommand;
            }
        }


        private void GetHighesValueReciept(object obj)
        {
            HighestValueReceipt = recieptsBLL.GetReceiptWithHighestValue(SelectedDate);

            if (HighestValueReceipt != null)
            {
                // Clear existing details
                ReceiptDetails.Clear();

                // Add details of the highest value receipt
                foreach (var detaliiBon in HighestValueReceipt.DetaliiBons)
                {
                    ReceiptDetails.Add(detaliiBon);
                }
            }
        }

        #region Navigation

        // COMMANDS

        private ICommand switchToAdministratorMenuCommand;

        public ICommand SwitchToAdministratorMenuCommand
        {
            get
            {
                if (switchToAdministratorMenuCommand == null)
                {
                    switchToAdministratorMenuCommand = new RelayPagesCommand(o => true, o => { OnSwitchToAdministratorMenu(); });
                }

                return switchToAdministratorMenuCommand;
            }
        }

        // DELEGATES

        public delegate void SwitchToAdministratorMenu();
        public SwitchToAdministratorMenu OnSwitchToAdministratorMenu { get; set; }

        #endregion
    }
}
