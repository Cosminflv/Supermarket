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

        public ObservableCollection<Utilizatori> Users {  get; set; }

        public ObservableCollection<Utilizatori> UsersActive { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Utilizatori> GetAllUsers()
        {
            List<Utilizatori> Users = context.Utilizatoris.ToList();

            ObservableCollection<Utilizatori> result = new ObservableCollection<Utilizatori>();
            UsersActive = new ObservableCollection<Utilizatori>();

            foreach (Utilizatori user in Users) {
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

            if(utilizator != null)
            {
                if (string.IsNullOrEmpty(utilizator.NumeUtilizator))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
                    context.Utilizatoris.Add(utilizator);
                    context.SaveChanges();
                    utilizator.UtilizatorID = context.Utilizatoris.Max(item => item.UtilizatorID);
                    Users.Add(utilizator);
                    UsersActive.Add(utilizator);
                    ErrorMessage = "";
                }
            }
        }

        public void UpdateUser(object obj)
        {
            Utilizatori utilizator = obj as Utilizatori;

            if(utilizator != null)
            {
                if (string.IsNullOrEmpty(utilizator.NumeUtilizator))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
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
        }

        public void DeleteUser(object obj)
        {
            Utilizatori utilizator = obj as Utilizatori;

            if(utilizator != null)
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
    }
}
