using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;


namespace Supermarket.Models.BusinessLogicLayer
{
    internal class RecieptsBLL
    {
        private ProductsBLL productsBLL;
        public RecieptsBLL(ProductsBLL productsBLL)
        {
            Reciepts = new ObservableCollection<BonuriCasa>(context.BonuriCasas.ToList());
            this.productsBLL = productsBLL;
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<BonuriCasa> Reciepts;

        public string ErrorMessage { get; set; }

        public int GetLastReceiptID()
        {
            return Reciepts.Last().BonID;
        }

        public void AddReceipt(DateTime releaseDay, int utilizatorID)
        {
            context.AddBonCasa(releaseDay, utilizatorID);
            context.SaveChanges();
            Reciepts = new ObservableCollection<BonuriCasa>(context.BonuriCasas.ToList());
        }

        public void AddReceiptDetail(int receiptID, int productID, int quantity)
        {
            context.AddDetaliuBon(receiptID, productID, quantity);
            context.SaveChanges();
        }

        private List<BonuriCasa> GetReceiptsFromDate(DateTime selectedDate)
        {
            // Retrieve all receipts from the specified date
            var receipts = context.BonuriCasas
                      .Where(bon => EntityFunctions.TruncateTime(bon.DataEliberarii) == selectedDate.Date)
                      .ToList();

            return receipts;
        }

        private decimal CalculateReceiptValue(BonuriCasa receipt)
        {
            decimal totalValue = 0;

            // Iterate over the details of the receipt and calculate the value of each product
            foreach (var detail in receipt.DetaliiBons)
            {
                // Calculate the value of the product using the ProductsBLL
                decimal productPrice = productsBLL.CalculateProductPrice(detail.ProdusID);
                totalValue += productPrice * detail.Cantitate;
            }

            return totalValue;
        }

        public BonuriCasa GetReceiptWithHighestValue(DateTime selectedDate)
        {
            var recieptsFromSelectedDate = GetReceiptsFromDate(selectedDate);
            BonuriCasa receiptWithHighestValue = null;
            decimal highestValue = decimal.MinValue;

            // Iterate over each receipt and calculate its total value
            foreach (var receipt in recieptsFromSelectedDate)
            {
                decimal receiptValue = CalculateReceiptValue(receipt);

                // Check if this receipt has a higher total value
                if (receiptValue > highestValue)
                {
                    highestValue = receiptValue;
                    receiptWithHighestValue = receipt;
                }
            }

            return receiptWithHighestValue;
        }
    }
}
