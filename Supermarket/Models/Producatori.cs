//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Supermarket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Producatori
    {
        public Producatori()
        {
            this.Produses = new HashSet<Produse>();
        }
    
        public int ProducatorID { get; set; }
        public string NumeProducator { get; set; }
        public string TaraOrigine { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<Produse> Produses { get; set; }
    }
}
