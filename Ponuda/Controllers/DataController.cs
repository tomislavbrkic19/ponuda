using Ponuda.Models;
using System.Linq;
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
            return Json(lookup.ToList());
        }

        [System.Web.Http.Route("api/Data/GetStavkaById")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetStavkaById(int StavkaId)
        {
            System.Diagnostics.Debug.WriteLine("Dobio sam StavkaID:" + StavkaId);
            var stavka = db.Stavke.Where(x => x.StavkaId == StavkaId).SingleOrDefault();
           
          
            return Json(stavka);
        }


    }
}
