using System;
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
    }
}
