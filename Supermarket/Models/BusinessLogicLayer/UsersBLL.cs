using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Models.BusinessLogicLayer
{
    class UsersBLL
    {
        public UsersBLL()
        {
            Users = GetAllUsers();
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<Utilizatori> Users { get; set; }

        public ObservableCollection<Utilizatori> UsersActive { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Utilizatori> GetAllUsers()
        {
            List<Utilizatori> Users = context.Utilizatoris.ToList();

            ObservableCollection<Utilizatori> result = new ObservableCollection<Utilizatori>();
            UsersActive = new ObservableCollection<Utilizatori>();

            foreach (Utilizatori user in Users)
            {
                result.Add(user);

                if (user.IsActive)
                {
                    UsersActive.Add(user);
                }
            }

            return result;
        }

        public void AddMethod(object obj)
        {
            Utilizatori utilizator = obj as Utilizatori;

            if (utilizator != null)
            {
                if (string.IsNullOrEmpty(utilizator.NumeUtilizator))
                {
                    ErrorMessage = "Name is required";
                }

                if (!checkUniqueUser(utilizator.NumeUtilizator))
                {
                    return;
                }
                context.Utilizatoris.Add(utilizator);
                context.SaveChanges();
                utilizator.UtilizatorID = context.Utilizatoris.Max(item => item.UtilizatorID);
                Users.Add(utilizator);
                UsersActive.Add(utilizator);
                ErrorMessage = "";

            }
        }

        public void UpdateUser(object obj)
        {
            Utilizatori utilizator = obj as Utilizatori;

            if (utilizator != null)
            {
                if (string.IsNullOrEmpty(utilizator.NumeUtilizator))
                {
                    ErrorMessage = "Name is required";
                    return;
                }

                Users[Users.IndexOf(utilizator)] = utilizator;
                if (utilizator.IsActive)
                {
                    UsersActive.Add(utilizator);
                }
                else
                {
                    UsersActive.Remove(utilizator);
                }

                context.UpdateUser(utilizator.UtilizatorID, utilizator.NumeUtilizator, utilizator.Parola, utilizator.TipUtilizator, utilizator.IsActive);
                context.SaveChanges();
                ErrorMessage = "";

            }

        }

        public void DeleteUser(object obj)
        {
            Utilizatori utilizator = obj as Utilizatori;

            if (utilizator != null)
            {
                if (string.IsNullOrEmpty(utilizator.NumeUtilizator))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
                    context.DeactivateUser(utilizator.UtilizatorID);
                    context.SaveChanges();
                    UsersActive.Remove(utilizator);
                    Users[Users.IndexOf(utilizator)].IsActive = false;
                    ErrorMessage = "";
                }
            }
        }

        private bool checkUniqueUser(string userName)
        {
            foreach (Utilizatori user in Users)
            {
                if (user.NumeUtilizator == userName)
                {
                    if (!user.IsActive) //IN CASE THERE IS A USER NOT ACTIVE WE UPDATE IT TO ACTIVE INSTEAD OF ADDING IT AGAIN
                    {
                        UsersActive.Add(user);
                        Users[Users.IndexOf(user)].IsActive = true;
                        context.UpdateUser(user.UtilizatorID, user.NumeUtilizator, user.Parola, user.TipUtilizator, user.IsActive);
                        context.SaveChanges();
                    }
                    else
                    {
                        ErrorMessage = "Producer already exists";
                    }
                    return false;
                }
            }
            return true;
        }

        public Dictionary<DateTime, decimal> GetSalesPerDay(int utilizatorId, int month, int year)
        {
            // Get all receipts issued by the specified user in the selected month and year
            var receipts = context.BonuriCasas
                .Where(b => b.UtilizatorID == utilizatorId
                            && b.DataEliberarii.Month == month
                            && b.DataEliberarii.Year == year)
                .ToList();

            // Group receipts by day and calculate the total amount received each day
            var dailyIncome = receipts
                .GroupBy(b => b.DataEliberarii.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalIncome = g.Sum(b => b.DetaliiBons.Sum(db =>
                    {
                        var stock = context.Stocuris.FirstOrDefault(s => s.ProdusID == db.ProdusID);
                        var pricePerUnit = stock != null && stock.Cantitate > 0 ? (stock.PretVanzare ?? 0) / stock.Cantitate : 0;
                        return db.Cantitate * pricePerUnit;
                    }))
                })
                .ToDictionary(x => x.Date, x => x.TotalIncome);

            return dailyIncome;
        }
    }
}

