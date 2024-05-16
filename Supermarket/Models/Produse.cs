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
    
    public partial class Produse
    {
        public Produse()
        {
            this.DetaliiBons = new HashSet<DetaliiBon>();
            this.Stocuris = new HashSet<Stocuri>();
        }
    
        public int ProdusID { get; set; }
        public string NumeProdus { get; set; }
        public string CodBare { get; set; }
        public int CategorieID { get; set; }
        public int ProducatorID { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Categorii Categorii { get; set; }
        public virtual ICollection<DetaliiBon> DetaliiBons { get; set; }
        public virtual Producatori Producatori { get; set; }
        public virtual ICollection<Stocuri> Stocuris { get; set; }
    }
}
