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


namespace Ponuda.Controllers
{
    public class PonudeController : Controller
    {
        private testDBEntities db = new testDBEntities();


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
            return PartialView("StavkaEdit", ponude);

        }
        [HttpPost]
        public ActionResult StavkaEdit(Stavke stavka)

        {

            Stavke jednastavka = db.Stavke.Where(m => m.StavkaId == stavka.StavkaId).FirstOrDefault();

            jednastavka.UkupnaCijenaStavke = stavka.UkupnaCijenaStavke;
            jednastavka.Kolicina = stavka.Kolicina;



            db.SaveChanges();

            return RedirectToAction("Index", "Ponude");



        }






        // GET: Ponudes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ponudes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PonudaID,UkupnaCijena,DatumPonude")] Ponude ponude)
        {
            if (ModelState.IsValid)
            {
                db.Ponude.Add(ponude);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ponude);
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

        // GET: Ponudes/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Ponudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ponude ponude = db.Ponude.Find(id);
            db.Ponude.Remove(ponude);
            db.SaveChanges();
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
    }
    
}
