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
    
    public partial class Stocuri
    {
        public int StocID { get; set; }
        public Nullable<int> ProdusID { get; set; }
        public int Cantitate { get; set; }
        public string UnitateMasura { get; set; }
        public System.DateTime DataAprovizionare { get; set; }
        public System.DateTime DataExpirare { get; set; }
        public decimal PretAchizitie { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> PretVanzare { get; set; }
    
        public virtual Produse Produse { get; set; }
    }
}
