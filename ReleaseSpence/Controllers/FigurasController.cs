using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReleaseSpence.Models;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class FigurasController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult Index(string buscar)
        {
            var figuras = db.Figuras.OrderBy(f => f.nombre).ToList();
            if(!String.IsNullOrEmpty(buscar)) figuras = figuras.Where(s => s.nombre.Contains(buscar)).ToList();
            return View(figuras);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public PartialViewResult Preview(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Figuras figuras = db.Figuras.Find(id);
            return PartialView(figuras);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="idFigura,tipo,nombre,size,color,borde,colorBorde,rotacion")] Figuras figuras)
        {
            if (ModelState.IsValid)
            {
                FigurasRep.Create(figuras);
                return RedirectToAction("Index");
            }
            return View(figuras);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Figuras figuras = db.Figuras.Find(id);
            if (figuras == null)
            {
                return HttpNotFound();
            }
            return View(figuras);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="idFigura,tipo,nombre,size,color,borde,colorBorde,rotacion")] Figuras figuras)
        {
            if (ModelState.IsValid)
            {
                FigurasRep.Update(figuras);
                return RedirectToAction("Index");
            }
            return View(figuras);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public PartialViewResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Figuras figuras = db.Figuras.Find(id);
            return PartialView(figuras);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Figuras figuras = db.Figuras.Find(id);
            db.Figuras.Remove(figuras);
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
    }
}