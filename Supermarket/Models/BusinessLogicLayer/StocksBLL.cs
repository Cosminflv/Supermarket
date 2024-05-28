using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    internal class StocksBLL
    {
        public StocksBLL() 
        {
            Stocks = GetAllStocks();
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<Stocuri> Stocks { get; set; }

        public ObservableCollection<Stocuri> StocksActive { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Stocuri> GetAllStocks()
        {
            List<Stocuri> stocks = context.Stocuris.ToList();

            ObservableCollection<Stocuri> result = new ObservableCollection<Stocuri>();
            StocksActive = new ObservableCollection<Stocuri>();

            foreach (Stocuri stock in stocks)
            {
                result.Add(stock);

                if (stock.IsActive)
                {
                    StocksActive.Add(stock);
                }
            }

            return result;
        }

        public bool SubstractStockQuantity(int stockID, int valueToSubstract)
        {
            bool hasBecomeInactive = false;
            context.SubtractQuantity(stockID, valueToSubstract);
            context.SaveChanges();

            Stocks[Stocks.IndexOf(Stocks.First(item => item.StocID == stockID))].Cantitate -= valueToSubstract;
            if(Stocks[Stocks.IndexOf(Stocks.First(item => item.StocID == stockID))].Cantitate == 0)
            {
            Stocks[Stocks.IndexOf(Stocks.First(item => item.StocID == stockID))].IsActive = false;
            context.SaveChanges();
                hasBecomeInactive = true;
            }

            UpdateProductIsActiveBasedOnStock(stockID);
            return hasBecomeInactive;
        }

        public void AddMethod(object obj)
        {
            Stocuri stock = obj as Stocuri;

            if (stock != null)
            {
                context.Stocuris.Add(stock);
                context.SaveChanges();
                stock.StocID = context.Stocuris.Max(item => item.StocID);
                Stocks.Add(stock);
                StocksActive.Add(stock);
                ErrorMessage = "";

            }
        }

        public void DeleteStock(object obj)
        {
            Stocuri stockToDelete = obj as Stocuri;

            if (stockToDelete != null)
            {
                context.DeactivateStoc(stockToDelete.StocID);
                context.SaveChanges();
                StocksActive.Remove(stockToDelete);
                Stocks[Stocks.IndexOf(stockToDelete)].IsActive = false;
                ErrorMessage = "";

            }
        }

        public bool IsProductAvailable(int productID, int selectedQuantity)
        {
            foreach (Stocuri stock in StocksActive)
            {
                if(stock.ProdusID == productID && stock.Cantitate - selectedQuantity >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEnoughStock(int productID, int selectedQuantity)
        {
            foreach (Stocuri stock in StocksActive)
            {
                if (stock.ProdusID == productID && stock.Cantitate - selectedQuantity >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateProductIsActiveBasedOnStock(int stockID)
        {
            Stocuri stockToUpdate = Stocks.FirstOrDefault(s => s.StocID == stockID);

            if (stockToUpdate != null)
            {
                Produse product = context.Produses.Find(stockToUpdate.ProdusID);

                if (product != null && stockToUpdate.IsActive != product.IsActive)
                {
                    product.IsActive = stockToUpdate.IsActive;
                    context.SaveChanges();

                    // Update in-memory collections (optional)
                    int productIndex = Stocks.IndexOf(Stocks.FirstOrDefault(s => s.ProdusID == product.ProdusID));
                    if (productIndex > -1)
                    {
                        Stocks[productIndex].Produse.IsActive = product.IsActive;
                    }
                }
            }
        }
    }
}
