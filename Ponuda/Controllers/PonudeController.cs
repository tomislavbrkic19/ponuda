using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Ponuda.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Ponuda.Controllers
{
    public class PonudeController : Controller
    {
        private testDBEntities db = new testDBEntities();

        //public object artikli = null;


        // GET: Ponudes
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Ponude> svePonude = null;
            svePonude = db.Ponude.OrderBy(x => x.PonudaID).ToPagedList(pageIndex, pageSize);
            return View(svePonude);
        }

        // GET: Ponudes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponude ponude = db.Ponude.Find(id);
            if (ponude == null)
            {
                return HttpNotFound();
            }
            return View(ponude);
        }



        // GET: Ponude/StavkaEdit/5
        public ActionResult StavkaEdit(int? id)
        {
            Stavke ponude = db.Stavke.Find(id);
            if (ponude == null)
            {
                return HttpNotFound();
            }
            var artikl = new SelectList(db.Artikli.Where(x => x.ArtikalId == ponude.ArtikalId), "ArtikalId", "NazivArtikla");
            ViewBag.Artikli = artikl;
            return PartialView("StavkaEdit", ponude);
        }
        [HttpPost]
        public ActionResult StavkaEdit(Stavke stavka)
        {
            Stavke jednastavka = db.Stavke.Where(m => m.StavkaId == stavka.StavkaId).FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("kol:" + stavka.Kolicina);
            System.Diagnostics.Debug.WriteLine("PonudaId:" + stavka.PonudaId);
            System.Diagnostics.Debug.WriteLine("ArtikalId:" + stavka.ArtikalId);
            System.Diagnostics.Debug.WriteLine("UkupnaCijenaStavke:" + stavka.UkupnaCijenaStavke);
            System.Diagnostics.Debug.WriteLine("Kolicina:" + stavka.Kolicina);

            jednastavka.UkupnaCijenaStavke = stavka.UkupnaCijenaStavke;
            jednastavka.Kolicina = stavka.Kolicina;
            jednastavka.ArtikalId = stavka.ArtikalId;
            db.SaveChanges();


            //UPdate u bazi, stora...
            CalculatePonuda(stavka.PonudaId);
            return RedirectToAction("Index", "Ponude");
        }


        public ActionResult StavkaCreate(int PonudaId)
        {
            ViewBag.PonudaId = PonudaId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StavkaCreate([Bind(Include = "PonudaId,ArtikalId,Kolicina, UkupnaCijenaStavke")] Stavke stavka)
        {
            if (ModelState.IsValid)
            {
                
                db.Stavke.Add(stavka);
                db.SaveChanges();
                CalculatePonuda(stavka.PonudaId);
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        // GET: Ponudes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponude ponude = db.Ponude.Find(id);
            if (ponude == null)
            {
                return HttpNotFound();
            }
            return View(ponude);
        }

        // POST: Ponudes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PonudaID,UkupnaCijena,DatumPonude")] Ponude ponude)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ponude).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ponude);

        }

        public ActionResult Delete(int Stavkaid)
        {
            Stavke stavka = db.Stavke.Where(x => x.StavkaId ==Stavkaid).SingleOrDefault();
            int? IdPonude= stavka.PonudaId;
            db.Stavke.Remove(stavka);
            db.SaveChanges();
            CalculatePonuda(IdPonude);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetPonudaById(int PonudaId)
        {
            var lookup = db.Ponude.Where(x => x.PonudaID == PonudaId).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(lookup, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStavkaById(int StavkaId)
        {
            System.Diagnostics.Debug.WriteLine("Dobio sam StavkaID:" + StavkaId);
            var stavka = db.Stavke.Where(x => x.StavkaId == StavkaId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(stavka, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArtikls(int id, int pageIndex, int pageSize)
        {


            var query = (from c in db.Artikli
                         orderby c.NazivArtikla ascending
                         select c)
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize);
            return Json(query.ToList(), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetArtiklsAll()
        {

            var query = (from c in db.Artikli
                         orderby c.NazivArtikla ascending
                         select
                         c.NazivArtikla);
            return Json(query.ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetArtikalById(int id)
        {

            var query = (from c in db.Artikli where c.ArtikalId == id
                         select c);
            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePonuda()
        {

            try
            {
                Ponude ponuda = new Ponude();
                ponuda.UkupnaCijena = (decimal)0.00;
                ponuda.DatumPonude = DateTime.UtcNow;
                db.Ponude.Add(ponuda);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");



        }
        public static void CalculatePonuda(int? PonudaId)
        {
            //UPdate u bazi, stora...
            using (var conn = new SqlConnection(@";data source=.\SQLEXPRESS14;initial catalog=testDBNEW;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
            using (SqlCommand cmd = new SqlCommand("CalculateStavke", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PonudaId", PonudaId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
    
}
