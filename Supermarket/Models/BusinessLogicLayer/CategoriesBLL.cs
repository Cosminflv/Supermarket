using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    internal class CategoriesBLL
    {
        public CategoriesBLL()
        {
            Categories = GetAllCategories();
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<Categorii> Categories { get; set; }

        public ObservableCollection<Categorii> CategoriesActive { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Categorii> GetAllCategories()
        {
            List<Categorii> categories = context.Categoriis.ToList();

            ObservableCollection<Categorii> result = new ObservableCollection<Categorii>();
            CategoriesActive = new ObservableCollection<Categorii>();

            foreach (Categorii category in categories)
            {
                result.Add(category);

                if (category.IsActive)
                {
                    CategoriesActive.Add(category);
                }
            }

            return result;
        }

        public void AddMethod(object obj)
        {
            Categorii category = obj as Categorii;

            if (category != null)
            {
                if (string.IsNullOrEmpty(category.NumeCategorie))
                {
                    ErrorMessage = "CategoryName is required";
                    return;
                }
                if (!checkUniqueCategory(category.NumeCategorie))
                {
                    return;
                }

                context.Categoriis.Add(category);
                context.SaveChanges();
                category.CategorieID = context.Categoriis.Max(item => item.CategorieID);
                Categories.Add(category);
                CategoriesActive.Add(category);
                ErrorMessage = "";

            }
        }

        public void UpdateCategory(object obj)
        {
            Categorii category = obj as Categorii;

            if (category != null)
            {
                if (string.IsNullOrEmpty(category.NumeCategorie))
                {
                    ErrorMessage = "Name is required";
                    return;
                }

                if (!checkUniqueCategoryName(category))
                {
                    ErrorMessage = "Category Exists";
                    return;
                }

                Categories[Categories.IndexOf(category)] = category;

                if (category.IsActive)
                {
                    CategoriesActive.Add(category);
                }
                else
                {
                    CategoriesActive.Remove(category);
                }

                context.UpdateCategory(category.CategorieID, category.NumeCategorie, category.IsActive);
                context.SaveChanges();
                ErrorMessage = "";


            }
        }

        public void DeleteCategory(object obj)
        {
            Categorii category = obj as Categorii;

            if (category != null)
            {
                if (string.IsNullOrEmpty(category.NumeCategorie))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
                    context.DeactivateCategory(category.CategorieID);
                    context.SaveChanges();
                    CategoriesActive.Remove(category);
                    Categories[Categories.IndexOf(category)].IsActive = false;
                    ErrorMessage = "";
                }
            }
        }

        private bool checkUniqueCategoryName(Categorii categoryToCheck)
        {
            foreach(Categorii category in Categories)
            {
                if(category.NumeCategorie == categoryToCheck.NumeCategorie && category.CategorieID != categoryToCheck.CategorieID)
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkUniqueCategory(string categoryName)
        {
            foreach (Categorii category in Categories)
            {
                if (category.NumeCategorie == categoryName)
                {
                    if (!category.IsActive) //IN CASE THERE IS A CATEGORY NOT ACTIVE WE UPDATE IT TO ACTIVE INSTEAD OF ADDING IT AGAIN
                    {
                        CategoriesActive.Add(category);
                        Categories[Categories.IndexOf(category)].IsActive = true;
                        context.UpdateCategory(category.CategorieID, category.NumeCategorie, category.IsActive);
                        context.SaveChanges();
                    } else
                    {
                        ErrorMessage = "Category already exists";
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
