using ReleaseSpence.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class ImagenesController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult Index()
        {
            return View(db.Imagenes.OrderBy(i => i.nombre).ToList());
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Create()
        {
            ViewBag.idTipos = new MultiSelectList(db.TipoSensores, "idTipo", "nombre");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nombre,archivo,idTipos")] ImagenesCreate imagenes)
        {
            if (ModelState.IsValid)
            {
                int idimagen = ImagenesRep.Create(imagenes);
                Imagen_TipoSensor imagen_TipoSensor = new Imagen_TipoSensor();
                imagen_TipoSensor.idImagen = idimagen;
                imagenes.idTipos = imagenes.idTipos ?? new int[0];
                foreach (int idTipo in imagenes.idTipos)
                {
                    imagen_TipoSensor.idTipo = idTipo;
                    Imagen_TipoSensorRep.Create(imagen_TipoSensor);
                }
                if (imagenes.archivo != null && imagenes.archivo.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Images/Mapas"), idimagen.ToString() + ".jpg");
                    imagenes.archivo.SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            ViewBag.idTipos = new MultiSelectList(db.TipoSensores, "idTipo", "nombre");
            return View(imagenes);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagenes imagenes = db.Imagenes.Find(id);
            if (imagenes == null)
            {
                return HttpNotFound();
            }
            List<int> existentes = imagenes.Imagen_TipoSensor.Select(i => i.idTipo).ToList();
            ViewBag.idTipos = new MultiSelectList(db.TipoSensores, "idTipo", "nombre", existentes);
            return View(imagenes);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idImagen,nombre,archivo,idTipos")] Imagenes imagenes)
        {
            if (ModelState.IsValid)
            {
                ImagenesRep.Update(imagenes);
                imagenes.idTipos = imagenes.idTipos ?? new int[0];
                List<int> existentes = db.Imagenes.Find(imagenes.idImagen).Imagen_TipoSensor.Select(i => i.idTipo).ToList() ?? new List<int>();
                List<int> eliminar = existentes.Except(imagenes.idTipos).ToList() ?? new List<int>();
                List<int> agregar = imagenes.idTipos.Except(existentes).ToList() ?? new List<int>();
                foreach (int idTipo in eliminar)
                {
                    Imagen_TipoSensor objeto = new Imagen_TipoSensor();
                    objeto.idImagen = imagenes.idImagen;
                    objeto.idTipo = idTipo;
                    Imagen_TipoSensorRep.Delete(objeto);
                }
                foreach (int idTipo in agregar)
                {
                    Imagen_TipoSensor objeto = new Imagen_TipoSensor();
                    objeto.idImagen = imagenes.idImagen;
                    objeto.idTipo = idTipo;
                    Imagen_TipoSensorRep.Create(objeto);
                }
                if (imagenes.archivo != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Images/Mapas"), imagenes.idImagen.ToString() + ".jpg");
                    imagenes.archivo.SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            else
            {
                List<int> existentes = imagenes.Imagen_TipoSensor.Select(i => i.idTipo).ToList();
                ViewBag.idTipos = new MultiSelectList(db.TipoSensores, "idTipo", "nombre", existentes);
                return View(imagenes);
            }
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult Georeferenciar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagenes imagenes = db.Imagenes.Find(id);
            if (imagenes == null)
            {
                return HttpNotFound();
            }
            return View(imagenes);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult Georeferenciar([Bind(Include = "idImagen,mx,nx,my,ny")] Georeferenciar imagenes)
        {
            if (ModelState.IsValid)
            {
                ImagenesRep.Georeferenciar(imagenes);
                return RedirectToAction("Index");
            }
            else
            {
                return View(imagenes);
            }
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public PartialViewResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Imagenes imagenes = db.Imagenes.Find(id);
            return PartialView(imagenes);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var marcadoresSensores = db.Marcadores.Where(m => m.idImagen == id).Count();
            ////List<Marcadores> marcadorSenLista = db.Marcadores.Where(m => m.idImagen == id).ToList();
            //var marcadoresPuntos = db.MarcadoresMpz.Where(p => p.idImagen == id).Count();
            ////if (marcadoresSensores > 0 || marcadoresPuntos > 0)
            ////{
            if (ModelState.IsValid)
            {
                BorraMarcadoresMapa(id);
            }

            //}
            //var opt = db.Marcadores.Find();
            //db.Marcadores.Remove(opt);
            Imagenes imagenes = db.Imagenes.Find(id);
            db.Imagenes.Remove(imagenes);
            db.SaveChanges();
            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/Mapas"), imagenes.idImagen.ToString() + ".jpg")))
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Images/Mapas"), imagenes.idImagen.ToString() + ".jpg"));
            }
            

            return RedirectToAction("Index");
        }

        public void BorraMarcadoresMapa(int id)
        {
            var marcadoresSensores = db.Marcadores.Where(m => m.idImagen == id).Count();
            //List<Marcadores> marcadorSenLista = db.Marcadores.Where(m => m.idImagen == id).ToList();
            var marcadoresPuntos = db.MarcadoresMpz.Where(p => p.idImagen == id).Count();
            if (marcadoresSensores > 0 || marcadoresPuntos > 0)
            {
            Marcadores marcadorSensorU = new Marcadores();
            List<Marcadores> marcadorSensor = db.Marcadores.Where(x => x.idImagen == id).ToList();
            MarcadoresMpz marcadorPuntoU = new MarcadoresMpz();
            List<MarcadoresMpz> marcadorPunto = db.MarcadoresMpz.Where(y => y.idImagen == id).ToList();
            //marcadorSensor.idSensor = sensores.idSensor;
            if(marcadorSensor.Count() > 0)
            {
                foreach (var item in marcadorSensor)
                {
                    marcadorSensorU.idSensor = item.idSensor;
                    BorraMarcadorSensor(marcadorSensorU);
                }
            }
            if (marcadorPunto.Count() > 0) {
                foreach (var item in marcadorPunto)
                {
                    marcadorPuntoU.idPuntoMonitoreo = item.idPuntoMonitoreo;
                    BorraMarcadorPunto(marcadorPuntoU);
                }
            }
            }

            //BorraMarcadorSensor(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void BorraMarcadorSensor([Bind(Include = "idSensor")] Marcadores marcadorSensor)
        {
            if (ModelState.IsValid)
            {
                MarcadoresRep.Delete(marcadorSensor);
            }
        }
        private void BorraMarcadorPunto([Bind(Include = "idPuntoMonitoreo")] MarcadoresMpz marcadorPunto)
        {
            if (ModelState.IsValid)
            {
                MarcadoresMpzRep.Delete(marcadorPunto);
            }
        }
    }
}