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

            if (isBarCodeUnique(product))
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

                context.Produses.Add(product);
                context.SaveChanges();
                product.ProducatorID = context.Producatoris.Max(item => item.ProducatorID);
                Products.Add(product);
                ProductsActive.Add(product);
                ErrorMessage = "";
            }
        }

        private bool isBarCodeUnique(Produse productToCheck)
        {
            foreach(Produse product in Products)
            {
                if(product.NumeProdus == productToCheck.NumeProdus && product.ProdusID != productToCheck.ProdusID)
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

                if (!checkUniqueProductName(product))
                {
                    ErrorMessage = "Producer Exists";
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
                    context.DeactivateProducer(product.ProducatorID);
                    context.SaveChanges();
                    ProductsActive.Remove(product);
                    Products[Products.IndexOf(product)].IsActive = false;
                    ErrorMessage = "";
                }
            }
        }

        private bool checkUniqueProductName(Produse productToCheck)
        {
            foreach (Produse product in Products)
            {
                if (product.NumeProdus == productToCheck.NumeProdus && product.ProdusID != productToCheck.ProdusID)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
