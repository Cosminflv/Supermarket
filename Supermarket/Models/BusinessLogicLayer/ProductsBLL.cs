using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    internal class ProductsBLL
    {
        public ProductsBLL()
        {
            Products = GetAllProducts();
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<Produse> Products { get; set; }

        public ObservableCollection<Produse> ProductsActive { get; set; }

        public string ErrorMessage { get; set; }

        public decimal CalculateProductPrice(int produsID)
        {
            // Find the stock entry for the specified product ID
            var stockEntry = context.Stocuris.FirstOrDefault(stoc => stoc.ProdusID == produsID);

            if (stockEntry != null)
            {
                // Calculate the price of the product
                if (stockEntry.Cantitate != 0)
                {
                    decimal productPrice = (decimal)(stockEntry.PretVanzare ?? 0) / stockEntry.Cantitate;
                    return productPrice;
                }
                else
                {
                    // Cantitate is zero, handle as needed
                    return 0; // or throw an exception
                }
            }
            else
            {
                // No stock entry found for the specified product ID
                return 0; // or throw an exception
            }
        }

        public ObservableCollection<Produse> GetAllProducts()
        {
            List<Produse> products = context.Produses.ToList();

            ObservableCollection<Produse> result = new ObservableCollection<Produse>();
            ProductsActive = new ObservableCollection<Produse>();

            foreach (Produse product in products)
            {
                result.Add(product);

                if (product.IsActive)
                {
                    ProductsActive.Add(product);
                }
            }

            return result;
        }

        public void AddMethod(object obj)
        {
            Produse product = obj as Produse;

            if (!isBarCodeUnique(product))
            {
                ErrorMessage = "Barcode already exists";
                return;
            }

            if (product != null)
            {
                if (string.IsNullOrEmpty(product.NumeProdus))
                {
                    ErrorMessage = "ProducerName is required";
                    return;
                }

                // Ensure CategorieID is valid and fetch the corresponding category
                var category = context.Categoriis.Find(product.CategorieID);
                if (category == null)
                {
                    ErrorMessage = "Invalid Category ID";
                    return;
                }

                var producer = context.Producatoris.Find(product.ProducatorID);
                if (producer == null)
                {
                    ErrorMessage = "Invalid Producer ID";
                    return;
                }
                context.Produses.Add(product);
                context.SaveChanges();

                product.Categorii = category; // Assign the loaded category
                product.Producatori = producer; // Assign the loaded producer

                Products.Add(product);
                ProductsActive.Add(product);

                ErrorMessage = "";
            }
        }

        private bool isBarCodeUnique(Produse productToCheck)
        {
            foreach(Produse product in Products)
            {
                if(product.CodBare == productToCheck.CodBare && product.ProdusID != productToCheck.ProdusID)
                {
                   
                    return false;
                }
            }
            return true;
        }

        public void UpdateProduct(object obj)
        {
            Produse product = obj as Produse;

            if (product != null)
            {
                if (string.IsNullOrEmpty(product.NumeProdus))
                {
                    ErrorMessage = "Name is required";
                    return;
                }

                if (!isBarCodeUnique(product))
                {
                    ErrorMessage = "Barcode already exists";
                    return;
                }

                Products[Products.IndexOf(product)] = product;

                if (product.IsActive)
                {
                    ProductsActive.Add(product);
                }
                else
                {
                    ProductsActive.Remove(product);
                }

                context.UpdateProduct(product.ProdusID, product.NumeProdus, product.CodBare, product.CategorieID, product.ProducatorID);
                context.SaveChanges();
                ErrorMessage = "";
            }
        }

        public void DeleteProduct(object obj)
        {
            Produse product = obj as Produse;

            if (product != null)
            {
                if (string.IsNullOrEmpty(product.NumeProdus))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
                    context.DeactivateProduct(product.ProdusID);
                    context.SaveChanges();
                    ProductsActive.Remove(product);
                    Products[Products.IndexOf(product)].IsActive = false;
                    ErrorMessage = "";
                }
            }
        }
    }
}
