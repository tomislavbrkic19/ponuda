using Ponuda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ponuda.ViewModel
{
    public class Stavka
    {
      
  
        public int StavkaId { get; set; }
        public Nullable<int> PonudaId { get; set; }
        public Nullable<int> ArtikalId { get; set; }
        public Nullable<int> Kolicina { get; set; }
        public Nullable<decimal> UkupnaCijenaStavke { get; set; }

        private Ponude Ponude { get; set; }
        public virtual Artikli Artikli { get; set; }
   

}
}