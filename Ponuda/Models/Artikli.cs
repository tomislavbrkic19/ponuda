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
    
    public partial class Artikli
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Artikli()
        {
            this.Stavke = new HashSet<Stavke>();
        }
    
        public int ArtikalId { get; set; }
        public string NazivArtikla { get; set; }
        public Nullable<decimal> JedCijenaArtikla { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      private ICollection<Stavke> Stavke { get; set; }
    }
}
