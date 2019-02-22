using Ponuda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ponuda.Controllers
{
    public class DataController : ApiController
    {
        private testDBEntities db = new testDBEntities();
      

        public IHttpActionResult Get(int id)
        {
            System.Diagnostics.Debug.WriteLine("UŠAO!");

          var  lookup = db.Ponude.Where(x => x.PonudaID == id).ToList();

            foreach (var p in lookup)
            {
                System.Diagnostics.Debug.WriteLine(p.DatumPonude.ToString()+"|+"+p.PonudaID.ToString()+"|"+p.Stavke.ToList().ToString());
            
            }
            System.Diagnostics.Debug.WriteLine("UŠAO1:"+lookup.ToList().ToString());
            //return lookup.ToList().ToString();
            return Json(lookup.ToList());
        }

    }
}
