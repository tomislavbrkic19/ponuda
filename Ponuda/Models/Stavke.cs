//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ponuda.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stavke
    {
        public int StavkaId { get; set; }
        public Nullable<int> PonudaId { get; set; }
        public Nullable<int> ArtikalId { get; set; }
        public Nullable<int> Kolicina { get; set; }
        public Nullable<decimal> UkupnaCijenaStavke { get; set; }
    
        public virtual Artikli Artikli { get; set; }
        private Ponude Ponude { get; set; }
    }
}
