using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Models.BusinessLogicLayer
{
    class UsersBLL
    { 
        private SupermarketEntities1 context = new SupermarketEntities1();

        public ObservableCollection<Utilizatori> Users {  get; set; }  

        public string ErrorMessage { get; set; }

        public ObservableCollection<Utilizatori> GetAllUsers()
        {
            List<Utilizatori> users = context.Utilizatoris.ToList();

            ObservableCollection<Utilizatori> result = new ObservableCollection<Utilizatori>();

            foreach (Utilizatori user in users) {
                    result.Add(user);
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
                    ErrorMessage = "";
                }
            }
        }
    }
}
